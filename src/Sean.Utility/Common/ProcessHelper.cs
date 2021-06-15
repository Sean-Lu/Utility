using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Sean.Utility.Enums;
using System.Linq;

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
        /// 根据当前进程名和进程的文件路径检查进程是否已经存在，存在则返回已经运行的实例（进程）。
        /// </summary>
        /// <returns></returns>
        public static List<Process> GetRunningInstance()
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            return processes?.Where(process => process.Id != current.Id && process.MainModule.FileName == current.MainModule.FileName).ToList();
        }

        /// <summary>
        /// 执行指定的应用程序
        /// </summary>
        /// <param name="path">应用程序的文件路径</param>
        /// <param name="arguments">启动应用程序时的参数</param>
        /// <param name="outputDataReceived"></param>
        /// <param name="errorDataReceived"></param>
        /// <param name="createNoWindow">true:不显示窗口，false:不显示窗口</param>
        /// <returns></returns>
        public static void RunProcess(string path, string arguments, DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived, bool createNoWindow = true)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = createNoWindow;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();

                //output = process.StandardOutput.ReadToEnd();
                //error = process.StandardError.ReadToEnd();

                if (outputDataReceived != null)
                {
                    process.BeginOutputReadLine();
                    process.OutputDataReceived += outputDataReceived;
                }
                if (errorDataReceived != null)
                {
                    process.BeginErrorReadLine();
                    process.ErrorDataReceived += errorDataReceived;
                }

                process.WaitForExit();
                process.Close();
            }
        }
        /// <summary>
        /// 通过 cmd.exe 执行指定的应用程序
        /// </summary>
        /// <param name="path">应用程序的文件路径</param>
        /// <param name="arguments">启动应用程序时的参数</param>
        /// <param name="outputDataReceived"></param>
        /// <param name="errorDataReceived"></param>
        /// <param name="createNoWindow">true:不显示窗口，false:不显示窗口</param>
        /// <returns></returns>
        public static void RunProcessByCmd(string path, string arguments, DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived, bool createNoWindow = true)
        {
            path = Path.GetFullPath(path);

            //CmdHelper.RunCmd(new[]
            //{
            //    $"{Path.GetPathRoot(path).TrimEnd('\\')}",//1. 切换盘符
            //    $"cd \"{Path.GetDirectoryName(path)}\"",//2. 切换目录
            //    $"{Path.GetFileName(path)} {arguments}"//3. 运行程序
            //}, outputDataReceived, errorDataReceived, createNoWindow: createNoWindow);

            CmdHelper.RunCmd(new[]
            {
                $"{Path.GetFileName(path)}{(!string.IsNullOrWhiteSpace(arguments)?$" {arguments}":string.Empty)}"
            }, outputDataReceived, errorDataReceived, Path.GetDirectoryName(path), createNoWindow);
        }
    }
}
