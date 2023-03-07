//#define DEBUG_OUTPUT
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Extensions;

namespace Sean.Utility.Impls.MQ
{
    /// <summary>
    /// 基于 <see cref="ConcurrentQueue{T}"/> 实现的线程安全的本地消息队列
    /// <para>- 支持多线程下的生产者\消费者模式</para>
    /// <para>- 支持多生产者、多消费者</para>
    /// <para>- 暂不支持消息持久化</para>
    /// </summary>
    public class SimpleLocalMQ<T> : ISimpleLocalMQ<T>
    {
        /// <summary>
        /// 消息队列名称
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// 消息队列类型
        /// </summary>
        public MQType Type => _type;

        public bool IsStarted => _isStarted;

        private readonly string _name;
        private readonly MQType _type;
        public SimpleLocalMQOptions Options { get; }

        /// <summary>
        /// 线程安全的队列
        /// </summary>
        private ConcurrentQueue<T> _queue;
        /// <summary>
        /// 控制线程阻塞（消费线程 <see cref="ConsumeItemsThread"/> ）
        /// </summary>
        private AutoResetEvent _waitHandler;
        /// <summary>
        /// 消费线程 <see cref="ConsumeItemsThread"/> 是否处于等待状态（线程阻塞）
        /// </summary>
        private bool _consumeThreadWaiting;
        /// <summary>
        /// 生产者
        /// </summary>
        private List<ISimpleLocalMQProducer<T>> _producers;
        /// <summary>
        /// 消费者
        /// </summary>
        private List<ISimpleLocalMQConsumer<T>> _consumers;
        private bool _isStarted;
        private int _queueConsumedTotalCount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="configOptions"></param>
        /// <param name="name"></param>
        public SimpleLocalMQ(string name, MQType type, Action<SimpleLocalMQOptions> configOptions = null)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _type = type;
            Options = new SimpleLocalMQOptions();
            configOptions?.Invoke(Options);
        }

        /// <summary>
        /// 开启消息队列
        /// </summary>
        public virtual void Start()
        {
            if (_isStarted)
            {
                return;
            }

            if (_queue == null)
            {
                _queue = new ConcurrentQueue<T>();
            }
            if (_producers == null)
            {
                _producers = new List<ISimpleLocalMQProducer<T>>();
            }
            if (_consumers == null)
            {
                _consumers = new List<ISimpleLocalMQConsumer<T>>();
            }
            _waitHandler = new AutoResetEvent(false);

            _isStarted = true;
            Task.Factory.StartNew(ConsumeItemsThread);
        }

        /// <summary>
        /// 停止消息队列
        /// </summary>
        public virtual void Stop()
        {
            if (!_isStarted)
            {
                return;
            }

            _isStarted = false;
            //if (_isStarted != null && _isStarted.IsAlive)
            //{
            //    _isStarted.Abort();// 报错：System.PlatformNotSupportedException:“Thread abort is not supported on this platform.”
            //}

            _waitHandler?.Set();// 避免消费线程一直阻塞，无法结束线程
            _waitHandler?.Dispose();

            //for (var i = _producers.Count - 1; i >= 0; i--)
            //{
            //    var producer = _producers[i];
            //    producer?.Dispose();
            //}

            //for (var i = _consumers.Count - 1; i >= 0; i--)
            //{
            //    var consumer = _consumers[i];
            //    consumer?.Dispose();
            //}
        }

