using Sean.Utility.Job;
using System;
using Demo.NetCore.Contracts;

namespace Demo.NetCore.Impls.Test.Job
{
    public class CronJobSchedulerTest : ISimpleDo
    {
        public void Execute()
        {
            // 等待用户输入以开始程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 开始执行定时任务");

            //string cron = "*/2 * * * * ?";// 每2秒钟执行1次
            string cron = "0/2 * * * * ?";// 每2秒钟执行1次

            // 创建Cron任务调度器
            var scheduler = new CronJobScheduler("Cron定时任务测试", cron, (context) =>
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context.JobName}] Task executed.");
                //Thread.Sleep(5000);// 模拟并发执行
            }, true, true);

            // 启动定时任务
            scheduler.Start();

            // 等待用户输入以停止程序
            Console.ReadLine();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 停止执行定时任务");

            // 停止任务
            scheduler.Stop();
        }
    }
}
