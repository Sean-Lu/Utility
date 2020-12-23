using System;
using System.Threading.Tasks;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Log
{
    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public class SimpleLocalLoggerAsync : SimpleLocalLoggerBase, ISimpleLoggerAsync
    {
        public SimpleLocalLoggerAsync() : base()
        {

        }

        public SimpleLocalLoggerAsync(Action<SimpleLocalLoggerOptions> options) : base(options)
        {

        }

        public Task LogDebugAsync(string msg, Exception ex = null)
        {
            return LogAsync(LogLevel.Debug, msg, ex);
        }
        public Task LogInfoAsync(string msg, Exception ex = null)
        {
            return LogAsync(LogLevel.Info, msg, ex);
        }
        public Task LogWarnAsync(string msg, Exception ex = null)
        {
            return LogAsync(LogLevel.Warn, msg, ex);
        }
        public Task LogErrorAsync(string msg, Exception ex = null)
        {
            return LogAsync(LogLevel.Error, msg, ex);
        }
        public Task LogFatalAsync(string msg, Exception ex = null)
        {
            return LogAsync(LogLevel.Fatal, msg, ex);
        }
    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public class SimpleLocalLoggerAsync<T> : SimpleLocalLoggerAsync, ISimpleLoggerAsync<T> where T : class
    {
        public SimpleLocalLoggerAsync() : base()
        {
            ClassType = typeof(T);
        }

        public SimpleLocalLoggerAsync(Action<SimpleLocalLoggerOptions> options) : base(options)
        {
            ClassType = typeof(T);
        }
    }
}
