using Demo.NetCore.Models;
using Sean.Utility.Common;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Format;
using Sean.Utility.Impls.Queue;
using System;
using System.Threading;

namespace Demo.NetCore.Impls.Test
{
    public class SimpleQueueTest : ISimpleDo
    {
        public void Execute()
        {
            SimpleQueue<TestModel>.OnDataConsumed += (sender, args) =>
            {
                Console.WriteLine(JsonHelper.SerializeIgnoreNullValue(args.QueueData));
                Console.WriteLine("===================================");
            };
            SimpleQueue<TestModel>.OnDataBatchConsumed += (sender, args) => 
            {
                args.QueueData.ForEach(c =>
                {
                    Console.WriteLine(JsonHelper.SerializeIgnoreNullValue(c));
                });
                Console.WriteLine("===================================");
            };
            SimpleQueue<TestModel>.OnException += (sender, args) =>
            {
                Console.WriteLine($"出现异常：{args.Data?.Message}");
            };

            var queue = new SimpleQueue<TestModel>(options =>
            {
                //options.QueueTriggerType = QueueTriggerType.Count;
                options.QueueTriggerType = QueueTriggerType.Count | QueueTriggerType.Timer | QueueTriggerType.Manual;
                options.TimerInterval = 1000;
                options.CountLimit = 3;
                options.MaxReconsumeCount = 5;
                options.ConsumeAsync = false;
            });

            var id = 0L;
            var now = DateTime.Now;
            //var list = new List<TestModel>();
            DelegateHelper.BatchFunc(() =>
            {
                Thread.Sleep(100);

                now = now.AddSeconds(1);
                var model = new TestModel
                {
                    Id = ++id,
                    CreateTime = now
                };
                queue.Enqueue(model);
                //list.Add(model);
                return false;
            }, 50);

            Console.WriteLine($"当前队列中的数据量：{queue.Count}");
            Console.WriteLine($"队列配置：{JsonHelper.Serialize(queue.Options)}");
        }
    }
}
