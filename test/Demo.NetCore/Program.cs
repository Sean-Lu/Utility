using System;
using Demo.NetCore.Impls.Test;
using Sean.Core.Ioc;
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

            SimpleLocalLoggerBase.CustomLog = (level, msg, ex) =>
            {
                //Console.WriteLine($"{options.LogToConsole}|{options.LogToLocalFile}");
            };
            SimpleLocalLoggerBase.DateTimeFormat = time => time.ToLongDateTime();

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
