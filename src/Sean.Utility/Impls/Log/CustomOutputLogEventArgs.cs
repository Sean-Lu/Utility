using Sean.Utility.Enums;
using System;
using Sean.Utility.Contracts;

namespace Sean.Utility.Impls.Log
{
    public class CustomOutputLogEventArgs : EventArgs
    {
        public CustomOutputLogEventArgs(ISimpleLocalLoggerOptions options, LogLevel logLevel, string message, Exception exception)
        {
            Options = options;
            LogLevel = logLevel;
            Message = message;
            Exception = exception;
        }

        public ISimpleLocalLoggerOptions Options { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public Exception Exception { get; }
    }
}
