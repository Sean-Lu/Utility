using Sean.Utility.Contracts;

namespace Sean.Utility.Impls.Queue
{
    /// <summary>
    /// 生产者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleLocalQueueProducer<T>
    {
        private readonly ISimpleLocalQueue<T> _queue;

        public SimpleLocalQueueProducer(ISimpleLocalQueue<T> queue)
        {
            _queue = queue;
        }

        public void Send(T data)
        {
            _queue.Queue.Enqueue(data);
            _queue.WaitHandler.Set();
        }
    }
}