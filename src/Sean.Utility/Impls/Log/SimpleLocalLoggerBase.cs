﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Extensions;
using Sean.Utility.IO;
#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#endif

namespace Sean.Utility.Impls.Log
{
    public abstract class SimpleLocalLoggerBase : ISimpleLoggerBase
    {
        #region Event
        /// <summary>
        /// Log file path, automatically generated by time by default: \Log\yyyy-MM-dd.log.
        /// </summary>
        /// <remarks>
        /// 日志文件路径，默认情况下会自动按时间生成：\Log\yyyy-MM-dd.log。
        /// </remarks>
        public static Func<string> LogFilePath { get; set; } = () => Path.Combine(GetLogBaseDir(), $"{DateTime.Now:yyyy-MM-dd}.log");

        /// <summary>
        /// 时间格式化
        /// </summary>
        public static Func<DateTime, string> DateTimeFormat { get; set; }

        /// <summary>
        /// Custom processing message format, default: [yyyy-MM-ddTHH:mm:sszzz] [logLevel] [classType.FullName] => msg
        /// </summary>
        /// <remarks>
        /// 自定义处理消息格式，默认为：[yyyy-MM-ddTHH:mm:sszzz] [logLevel] [classType.FullName] => msg
        /// </remarks>
        public static Func<Type, string, Exception, LogLevel, string> MsgFormat { get; set; } = (classType, msg, exception, logLevel) =>
        {
            var result = $"[{(DateTimeFormat != null ? DateTimeFormat(DateTime.Now) : DateTime.Now.ToLongDateTimeWithTimezone())}] [{logLevel}]{(classType != null ? $" [{classType.FullName}]" : string.Empty)} => {msg}";
            return exception != null ? $"{result}{Environment.NewLine}Message: {exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}" : result;
        };

        /// <summary>
        /// To handle internal exceptions, the default output log is: Console, \Log\internal.log.
        /// </summary>
        /// <remarks>
        /// 处理内部异常，默认输出日志到：Console、\Log\internal.log。
        /// </remarks>
        public static Action<Exception> OnException { get; set; } = exception =>
        {
            if (exception != null)
            {
                try
                {
                    var msg = $"[{DateTime.Now.ToLongDateTimeWithTimezone()}] [{LogLevel.Fatal}] => Internal error!{Environment.NewLine}Message: {exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}";
                    Console.WriteLine(msg);
                    using (var sw = File.AppendText(Path.Combine(GetLogBaseDir(), "internal.log")))
                    {
                        sw.WriteLine(msg);
                        sw.Close();
                    }
                }
                catch
                {
                    // ignored
                }
            }
        };

        /// <summary>
        /// Custom output log.
        /// </summary>
        /// <remarks>
        /// 自定义输出日志。
        /// </remarks>
        public static Action<LogLevel, string, Exception> CustomLog { get; set; }

        /// <summary>
        /// Encoding, The default value is UTF8.
        /// </summary>
        /// <remarks>
        /// Encoding，默认值为UTF8。
        /// </remarks>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        private static string GetLogBaseDir()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
        #endregion

        public SimpleLocalLoggerOptions Options => _options;

        protected Type ClassType;

        private readonly SimpleLocalLoggerOptions _options;

#if NETSTANDARD
        internal static IServiceProvider ServiceProvider { get; set; }
#endif

        private static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        protected SimpleLocalLoggerBase() : this(null)
        {

        }

        protected SimpleLocalLoggerBase(Action<SimpleLocalLoggerOptions> options)
        {
#if NETSTANDARD
            _options = ServiceProvider?.GetService<IOptionsMonitor<SimpleLocalLoggerOptions>>().CurrentValue ?? new SimpleLocalLoggerOptions();
#else
            _options = new SimpleLocalLoggerOptions();
#endif

            options?.Invoke(_options);
        }

        public virtual void Log(LogLevel logLevel, string msg, Exception ex = null)
        {
            try
            {
                // 输出日志 => 自定义（默认无）
                CustomLog?.Invoke(logLevel, msg, ex);

                if (!_options.LogToConsole && !_options.LogToLocalFile)
                {
                    return;
                }

                var msgFormat = MsgFormat != null ? MsgFormat(ClassType, msg, ex, logLevel) : msg;
                if (!string.IsNullOrWhiteSpace(msgFormat))
                {
                    if (_options.LogToConsole && logLevel >= _options.MinLogLevelForConsole)
                    {
                        // 输出日志 => 控制台
                        Console.WriteLine(msgFormat);
                    }

                    if (_options.LogToLocalFile && logLevel >= _options.MinLogLevelForLocalFile)
                    {
                        // 输出日志 => 本地文件
                        try
                        {
                            _locker.EnterWriteLock();

                            var filePath = LogFilePath?.Invoke();
                            if (string.IsNullOrWhiteSpace(filePath))
                            {
                                throw new Exception("Log file path cannot be empty!");
                            }

                            #region 日志文件备份
                            if (_options.MaxFileSize > 0 && File.Exists(filePath))
                            {
                                //var msgByteCount = Options.DefaultEncoding.GetByteCount(msgFormat);
                                var fileInfo = new FileInfo(filePath);
                                if (fileInfo.Length >= _options.MaxFileSize)
                                {
                                    var backupFilePath = PathHelper.GetBackupFilePath(filePath, out _);
                                    File.Move(filePath, backupFilePath);
                                }
                            }
                            #endregion

                            //using (var sw = File.AppendText(filePath))
                            using (var sw = new StreamWriter(filePath, true, DefaultEncoding))
                            {
                                sw.WriteLine(msgFormat);
                                sw.Close();
                            }
                        }
                        finally
                        {
                            if (_locker.IsWriteLockHeld)
                            {
                                _locker.ExitWriteLock();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                OnException?.Invoke(exception);
            }
        }

        public virtual Task LogAsync(LogLevel logLevel, string msg, Exception ex = null)
        {
            return Task.Factory.StartNewCatchException(() =>
            {
                Log(logLevel, msg, ex);
            });
        }
    }
}
