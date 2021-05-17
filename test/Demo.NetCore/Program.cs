using System;
using Demo.NetCore.Impls.Test;
using Sean.Core.Ioc;
using Sean.Utility.Common;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;
using Sean.Utility.Impls.Log;

namespace Demo.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.ConfigureServices(services =>
            {
                services.AddSimpleLocalLogger(configureOptions: options =>
                {
                    //options.LogToLocalFile = true;
                });
            });

            SimpleLocalLoggerBase.DateTimeFormat = time => time.ToLongDateTime();
            SimpleLocalLoggerBase.CustomOutputLog += (sender, eventArgs) =>
            {
                //Console.WriteLine($"自定义输出日志：{eventArgs.Options.LogToConsole}|{eventArgs.Options.LogToLocalFile}");
            };

            ILogger logger = ServiceManager.GetService<ISimpleLogger<Program>>();
            //logger.LogError("测试");

            ISimpleDo toDo = new SimpleQueueTest();
            toDo.Execute();

            //Console.WriteLine("===================================================");
            //Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
