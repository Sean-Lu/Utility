using System;
using System.Threading;
using Sean.Utility.Contracts;

namespace Sean.Utility.Impls.Queue
{
    /// <summary>
    /// 消费者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleLocalQueueConsumer<T>
    {
        /// <summary>
        /// 接收消息的事件
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;

        private readonly ISimpleLocalQueue<T> _queue;
        private Thread _consumerThread;
        private bool _isConsumerStarted;

        public SimpleLocalQueueConsumer(ISimpleLocalQueue<T> queue, EventHandler<MessageReceivedEventArgs<T>> messageReceived = null)
        {
            _queue = queue;

            if (messageReceived != null)
            {
                MessageReceived += messageReceived;
            }
        }

        public void Start()
        {
            if (_isConsumerStarted)
            {
                return;
            }

            _isConsumerStarted = true;
            _consumerThread = new Thread(ConsumeItems) { IsBackground = true };
            _consumerThread.Start();
        }

        public void Stop()
        {
            if (!_isConsumerStarted)
            {
                return;
            }

            _isConsumerStarted = false;
            //if (_consumerThread != null && _consumerThread.IsAlive)
            //{
            //    _consumerThread.Abort();// 会报错
            //}
        }

        private void ConsumeItems()
        {
            if (MessageReceived == null)
            {
                throw new ArgumentNullException(nameof(MessageReceived));
            }

            while (_isConsumerStarted)
            {
                if (_queue.Queue.IsEmpty)
                {
                    _queue.WaitHandler.WaitOne();
                }
                //Console.WriteLine("##############################");
                if (_queue.Queue.TryDequeue(out var item))
                {
                    MessageReceived(this, new MessageReceivedEventArgs<T>(item));
                }
            }
            //Console.WriteLine("已经关闭消费者");
        }
    }
}