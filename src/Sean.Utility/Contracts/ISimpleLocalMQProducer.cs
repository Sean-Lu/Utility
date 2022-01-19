using System;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalMQProducer : IDisposable
    {
        string Identity { get; }
    }

    public interface ISimpleLocalMQProducer<T> : ISimpleLocalMQProducer
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        void Send(T data);
    }
}