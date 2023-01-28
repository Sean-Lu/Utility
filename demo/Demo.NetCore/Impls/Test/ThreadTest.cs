using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Impls.Test
{
    public class ThreadTest : ISimpleDo
    {
        public void Execute()
        {
            Console.WriteLine($"当前线程id：{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
