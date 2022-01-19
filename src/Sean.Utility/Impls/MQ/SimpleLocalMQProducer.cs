using System;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.MQ
{
    /// <summary>
    /// 生产者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleLocalMQProducer<T> : SimpleLocalMQBase<T>, ISimpleLocalMQProducer<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mq"></param>
        public SimpleLocalMQProducer(ISimpleLocalMQ<T> mq) : base(mq)
        {
            _mq.AddProducer(this);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public virtual void Send(T data)
        {
            _mq.Send(data);
        }

        public void Dispose()
        {
            _mq.RemoveProducer(this);
        }
    }
}