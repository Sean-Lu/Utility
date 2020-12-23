using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace Sean.Utility.Common
{
    /// <summary>
    /// CMD控制台操作
    /// </summary>
    public class CmdHelper
    {
        #region DllImport
        /// <summary>
        /// 分配控制台（为调用进程分配一个新的控制台）
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        /// <summary>
        /// 附加控制台（为调用进程附加指定进程的控制台）
        /// </summary>
        /// <param name="dwProcessId">要使用的其控制台的进程的标识符。此参数可以是下列值之一：1、pid 使用指定进程的控制台 2、ATTACH_PARENT_PROCESS 使用当前进程的父窗口的控制台。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        /// <summary>
        /// 释放控制台（分离与调用进程相关联的控制台）
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        #endregion

        #region Fields
        /// <summary>
        /// 使用当前进程的父窗口的控制台
        /// </summary>
        private const int AttachParentProcess = -1;
        /// <summary>
        /// 标题
        /// </summary>
        private static string _tittle = "Author：Sean";
        /// <summary>
        /// 字体背景颜色
        /// </summary>
        private static ConsoleColor _fontBackgroundColor = ConsoleColor.Black;
        /// <summary>
        /// 字体颜色
        /// </summary>
        private static ConsoleColor _fontColor = ConsoleColor.Green;
        #endregion

        #region Properties
        /// <summary>
        /// 标题
        /// </summary>
        public static string Tittle
        {
            get { return _tittle; }
            set { _tittle = value; Console.Title = _tittle; }
        }
        /// <summary>
        /// 字体背景颜色
        /// </summary>
        public static ConsoleColor FontBackgroundColor
        {
            get { return _fontBackgroundColor; }
            set { _fontBackgroundColor = value; Console.BackgroundColor = _fontBackgroundColor; }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        public static ConsoleColor FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; Console.ForegroundColor = _fontColor; }
        }
        #endregion

        #region Methods
        private CmdHelper() { }

        /// <summary>
        /// 为调用进程分配一个新的控制台（一个进程最多只能分配一个控制台窗口）
        /// </summary>
        public static bool Create()
        {
            bool bRet = false;
            if (AllocConsole())
            {
                Init();
                bRet = true;
            }
            return bRet;
        }
        /// <summary>
        /// 为调用进程附加指定进程的控制台（一个进程最多只能分配一个控制台窗口）
        /// </summary>
        /// <param name="processId">指定父进程ID</param>
        public static bool Create(int processId)
        {
            bool bRet = false;
            if (AttachConsole(processId))
            {
                Init();
                bRet = true;
            }
            return bRet;
        }
        /// <summary>
        /// 为调用进程附加父进程的控制台（一个进程最多只能分配一个控制台窗口）。
        /// 注：当通过cmd启动程序时才会分配（通常在调试时使用）。
        /// </summary>
        public static bool CreateToParent()
        {
            return Create(AttachParentProcess);
        }

        /// <summary>
        /// 释放控制台（分离与调用进程相关联的控制台）
        /// </summary>
        public static bool Free()
        {
            return FreeConsole();
        }

        /// <summary>
        /// 将指定的字符串值写入标准输出流
        /// </summary>
        /// <param name="content"></param>
        public static void Write(string content)
        {
            Console.Write(content);
        }
        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）写入标准输出流
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        /// <summary>
        /// 将控制台的前景色和背景色设置为默认值。
        /// </summary>
        public static void ResetColor()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            Console.Title = _tittle;
            Console.BackgroundColor = _fontBackgroundColor;
            Console.ForegroundColor = _fontColor;
        }
        #endregion

        #region 功能扩展
        /// <summary>
        /// 调用cmd执行指定命令
        /// </summary>
        /// <param name="cmd">cmd命令字符串</param>
        /// <param name="output">输出信息</param>
        /// <param name="error">错误信息</param>
        /// <param name="show">是否显示窗口</param>
        /// <param name="workingDirectory">设置进程的初始目录</param>
        public static void RunCmd(string cmd, out string output, out string error, bool show = false, string workingDirectory = ".")
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,  //重定向标准输入
                    RedirectStandardOutput = true, //重定向标准输出
                    RedirectStandardError = true,  //重定向标准错误输出
                    UseShellExecute = false,       //是否使用操作系统外壳程序启动进程。[DefaultValue(true)]
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = !show
                }
            };
            process.Start();
            process.StandardInput.AutoFlush = true;
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||，前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令
            process.StandardInput.WriteLine(cmd + "&exit");

            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();

            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 调用cmd执行指定命令（异步）
        /// </summary>
        /// <param name="cmd">cmd命令字符串</param>
        /// <param name="output">输出信息</param>
        /// <param name="error">错误信息</param>
        /// <param name="show">是否显示窗口</param>
        /// <param name="workingDirectory">设置进程的初始目录</param>
        public static void RunCmdAsyn(string cmd, out string output, out string error, bool show = false, string workingDirectory = ".")
        {
            try
            {
                Process process = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,// 必须禁用操作系统外壳程序
                        WorkingDirectory = workingDirectory,
                        CreateNoWindow = !show
                    }
                };
                process.Start();
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine(cmd + "&exit");

                StringBuilder outputData = new StringBuilder();
                StringBuilder errorData = new StringBuilder();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // 异步获取命令行内容
                process.OutputDataReceived += (sender, e) =>
                {
                    var data = e.Data;
                    if (data != null)
                        outputData.Append(data + "\n");
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    var data = e.Data;
                    if (data != null)
                        errorData.Append(data + "\n");
                };

                process.WaitForExit();
                process.Close();

                output = outputData.ToString();
                error = errorData.ToString();
            }
            catch (Exception)
            {
                output = null;
                error = null;
            }
        }

        /// <summary>
        /// 执行指定的应用程序
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="arguments">命令行参数</param>
        /// <param name="output">输出信息</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static void RunExe(string path, string arguments, out string output, out string error)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;// 必须禁用操作系统外壳程序

                process.Start();

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                process.Close();
            }
        }
        /// <summary>
        /// 调用cmd执行指定的应用程序
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="arguments">命令行参数</param>
        /// <param name="output">输出信息</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static void RunExeByCmd(string path, string arguments, out string output, out string error)
        {
            if (path == null) throw new ArgumentNullException("path");

            using (Process cmd = new Process())
            {
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.RedirectStandardError = true;
                cmd.StartInfo.UseShellExecute = false;// 必须禁用操作系统外壳程序

                cmd.Start();
                cmd.StandardInput.AutoFlush = true;
                //1、切换盘符
                cmd.StandardInput.WriteLine(Path.GetPathRoot(path).TrimEnd('\\'));
                //2、切换目录。如果路径中可能含有空格时，可以用双引号括起来，防止命令执行失败。
                cmd.StandardInput.WriteLine("cd \"{0}\"", Path.GetDirectoryName(path));
                //2、运行程序
                cmd.StandardInput.WriteLine("{0} {1} {2}", Path.GetFileName(path), arguments, "&exit");

                output = cmd.StandardOutput.ReadToEnd();
                error = cmd.StandardError.ReadToEnd();

                cmd.WaitForExit();
                cmd.Close();
            }
        }

        /// <summary>
        /// Ping指定的地址
        /// </summary>
        /// <param name="target">IP地址 或 主机名 或 域名</param>
        /// <param name="timeout">等待答复的最大毫秒数</param>
        /// <param name="tryCnt">尝试次数</param>
        /// <returns>true：通；false：不通</returns>
        public static bool Ping(string target, int timeout, uint tryCnt = 1)
        {
            if (tryCnt < 1) return false;

            bool flag = new Ping().Send(target, timeout)?.Status == IPStatus.Success;
            if (!flag && --tryCnt > 0)
                flag = Ping(target, timeout, tryCnt);
            return flag;
        }

        #endregion
    }
}
