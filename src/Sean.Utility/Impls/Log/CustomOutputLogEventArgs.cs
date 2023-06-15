using Sean.Utility.Enums;
using System;

namespace Sean.Utility.Impls.Log
{
    public class CustomOutputLogEventArgs : EventArgs
    {
        public CustomOutputLogEventArgs(SimpleLocalLoggerOptions options, LogLevel logLevel, string message, Exception exception)
        {
            Options = options;
            LogLevel = logLevel;
            Message = message;
            Exception = exception;
        }

        public SimpleLocalLoggerOptions Options { get; }
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public Exception Exception { get; }
    }
}
