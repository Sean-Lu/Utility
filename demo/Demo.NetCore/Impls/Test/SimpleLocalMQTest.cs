using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.NetCore.Models;
using Newtonsoft.Json;
using Sean.Utility;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Impls.Log;
using Sean.Utility.Impls.MQ;
using Sean.Utility.Impls.Queue;

namespace Demo.NetCore.Impls.Test
{
    public class SimpleLocalMQTest : ISimpleDo
    {
        private readonly ILogger _logger;

        private readonly ISimpleLocalMQ<TestModel> _mq;// 队列
        private readonly ISimpleLocalMQConsumer<TestModel> _consumer;// 消费者
        private readonly ISimpleLocalMQConsumer<TestModel> _consumer2;// 消费者
        private readonly ISimpleLocalMQConsumer<TestModel> _consumer3;// 消费者
        //private readonly ISimpleLocalMQProducer<TestModel> _producer;// 生产者

        public SimpleLocalMQTest()
        {
            _logger = SimpleLocalLoggerManager.GetCurrentClassLogger();

            var name = "Test";
            var type = MQType.Topic;

            #region 消息队列
            _mq = new SimpleLocalMQ<TestModel>(name, type, options =>
            {

            });
            _mq.Start();
            #endregion

            #region 消费者
            _consumer = _mq.CreateConsumer();
            _consumer.MessageReceived += QueueOnMessageReceived;
            _consumer.Start();

            _consumer2 = _mq.CreateConsumer();
            _consumer2.MessageReceived += QueueOnMessageReceived;
            _consumer2.Start();

            _consumer3 = _mq.CreateConsumer();
            _consumer3.MessageReceived += QueueOnMessageReceived;
            _consumer3.Start();
            #endregion

            #region 生产者
            //_producer = _mq.CreateProducer();
            #endregion
        }

        public void Execute()
        {
            #region 单线程发送消息
            //SendTestMessage("Sean", false);
            #endregion

            #region 多线程发送消息
            var task1 = Task.Factory.StartNew(() =>
            {
                SendTestMessage("Sean-线程1", true);
            });
            var task2 = Task.Factory.StartNew(() =>
            {
                SendTestMessage("Sean-线程2", true);
            });
            Task.WaitAll(task1, task1);
            #endregion

            #region 释放资源
            //_producer.Dispose();
            _consumer.Dispose();
            _consumer2.Dispose();
            _consumer3.Dispose();
            _mq.Dispose();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loop">true：循环触发；false：手动触发</param>
        private void SendTestMessage(string name, bool loop = true)
        {
            using (var producer = _mq.CreateProducer())
            {
                var id = 1000;
                if (loop)
                {
                    //Random random = new Random();
                    while (true)
                    {
                        producer.Send(new TestModel
                        {
                            Id = id++,
                            Name = name,
                            Age = 21,
                            CreateTime = DateTime.Now
                        });

                        //Thread.Sleep(random.Next(1000, 3000));
                        Thread.Sleep(3000);
                    }
                }
                else
                {
                    string flag = null;
                    do
                    {
                        switch (flag)
                        {
                            case "1":// 发送消息
                                {
                                    producer.Send(new TestModel
                                    {
                                        Id = id++,
                                        Name = name,
                                        Age = 21,
                                        CreateTime = DateTime.Now
                                    });
                                    break;
                                }
                            case "2":// 停止服务
                                {
                                    _mq.Stop();

                                    //_consumer.Stop();
                                    //_consumer2.Stop();
                                    //_consumer3.Stop();
                                    break;
                                }
                            case "3":// 开启服务
                                {
                                    _mq.Start();

                                    //_consumer.Start();
                                    //_consumer2.Start();
                                    //_consumer3.Start();
                                    break;
                                }
                        }

                        flag = Console.ReadLine();
                    } while (flag == "1" || flag == "2" || flag == "3");
                }
            }
        }

        private void QueueOnMessageReceived(object sender, MessageReceivedEventArgs<TestModel> e)
        {
            try
            {
                var consumer = (ISimpleLocalMQConsumer)sender;
                Console.WriteLine($"[{consumer.Identity}][{Thread.CurrentThread.ManagedThreadId}]消费到消息：{JsonConvert.SerializeObject(e.Data)}");
            }
            catch (Exception ex)
            {
                e.NeedReconsume = true;
                _logger.LogError($"消费消息异常：{JsonConvert.SerializeObject(e.Data)}", ex);
            }
        }
    }
}
