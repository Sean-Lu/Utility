using Sean.Utility.Job;
using System;
using Demo.NetCore.Contracts;

namespace Demo.NetCore.Impls.Test.Job
{
    public class CronExpressionTest : ISimpleDo
    {
        public void Execute()
        {
            //string cron = "*/2 * * * * ?";// 每2秒钟执行1次
            string cron = "0/2 * * * * ?";// 每2秒钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            Console.WriteLine($"Now is:           {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            DateTime nextExecutionTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
                Console.WriteLine($"Next DateTime is: {nextExecutionTime:yyyy-MM-dd HH:mm:ss}");
            }
        }
    }
}
