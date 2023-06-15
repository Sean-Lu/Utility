using Sean.Utility.Enums;
using System;

namespace Sean.Utility.Impls.Log
{
    public class MsgFormatEventArgs : EventArgs
    {
        public MsgFormatEventArgs(SimpleLocalLoggerOptions options, Type classType, LogLevel logLevel, string message, Exception exception)
        {
            Options = options;
            ClassType = classType;
            LogLevel = logLevel;
            Message = message;
            Exception = exception;

            MsgFormat = message;
        }

        public SimpleLocalLoggerOptions Options { get; }
        public Type ClassType { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public Exception Exception { get; }

        /// <summary>
        /// 格式化后的消息
        /// </summary>
        public string MsgFormat { get; set; }
    }
}
