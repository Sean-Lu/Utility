using System;
using System.Threading.Tasks;
using Sean.Utility.Enums;
using Sean.Utility.Impls.Log;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLoggerBase
    {
        ISimpleLocalLoggerOptions Options { get; }

        void Log(LogLevel logLevel, string msg, Exception ex = null);

        Task LogAsync(LogLevel logLevel, string msg, Exception ex = null);
    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public interface ISimpleLogger : ISimpleLoggerBase, ILogger
    {

    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public interface ISimpleLogger<T> : ISimpleLogger, ILogger<T> where T : class
    {

    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public interface ISimpleLoggerAsync : ISimpleLoggerBase, ILoggerAsync
    {

    }

    /// <summary>
    /// Simple implementation of local log. Note: Support console, local log file, custom output.
    /// </summary>
    public interface ISimpleLoggerAsync<T> : ISimpleLoggerAsync, ILoggerAsync<T> where T : class
    {

    }
}
