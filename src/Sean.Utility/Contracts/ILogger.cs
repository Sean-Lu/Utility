using System;
using System.Threading.Tasks;

namespace Sean.Utility.Contracts
{
    /// <summary>
    /// Log general interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        void LogDebug(string msg, Exception ex = null);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        void LogInfo(string msg, Exception ex = null);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        void LogWarn(string msg, Exception ex = null);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        void LogError(string msg, Exception ex = null);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        void LogFatal(string msg, Exception ex = null);
    }

    /// <summary>
    /// Log general interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogger<T> : ILogger
    {

    }

    /// <summary>
    /// Log general interface
    /// </summary>
    public interface ILoggerAsync
    {
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        Task LogDebugAsync(string msg, Exception ex = null);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        Task LogInfoAsync(string msg, Exception ex = null);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        Task LogWarnAsync(string msg, Exception ex = null);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        Task LogErrorAsync(string msg, Exception ex = null);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="msg">Content</param>
        /// <param name="ex">Exception</param>
        Task LogFatalAsync(string msg, Exception ex = null);
    }

    /// <summary>
    /// Log general interface
    /// </summary>
    public interface ILoggerAsync<T> : ILoggerAsync
    {

    }
}
