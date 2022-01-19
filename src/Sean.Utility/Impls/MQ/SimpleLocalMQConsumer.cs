using System;
using System.Threading.Tasks;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.MQ
{
    /// <summary>
    /// 消费者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleLocalMQConsumer<T> : SimpleLocalMQBase<T>, ISimpleLocalMQConsumer<T>
    {
        public bool IsStarted => _isStarted;

        /// <summary>
        /// 接收消息的事件
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;

        private bool _isStarted;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mq"></param>
        /// <param name="messageReceived"></param>
        public SimpleLocalMQConsumer(ISimpleLocalMQ<T> mq, EventHandler<MessageReceivedEventArgs<T>> messageReceived = null) : base(mq)
        {
            if (messageReceived != null)
            {
                MessageReceived += messageReceived;
            }
        }

        /// <summary>
        /// 开始消费
        /// </summary>
        public virtual void Start()
        {
            if (_isStarted)
            {
                return;
            }

            _mq.AddConsumer(this);

            _isStarted = true;
        }

        /// <summary>
        /// 停止消费
        /// </summary>
        public virtual void Stop()
        {
            if (!_isStarted)
            {
                return;
            }

            _mq.RemoveConsumer(this);

            _isStarted = false;
        }

        /// <summary>
        /// 消费消息的异步回调方法
        /// </summary>
        /// <param name="data"></param>
        public virtual Task ConsumeDataCallBackAsync(T data)
        {
            if (MessageReceived == null)
            {
                return default;
            }

            return Task.Factory.StartNew(() =>
            {
                var args = new MessageReceivedEventArgs<T>(data);
                try
                {
                    MessageReceived(this, args);
                }
                finally
                {
                    if (args.NeedReconsume)
                    {
                        // 消息补偿（重试机制）
                        using (var producer = _mq.CreateProducer())
                        {
                            producer.Send(data);
                        }
                    }
                }
            });
        }

        public void Dispose()
        {
            Stop();
        }
    }
}