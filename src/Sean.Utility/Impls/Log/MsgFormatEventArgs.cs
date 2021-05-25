using Sean.Utility.Enums;
using System;
using Sean.Utility.Contracts;

namespace Sean.Utility.Impls.Log
{
    public class MsgFormatEventArgs : EventArgs
    {
        public MsgFormatEventArgs(ISimpleLocalLoggerOptions options, Type classType, LogLevel logLevel, string message, Exception exception)
        {
            Options = options;
            ClassType = classType;
            LogLevel = logLevel;
            Message = message;
            Exception = exception;

            MsgFormat = message;
        }

        public ISimpleLocalLoggerOptions Options { get; }
        public Type ClassType { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public Exception Exception { get; }

        /// <summary>
        /// 格式化后的消息
        /// </summary>
        public string MsgFormat { get; set; }
        /// <summary>
        /// 是否已经将信息输出到<see cref="Console"/>控制台窗口
        /// </summary>
        public bool HasOutputConsole { get; set; }
    }
}
