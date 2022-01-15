using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Contracts;

namespace Sean.Utility.Impls.Queue
{
    /// <summary>
    /// 基于 <see cref="ConcurrentQueue{T}"/> 实现的线程安全的本地队列
    /// </summary>
    public class SimpleLocalQueue<T> : ISimpleLocalQueue<T>, IDisposable
    {
        public ConcurrentQueue<T> Queue { get; } = new ConcurrentQueue<T>();

        public AutoResetEvent WaitHandler { get; } = new AutoResetEvent(false);

        public SimpleLocalQueue()
        {

        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <returns></returns>
        public SimpleLocalQueueProducer<T> CreateProducer()
        {
            return new SimpleLocalQueueProducer<T>(this);
        }
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <returns></returns>
        public SimpleLocalQueueConsumer<T> CreateConsumer(EventHandler<MessageReceivedEventArgs<T>> messageReceived = null)
        {
            return new SimpleLocalQueueConsumer<T>(this, messageReceived);
        }

        public void Dispose()
        {
            WaitHandler?.Dispose();
        }
    }
}
