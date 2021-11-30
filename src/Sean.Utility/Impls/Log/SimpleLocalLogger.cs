using System;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Log
{
    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public class SimpleLocalLogger : SimpleLocalLoggerBase, ISimpleLogger
    {
        public SimpleLocalLogger() : base()
        {
        }

        public SimpleLocalLogger(Action<SimpleLocalLoggerOptions> options) : base(options)
        {
        }

        public SimpleLocalLogger(Type type) : base(type)
        {
        }

        public void LogDebug(string msg, Exception ex = null)
        {
            Log(LogLevel.Debug, msg, ex);
        }
        public void LogInfo(string msg, Exception ex = null)
        {
            Log(LogLevel.Info, msg, ex);
        }
        public void LogWarn(string msg, Exception ex = null)
        {
            Log(LogLevel.Warn, msg, ex);
        }
        public void LogError(string msg, Exception ex = null)
        {
            Log(LogLevel.Error, msg, ex);
        }
        public void LogFatal(string msg, Exception ex = null)
        {
            Log(LogLevel.Fatal, msg, ex);
        }
    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public class SimpleLocalLogger<T> : SimpleLocalLogger, ISimpleLogger<T> where T : class
    {
        public SimpleLocalLogger() : base()
        {
            ClassType = typeof(T);
        }

        public SimpleLocalLogger(Action<SimpleLocalLoggerOptions> options) : base(options)
        {
            ClassType = typeof(T);
        }
    }
}
