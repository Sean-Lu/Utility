using System;
using System.Runtime.InteropServices;
using Demo.NetCore.Impls.Test;
using Demo.NetCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sean.Core.Ioc;
using Sean.Utility.Common;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;
using Sean.Utility.Format;
using Sean.Utility.Impls.Log;
using Sean.Utility.Serialize;

namespace Demo.NetCore
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            #region 依赖注入（DI）
            IocContainer.Instance.ConfigureServices(services =>
            {
                services.AddSimpleLocalLogger();
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
            //SimpleLocalLoggerBase.MsgFormat
            #endregion

            //_logger = SimpleLocalLoggerManager.GetCurrentClassLogger();
            _logger = IocContainer.Instance.GetService<ISimpleLogger<Program>>();
            //_logger.LogError("这是一条测试内容");

            //ISimpleDo toDo = new SimpleQueueTest();
            //ISimpleDo toDo = new ProcessTest();
            ISimpleDo toDo = new MultiThreadTest();
            toDo.Execute();

            Console.WriteLine("--------------->Done.");
            Console.ReadLine();
        }
    }
}
