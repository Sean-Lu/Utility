using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.NetCore.Models;
using Newtonsoft.Json;
using Sean.Utility;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Queue;

namespace Demo.NetCore.Impls.Test
{
    public class SimpleLocalQueueTest : ISimpleDo
    {
        private SimpleLocalQueue<TestModel> _queue;// 队列
        private SimpleLocalQueueProducer<TestModel> _producer;// 生产者
        private SimpleLocalQueueConsumer<TestModel> _consumer;// 消费者

        public void Execute()
        {
            #region 队列
            _queue = new SimpleLocalQueue<TestModel>();
            #endregion

            #region 消费者
            _consumer = _queue.CreateConsumer();
            _consumer.MessageReceived += QueueOnMessageReceived;
            _consumer.Start();
            #endregion

            #region 生产者
            _producer = _queue.CreateProducer();

            #region 单线程
            SendTestMessage("Sean", false);
            #endregion

            #region 多线程
            //Task.Factory.StartNew(() =>
            //{
            //    SendTestMessage("Sean-线程1", true);
            //});
            //Task.Factory.StartNew(() =>
            //{
            //    SendTestMessage("Sean-线程2", true);
            //});
            #endregion

            #endregion

            //_queue.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loop">true：循环；false：手动</param>
        private void SendTestMessage(string name, bool loop = true)
        {
            var id = 1000;
            if (loop)
            {
                Random random = new Random();
                while (true)
                {
                    _producer.Send(new TestModel
                    {
                        Id = id++,
                        Name = name,
                        Age = 21,
                        CreateTime = DateTime.Now
                    });

                    Thread.Sleep(random.Next(1000, 3000));
                }
            }
            else
            {
                string flag = null;
                do
                {
                    if (flag == "2")
                    {
                        _consumer.Stop();
                    }
                    if (flag == "3")
                    {
                        _consumer.Start();
                    }

                    _producer.Send(new TestModel
                    {
                        Id = id++,
                        Name = "Sean",
                        Age = 21,
                        CreateTime = DateTime.Now
                    });
                    flag = Console.ReadLine();
                } while (flag == "1" || flag == "2" || flag == "3");
            }
        }

        private void QueueOnMessageReceived(object sender, MessageReceivedEventArgs<TestModel> e)
        {
            Console.WriteLine($"消费到消息：{JsonConvert.SerializeObject(e.Data)}");
        }
    }
}
