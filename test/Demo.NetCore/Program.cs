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
        private static ILogger _logger;

        static void Main(string[] args)
        {
            IocContainer.Instance.ConfigureServices(services =>
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

            _logger = IocContainer.Instance.GetService<ISimpleLogger<Program>>();
            //_logger.LogError("测试");

            //ISimpleDo toDo = new SimpleQueueTest();
            ISimpleDo toDo = new CmdTest();
            toDo.Execute();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
