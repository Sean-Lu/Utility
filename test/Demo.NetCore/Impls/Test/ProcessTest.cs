using System;
using System.Diagnostics;
using System.Linq;
using Sean.Utility.Common;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Impls.Test
{
    public class ProcessTest : ISimpleDo
    {
        public void Execute()
        {
            Console.WriteLine($"当前进程id：{Process.GetCurrentProcess().Id}");
        }

        public bool IsRunningInstance()
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            return processes.Any(process => process.Id != current.Id && process.MainModule.FileName == current.MainModule.FileName);
        }
    }
}
