using Sean.Utility.Contracts;
using Sean.Utility.Job;
using System;

namespace Demo.NetCore.Impls.Test.Job
{
    public class SimpleTaskSchedulerTest : ISimpleDo
    {
        public void Execute()
        {
            // 等待用户输入以开始程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 开始执行定时任务");

            // 创建定时任务调度器，每3秒执行一次
            var scheduler = new SimpleTaskScheduler("测试定时任务", () =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: Task executed.");
            }, TimeSpan.FromSeconds(3), false);

            // 启动定时任务
            scheduler.Start();

            // 等待用户输入以停止程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 停止执行定时任务");

            // 停止定时任务
            scheduler.Stop();
        }
    }
}
