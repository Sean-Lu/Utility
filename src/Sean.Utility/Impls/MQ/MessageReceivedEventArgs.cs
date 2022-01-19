using System;

namespace Sean.Utility.Impls.MQ
{
    public class MessageReceivedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; }
        /// <summary>
        /// 是否需要重新消费（重新丢入消息队列等待下次再次消费）
        /// <para><see cref="SimpleLocalMQOptions.ReconsumeWaitTime"/></para>
        /// <para><see cref="SimpleLocalMQOptions.MaxReconsumeTimes"/></para>
        /// </summary>
        public bool NeedReconsume { get; set; }

        public MessageReceivedEventArgs(T data)
        {
            Data = data;
        }
    }
}