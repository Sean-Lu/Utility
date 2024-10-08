﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sean.Utility;

public class ConsoleHelper
{
    public const int ATTACH_PARENT_PROCESS = -1;

    /// <summary>
    /// Allocates a new console for the calling process.
    /// </summary>
    /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
    /// <remarks>
    /// A process can be associated with only one console,
    /// so the function fails if the calling process already has a console.
    /// </remarks>
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dwProcessId">指定pid，或使用<see cref="ATTACH_PARENT_PROCESS"/>附加到父进程</param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern bool AttachConsole(int dwProcessId);
    /// <summary>
    /// Detaches the calling process from its console.
    /// </summary>
    /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
    /// <remarks>
    /// If the calling process is not already attached to a console,
    /// the error code returned is ERROR_INVALID_PARAMETER (87).
    /// </remarks>
    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();

    /// <summary>
    /// 调用 cmd.exe 执行指定命令
    /// </summary>
    /// <param name="command"></param>
    /// <param name="outputDataReceived"></param>
    /// <param name="errorDataReceived"></param>
    /// <param name="workingDirectory">设置进程的工作目录</param>
    /// <param name="createNoWindow">true:不显示窗口，false:不显示窗口</param>
    public static void RunCommand(string[] command, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, string workingDirectory = ".", bool createNoWindow = true)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;  //重定向标准输入
            process.StartInfo.RedirectStandardOutput = true; //重定向标准输出
            process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
            process.StartInfo.UseShellExecute = false;       //是否使用操作系统外壳程序启动进程
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.CreateNoWindow = createNoWindow;

            process.Start();

            process.StandardInput.AutoFlush = true;
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||，前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令
            process.StandardInput.WriteLine($"({string.Join("&&", command)})&exit");

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
}