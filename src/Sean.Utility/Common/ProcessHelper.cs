using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Sean.Utility.Enums;

namespace Sean.Utility.Common
{
    /// <summary>
    /// Process
    /// </summary>
    public class ProcessHelper
    {
        /// <summary>
        /// Get current process
        /// </summary>
        /// <returns></returns>
        public static Process GetCurrentProcess()
        {
            return Process.GetCurrentProcess();
        }
        /// <summary>
        /// Get the id of current process
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentProcessId()
        {
            return GetCurrentProcess().Id;
        }

        /// <summary>
        /// Get process by id
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static Process GetProcessById(int pid)
        {
            return Process.GetProcessById(pid);
        }

        /// <summary>
        /// Get processes by name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static Process[] GetProcessesByName(string processName)
        {
            return Process.GetProcessesByName(processName);
        }

        /// <summary>
        /// Get all process
        /// </summary>
        /// <returns></returns>
        public static Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }

        /// <summary>
        /// 根据指定进程名检查进程是否已经存在（不检查文件路径是否相同）
        /// </summary>
        /// <param name="name">进程名。示例：Application.ProductName</param>
        /// <returns></returns>
        public static bool IsProcessExist(string name)
        {
            var mutex = new Mutex(true, name, out var createNew);
            if (createNew)
            {
                mutex.ReleaseMutex();
            }
            return !createNew;
        }

        /// <summary>
        /// 根据当前进程名和进程的文件路径检查进程是否已经存在，存在则返回已经运行的实例（进程），不存在则返回null。
        /// </summary>
        /// <returns></returns>
        public static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name   
            foreach (Process process in processes)
            {
                //Ignore the current process   
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.   
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        //Return the other process instance.   
                        return process;
                    }
                }
            }
            //No other instance was found, return null. 
            return null;
        }
    }
}
