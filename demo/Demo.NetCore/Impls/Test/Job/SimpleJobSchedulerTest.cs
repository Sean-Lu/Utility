using Sean.Utility.Contracts;
using Sean.Utility.Job;
using System;
using System.Collections.Generic;
using System.Threading;
using Demo.NetCore.Contracts;

namespace Demo.NetCore.Impls.Test.Job
{
    public class SimpleJobSchedulerTest : ISimpleDo
    {
        private readonly List<SimpleJobScheduler> _schedulerList = new List<SimpleJobScheduler>();

        public void Execute()
        {
            // 等待用户输入以开始程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 开始执行定时任务");

            // 创建测试定时任务1：每3秒执行1次，禁止并发执行
            var scheduler = new SimpleJobScheduler("测试定时任务1", (context) =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context.JobName}] Task executed.");
                //Thread.Sleep(5000);// 模拟并发执行
            }, TimeSpan.FromSeconds(3), true, true);

            // 创建测试定时任务2：每3秒执行1次，禁止并发执行
            var scheduler2 = new SimpleJobScheduler("测试定时任务2", (context) =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context.JobName}] Task executed.");
                //Thread.Sleep(5000);// 模拟并发执行
            }, 3000, true, true);

            // 创建测试定时任务3：3秒后执行1次，仅执行1次
            var scheduler3 = new SimpleJobScheduler("测试定时任务3", (context) =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context.JobName}] Task executed.");
            }, TimeSpan.FromSeconds(3), Timeout.InfiniteTimeSpan);

            // 创建测试定时任务4：3秒后执行1次，仅执行1次
            var scheduler4 = new SimpleJobScheduler("测试定时任务4", (context) =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context.JobName}] Task executed.");
            }, 3000, Timeout.Infinite);

            _schedulerList.Add(scheduler);
            _schedulerList.Add(scheduler2);
            _schedulerList.Add(scheduler3);
            _schedulerList.Add(scheduler4);

            // 启动定时任务
            _schedulerList.ForEach(c => c.Start());

            // 等待用户输入以停止程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 停止执行定时任务");

            // 停止定时任务
            _schedulerList.ForEach(c => c.Stop());
        }
    }
}
