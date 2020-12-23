using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Common;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Extensions;
using Sean.Utility.Timers;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#endif

namespace Sean.Utility.Impls.Queue
{
    /// <summary>
    /// Simple implementation of ConcurrentQueue&lt;T&gt;. Note: See enum <see cref="QueueTriggerType"/> for the supported queue trigger types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleQueue<T> : SimpleQueueBase, ISimpleQueue<T>
    {
        #region Event
        /// <summary>
        /// 消费单个数据的事件（如果消费失败，设置<see cref="DataConsumedEventArgs{T}.ShouldReconsume"/>为true，会根据<see cref="SimpleQueueOptions.MaxReconsumeCount"/>尝试重新消费）
        /// </summary>
        public static event EventHandler<DataConsumedEventArgs<T>> OnDataConsumed;
        /// <summary>
        /// 消费批量数据的事件（如果消费失败，设置<see cref="DataBatchConsumedEventArgs{T}.ShouldReconsume"/>为true，会根据<see cref="SimpleQueueOptions.MaxReconsumeCount"/>尝试重新消费）
        /// </summary>
        public static event EventHandler<DataBatchConsumedEventArgs<T>> OnDataBatchConsumed;
        /// <summary>
        /// 处理内部异常的事件
        /// </summary>
        public static event EventHandler<EventArgs<Exception>> OnException;
        #endregion

        /// <summary>
        /// 获取当前队列中数据的数量
        /// </summary>
        public int Count => _queue.Count;
        /// <summary>
        /// 当前队列是否为空
        /// </summary>
        public bool IsEmpty => _queue.IsEmpty;
        /// <summary>
        /// 队列配置
        /// </summary>
        public SimpleQueueOptions Options => _options;

        #region Private Fields
        /// <summary>
        /// 队列（线程安全）
        /// </summary>
        private readonly ConcurrentQueue<SimpleQueueData<T>> _queue = new ConcurrentQueue<SimpleQueueData<T>>();

        /// <summary>
        /// 队列配置
        /// </summary>
        private readonly SimpleQueueOptions _options;
        /// <summary>
        /// 定时器
        /// </summary>
        private readonly TimerExt _timer;

        /// <summary>
        /// 生产者的状态
        /// </summary>
        private int _queueProducerStatus;
        /// <summary>
        /// 消费者的状态
        /// </summary>
        private int _queueConsumerStatus;

        /// <summary>
        /// 运行中
        /// </summary>
        private const int Running = 1;
        /// <summary>
        /// 等待
        /// </summary>
        private const int Waiting = 0;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleQueue() : this(null)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public SimpleQueue(Action<SimpleQueueOptions> options)
        {
#if NETSTANDARD
            _options = ServiceProvider?.GetService<IOptionsMonitor<SimpleQueueOptions>>().CurrentValue ?? new SimpleQueueOptions();
#else
            _options = new SimpleQueueOptions();
#endif

            options?.Invoke(_options);

            if ((_options.QueueTriggerType & QueueTriggerType.Count) == QueueTriggerType.Count)
            {
                if (_options.CountLimit <= 0)
                {
                    throw new Exception($"The value of {nameof(_options.CountLimit)} must be greater than 0.");
                }
            }

            if ((_options.QueueTriggerType & QueueTriggerType.Timer) == QueueTriggerType.Timer)
            {
                if (_options.TimerInterval <= 0)
                {
                    throw new Exception($"The value of {nameof(_options.TimerInterval)} must be greater than 0.");
                }

                _timer = new TimerExt(_options.TimerInterval, disallowConcurrentExecution: false)
                {
                    Execute = TimerExecute
                };
                _timer.Start();
            }
        }

        /// <summary>
        /// 将单个数据插入队列
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Enqueue(new SimpleQueueData<T>(item));
        }
        /// <summary>
        /// 将单个数据插入队列
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(SimpleQueueData<T> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item.Data == null) throw new ArgumentNullException($"{nameof(item)}.{nameof(item.Data)}");

