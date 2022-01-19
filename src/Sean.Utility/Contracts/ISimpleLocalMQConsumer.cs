using System;
using System.Threading.Tasks;
using Sean.Utility.Impls.MQ;
using Sean.Utility.Impls.Queue;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalMQConsumer : ISimpleService, IDisposable
    {
        string Identity { get; }
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