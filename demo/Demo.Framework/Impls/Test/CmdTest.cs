using System;
using System.Threading;
using Sean.Utility;
using Sean.Utility.Contracts;

namespace Demo.Framework.Impls.Test
{
    public class CmdTest : ISimpleDo
    {
        public void Execute()
        {
            // 分配控制台
            CmdHelper.AllocConsole();

            Console.WriteLine("在控制台输出测试内容");

            Thread.Sleep(3000);

            // 释放控制台
            CmdHelper.FreeConsole();
        }
    }
}