            ExceptionHelper.TryCatchFinal(() =>
            {
                if (_options.QueueTriggerType == QueueTriggerType.Immediate)
                {
                    // 不走队列，直接处理数据
                    ExecuteConsume(() =>
                    {
                        Consume(new List<SimpleQueueData<T>> { item }, QueueTriggerType.Immediate);
                    }, false);
                    return;
                }

                _queue.Enqueue(item);

                if ((_options.QueueTriggerType & QueueTriggerType.Count) == QueueTriggerType.Count && Count >= _options.CountLimit)
                {
                    // 数量达到上限
                    ExecuteConsume(() =>
                    {
                        ExecuteConsume(Math.Max(1, _options.CountLimit), QueueTriggerType.Count);
                    });
                }
            }, exception =>
            {
                if (OnException != null)
                {
                    OnException(this, new EventArgs<Exception>(exception));
                    return false;
                }
                return true;
            });
        }
        /// <summary>
        /// 将批量数据插入队列
        /// </summary>
        /// <param name="items"></param>
        public void Enqueue(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            Enqueue(items.Select(item => new SimpleQueueData<T>(item)));
        }
        /// <summary>
        /// 将批量数据插入队列
        /// </summary>
        /// <param name="items"></param>
        public void Enqueue(IEnumerable<SimpleQueueData<T>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any(c => c.Data != null))
            {
                return;
            }

            // 过滤无效数据
            if (items.Any(c => c.Data == null))
            {
                items = items.Where(c => c.Data != null);
            }

            ExceptionHelper.TryCatchFinal(() =>
            {
                if (_options.QueueTriggerType == QueueTriggerType.Immediate)
                {
                    // 不走队列，直接处理数据
                    ExecuteConsume(() =>
                    {
                        Consume(items, QueueTriggerType.Immediate);
                    }, false);
                    return;
                }

                foreach (var item in items)
                {
                    Enqueue(item);
                }
            }, exception =>
            {
                if (OnException != null)
                {
                    OnException(this, new EventArgs<Exception>(exception));
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 手动执行1次消费（必须先设置允许手动触发消费：<see cref="SimpleQueueOptions.QueueTriggerType"/> |= QueueTriggerType.Manual;）
        /// </summary>
        /// <param name="count">消费的数量（小于0：消费全部数据，大于0：消费部分数据）</param>
        /// <returns>本次队列中实际被消费的数量</returns>
        public int ExecuteConsume(int count)
        {
            if ((_options.QueueTriggerType & QueueTriggerType.Manual) != QueueTriggerType.Manual)
            {
                throw new Exception("Manual consume is not allowed.");
            }

            var result = 0;
            ExecuteConsume(() =>
            {
                result = ExecuteConsume(count, QueueTriggerType.Manual);
            });
            return result;
        }

        #region Private Methods
        /// <summary>
        /// 同步\异步消费
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isQueue"></param>
        private void ExecuteConsume(Action action, bool isQueue = true)
        {
            if (action == null || isQueue && IsEmpty)
            {
                return;
            }

            if (_options.ConsumeAsync)
            {
                // 异步消费
                Task.Factory.StartNewCatchException(action, exception =>
                {
                    OnException?.Invoke(this, new EventArgs<Exception>(exception));
                });
            }
            else
            {
                // 同步消费
                action();
            }
        }
        /// <summary>
        /// 手动执行1次消费
        /// </summary>
        /// <param name="count">消费的数量（小于0：消费全部数据，大于0：消费部分数据）</param>
        /// <param name="triggerType"></param>
        /// <returns>本次队列中实际被消费的数量</returns>
        private int ExecuteConsume(int count, QueueTriggerType triggerType)
        {
            if (count == 0 || IsEmpty /*|| Interlocked.CompareExchange(ref _queueConsumerStatus, Running, Waiting) != Waiting*/)// Interlocked原子操作：如果当前队列正在处于消费中的状态，会忽略本次操作，不会并发消费
            {
                return 0;
            }

            return ExceptionHelper.TryCatchFinal(() =>
            {
                var listWaitToConsume = new List<SimpleQueueData<T>>();
                var allCount = Count;
                if (allCount < 1)
                {
                    return 0;
                }
                DelegateHelper.BatchFunc(() =>
                {
                    if (!_queue.TryDequeue(out var result))
                    {
                        return true;
                    }

                    listWaitToConsume.Add(result);

                    return false;
                }, count < 0 || count > allCount ? allCount : count);

                var listCount = listWaitToConsume.Count;
                if (listCount > 0 && !Consume(listWaitToConsume, triggerType))
                {
                    return 0;
                }

                return listCount;
            }, exception =>
            {
                if (OnException != null)
                {
                    OnException(this, new EventArgs<Exception>(exception));
                    return 0;
                }
                throw exception;
            }, () =>
            {
                //Interlocked.Exchange(ref _queueConsumerStatus, Waiting);
            });
        }

        /// <summary>
        /// 消费批量数据
        /// </summary>
        /// <param name="items"></param>
        /// <param name="triggerType"></param>
        /// <returns></returns>
        private bool Consume(IEnumerable<SimpleQueueData<T>> items, QueueTriggerType triggerType)
        {
            if (items == null || !items.Any())
            {
                return false;
            }

            return ExceptionHelper.TryCatchFinal(() =>
            {
                if (OnDataBatchConsumed != null)
                {
                    var args = new DataBatchConsumedEventArgs<T>(items, triggerType);
                    OnDataBatchConsumed(this, args);
                    if (args.ShouldReconsume)
                    {
                        // 批量数据重新消费
                        items.ForEach(c => c.ReconsumeCount++);

                        var listRetry = new List<SimpleQueueData<T>>();
                        if (_options.MaxReconsumeCount > 0)
                        {
                            listRetry.AddRange(items.Where(c => c.ReconsumeCount < _options.MaxReconsumeCount));
                        }
                        else if (_options.MaxReconsumeCount < 0)
                        {
                            listRetry.AddRange(items);
                        }

                        if (listRetry.Any())
                        {
                            ReConsume(listRetry.ToArray());
                        }

                        return false;
                    }

                    return true;
                }
                else if (OnDataConsumed != null)
                {
                    foreach (var item in items)
                    {
                        var args = new DataConsumedEventArgs<T>(item, triggerType);
                        OnDataConsumed(this, args);
                        if (args.ShouldReconsume)
                        {
                            // 单个数据重新消费
                            item.ReconsumeCount++;

                            if (_options.MaxReconsumeCount > 0 && item.ReconsumeCount < _options.MaxReconsumeCount || _options.MaxReconsumeCount < 0)
                            {
                                ReConsume(item);
                            }

                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    throw new Exception($"To ensure that the data in the queue can be consumed normally, event {nameof(OnDataConsumed)} and event {nameof(OnDataBatchConsumed)} cannot be null at the same time (note: Event {nameof(OnDataBatchConsumed)} has a higher priority than event {nameof(OnDataConsumed)}).");
                }
            }, exception =>
            {
                if (OnException != null)
                {
                    OnException(this, new EventArgs<Exception>(exception));
                    return false;
                }

                throw exception;
            });
        }
        /// <summary>
        /// 重新丢入队列，等待下次消费
        /// </summary>
        /// <param name="items"></param>
        private void ReConsume(params SimpleQueueData<T>[] items)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            Task.Factory.StartNewCatchException(() =>
            {
                if (_options.ReconsumeSleepTime > 0)
                {
                    Thread.Sleep(_options.ReconsumeSleepTime);
                }
                Enqueue(items);
            }, exception =>
            {
                OnException?.Invoke(this, new EventArgs<Exception>(exception));
            });
        }

        /// <summary>
        /// 定时器触发
        /// </summary>
        private void TimerExecute()
        {
            ExecuteConsume(() =>
            {
                var count = -1;
                if ((_options.QueueTriggerType & QueueTriggerType.Count) == QueueTriggerType.Count && _options.CountLimit > 0)
                {
                    count = _options.CountLimit;
                }

                ExecuteConsume(count, QueueTriggerType.Timer);
            });
        }
        #endregion
    }
}
