using Demo.NetCore.Contracts;
using Demo.NetCore.Impls;
using Microsoft.Extensions.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;
using Sean.Utility.Format;
using Sean.Utility.Impls.Log;
using Sean.Utility.Serialize;
using System;
using Demo.NetCore.Impls.Test;

namespace Demo.NetCore
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            #region 依赖注入（DI）
            IocContainer.ConfigureServices((services, configuration) =>
            {
                services.AddSimpleLocalLogger();
                //services.AddSingleton<IJsonSerializer, JsonSerializer>();
                services.AddSingleton<IJsonSerializer, NewJsonSerializer>();
                JsonHelper.Serializer = NewJsonSerializer.Instance;
            });
            #endregion

            #region 配置Logger
            SimpleLocalLoggerBase.DateTimeFormat = time => time.ToLongDateTime();
            //SimpleLocalLoggerBase.DefaultLoggerOptions = new SimpleLocalLoggerOptions
            //{
            //    LogToConsole = true,
            //    LogToLocalFile = false
            //};
            SimpleLocalLoggerBase.CustomOutputLog += (sender, eventArgs) =>
            {
                //Console.WriteLine($"自定义输出日志：{JsonHelper.Serialize(eventArgs.Options)}");
            };
            //SimpleLocalLoggerBase.LogFileSeparatedByLogLevel = true;
            #endregion

            //_logger = SimpleLocalLoggerManager.GetCurrentClassLogger();
            _logger = IocContainer.GetService<ISimpleLogger<Program>>();
            //_logger.LogError("这是一条测试内容");

            //ISimpleDo toDo = new SimpleQueueTest();
            //ISimpleDo toDo = new SimpleLocalMQTest();
            ISimpleDo toDo = new SnowFlakeTest();
            Console.WriteLine(toDo.GetType());
            toDo.Execute();

            Console.WriteLine("--------------->Done.");
            Console.ReadLine();
        }
    }
}
