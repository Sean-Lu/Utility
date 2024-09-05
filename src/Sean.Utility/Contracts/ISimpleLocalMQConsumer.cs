using System;
using System.Threading.Tasks;
using Sean.Utility.Impls.MQ;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalMQConsumer : IDisposable
    {
        string Identity { get; }

        bool IsStarted { get; }

        void Start();
        void Stop();
    }
    public interface ISimpleLocalMQConsumer<T> : ISimpleLocalMQConsumer
    {
        event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;

        /// <summary>
        /// 消费消息的异步回调方法
        /// </summary>
        /// <param name="data"></param>
        Task ConsumeDataCallBackAsync(T data);
    }
}