        #region 生产者
        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <returns></returns>
        public virtual ISimpleLocalMQProducer<T> CreateProducer()
        {
            return new SimpleLocalMQProducer<T>(this);
        }
        /// <summary>
        /// 添加生产者
        /// </summary>
        /// <param name="producer"></param>
        public virtual void AddProducer(ISimpleLocalMQProducer<T> producer)
        {
            _producers.Add(producer);
        }
        /// <summary>
        /// 移除生产者
        /// </summary>
        /// <param name="producer"></param>
        public virtual void RemoveProducer(ISimpleLocalMQProducer<T> producer)
        {
            _producers.Remove(producer);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public virtual void Send(T data)
        {
            //if (!_isStarted)
            //{
            //    throw new InvalidOperationException("消息队列未开启或已被关闭！");
            //}

            _queue.Enqueue(data);
#if DEBUG_OUTPUT
            if (!_consumeThreadWaiting)
            {
                DebugOutput("+++++++++++消费线程没有阻塞");
            }
#endif
            if (_consumeThreadWaiting && _isStarted && _consumers.Any())
            {
                _waitHandler.Set();
            }
        }
        #endregion

        #region 消费者
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <returns></returns>
        public virtual ISimpleLocalMQConsumer<T> CreateConsumer(EventHandler<MessageReceivedEventArgs<T>> messageReceived = null)
        {
            return new SimpleLocalMQConsumer<T>(this, messageReceived);
        }
        /// <summary>
        /// 添加消费者
        /// </summary>
        /// <param name="consumer"></param>
        public void AddConsumer(ISimpleLocalMQConsumer<T> consumer)
        {
            _consumers.Add(consumer);
            if (_consumeThreadWaiting && _isStarted && !_queue.IsEmpty)
            {
                _waitHandler.Set();
            }
        }
        /// <summary>
        /// 移除消费者
        /// </summary>
        /// <param name="consumer"></param>
        public void RemoveConsumer(ISimpleLocalMQConsumer<T> consumer)
        {
            _consumers.Remove(consumer);
        }
        /// <summary>
        /// 消费消息
        /// </summary>
        protected virtual void ConsumeItemsThread()
        {
            while (_isStarted)
            {
                if (_queue.IsEmpty || !_consumers.Any())
                {
#if DEBUG_OUTPUT
                    DebugOutput("消费线程阻塞");
#endif
                    _consumeThreadWaiting = true;
                    _waitHandler.WaitOne();// 线程阻塞
                    _consumeThreadWaiting = false;

                    if (!_isStarted)
                    {
#if DEBUG_OUTPUT
                        DebugOutput("消息队列已经停止，即将退出消费线程");
#endif
                        break;
                    }

                    if (_queue.IsEmpty || !_consumers.Any())// 考虑到多线程，这里需要再判断一次
                    {
#if DEBUG_OUTPUT
                        DebugOutput("+++++++++++消费线程阻塞恢复后，不满足继续消费的条件");
#endif
                        continue;
                    }
                }

#if DEBUG_OUTPUT
                DebugOutput("开始消费数据");
#endif
                switch (_type)
                {
                    case MQType.Queue:
                        {
                            if (_queue.TryDequeue(out var item))
                            {
                                _queueConsumedTotalCount++;
                                if (_consumers.Count > 1)
                                {
                                    // 均衡分配消息给不同的消费者
                                    _consumers[_queueConsumedTotalCount % _consumers.Count].ConsumeDataCallBackAsync(item);
                                }
                                else
                                {
                                    _consumers.FirstOrDefault()?.ConsumeDataCallBackAsync(item);
                                }
                            }

                            break;
                        }
                    case MQType.Topic:
                        {
                            if (_queue.TryDequeue(out var item))
                            {
                                _consumers.ForEach(consumer =>
                                {
                                    consumer?.ConsumeDataCallBackAsync(item);
                                });
                            }

                            break;
                        }
                    default:
                        throw new NotSupportedException($"Unsupported type: {_type}");
                }
            }
#if DEBUG_OUTPUT
            DebugOutput("消费线程已经结束");
#endif
        }
        #endregion

        public void Dispose()
        {
            Stop();
        }

#if DEBUG_OUTPUT
        private void DebugOutput(string msg)
        {
            Debug.WriteLine($"######################################## [{DateTime.Now.ToLongDateTime()}] [{this.GetType().Name}] [{Thread.CurrentThread.ManagedThreadId}] [QueueCount:{_queue.Count}] [ConsumerCount:{_consumers.Count}] [ProducerCount:{_producers.Count}] {msg}");
        }
#endif
    }
}
