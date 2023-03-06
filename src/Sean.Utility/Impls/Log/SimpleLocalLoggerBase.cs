using System;
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
        /// <summary>
        /// Custom output log.
        /// </summary>
        /// <remarks>
        /// 自定义输出日志。
        /// </remarks>
        public static event EventHandler<CustomOutputLogEventArgs> CustomOutputLog;
        /// <summary>
        /// Handle internal exception, default output log to: \Log\internal.log.
        /// </summary>
        /// <remarks>
        /// 处理内部异常，默认输出日志到：\Log\internal.log。
        /// </remarks>
        public static event EventHandler<EventArgs<Exception>> OnException;

        /// <summary>
        /// 是否通过 <see cref="LogLevel"/> 分隔日志文件，如：
        /// <para>\Log\Debug\yyyy-MM-dd.log</para>
        /// <para>\Log\Info\yyyy-MM-dd.log</para>
        /// <para>\Log\Error\yyyy-MM-dd.log</para>
        /// <para>...</para>
        /// </summary>
        public static bool LogFileSeparatedByLogLevel { get; set; }
        /// <summary>
        /// 日志文件目录，默认：\Log
        /// </summary>
        public static Func<LogLevel, string> LogFileDirectory { get; set; } = (level) =>
         {
             var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log{(LogFileSeparatedByLogLevel ? $@"\{level.ToString()}" : string.Empty)}");
             if (!Directory.Exists(dir))
             {
                 Directory.CreateDirectory(dir);
             }

             return dir;
         };
        /// <summary>
        /// Log file name, default: yyyy-MM-dd.log.
        /// </summary>
        /// <remarks>
        /// 日志文件名称，默认：yyyy-MM-dd.log。
        /// </remarks>
        public static Func<LogLevel, string> LogFilePath { get; set; } = (level) => $"{DateTime.Now:yyyy-MM-dd}.log";
        /// <summary>
        /// 时间格式化
        /// </summary>
        public static Func<DateTime, string> DateTimeFormat { get; set; }
        /// <summary>
        /// Custom message format, default: [&lt;time&gt;] [&lt;logLevel:Upper&gt;] [&lt;classType:FullName&gt;] =&gt; &lt;msg&gt;&lt;newline&gt;&lt;exception&gt;
        /// </summary>
        /// <remarks>
        /// 自定义消息格式，默认：[&lt;time&gt;] [&lt;logLevel:Upper&gt;] [&lt;classType:FullName&gt;] =&gt; &lt;msg&gt;&lt;newline&gt;&lt;exception&gt;
        /// </remarks>
        public static Action<MsgFormatEventArgs> MsgFormat { get; set; }
        /// <summary>
        /// Encoding, The default value is UTF8.
        /// </summary>
        /// <remarks>
        /// Encoding，默认值为UTF8。
        /// </remarks>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 默认日志配置
        /// </summary>
        public static SimpleLocalLoggerOptions DefaultLoggerOptions { get; set; }

        public ISimpleLocalLoggerOptions Options => _options;

        protected Type ClassType;

        private readonly SimpleLocalLoggerOptions _options;

#if NETSTANDARD
        internal static IServiceProvider ServiceProvider { get; set; }
#endif

        private static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        protected SimpleLocalLoggerBase() : this(options: null)
        {
        }

        protected SimpleLocalLoggerBase(Action<SimpleLocalLoggerOptions> options)
        {
            if (DefaultLoggerOptions != null)
            {
                _options = DefaultLoggerOptions;
            }
            else
            {
#if NETSTANDARD
                _options = ServiceProvider?.GetService<IOptionsMonitor<SimpleLocalLoggerOptions>>()?.CurrentValue ?? new SimpleLocalLoggerOptions();
#else
                _options = new SimpleLocalLoggerOptions();
#endif
            }

            options?.Invoke(_options);
        }

        protected SimpleLocalLoggerBase(Type type) : this(options: null)
        {
            ClassType = type;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public virtual void Log(LogLevel logLevel, string msg, Exception ex = null)
        {
            try
            {
                // 输出日志 => 自定义（默认无）
                CustomOutputLog?.Invoke(this, new CustomOutputLogEventArgs(_options, logLevel, msg, ex));

                if (!_options.LogToConsole && !_options.LogToLocalFile)
                {
                    return;
                }

                var msgFormat = GetFormattedMessage(ClassType, logLevel, msg, ex);
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

                            var filePath = Path.Combine(LogFileDirectory(logLevel), LogFilePath(logLevel));
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
                                    var backupFilePath = NewBackupFilePath(filePath);
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
                if (OnException != null)
                {
                    OnException(this, new EventArgs<Exception>(exception));
                }
                else
                {
                    DefaultHandleInternalException(exception);
                }
            }
        }

        /// <summary>
        /// 记录日志（异步）
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public virtual Task LogAsync(LogLevel logLevel, string msg, Exception ex = null)
        {
            return Task.Factory.StartNewCatchException(() =>
            {
                Log(logLevel, msg, ex);
            });
        }

        /// <summary>
        /// 日志消息格式化处理
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual string GetFormattedMessage(Type classType, LogLevel logLevel, string msg, Exception ex = null)
        {
            if (MsgFormat != null)
            {
                var args = new MsgFormatEventArgs(_options, classType, logLevel, msg, ex);
                MsgFormat(args);
                return args.MsgFormat;
            }

            return DefaultHandleMsgFormat(ClassType, logLevel, msg, ex);
        }

        /// <summary>
        /// 获取新的备份文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected virtual string NewBackupFilePath(string filePath)
        {
            var index = 2;
            while (true)
            {
                var backupFilePath = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}({index}){Path.GetExtension(filePath)}");
                if (!File.Exists(backupFilePath))
                {
                    return backupFilePath;
                }

                index++;
            }
        }

        protected virtual string DefaultHandleMsgFormat(Type classType, LogLevel logLevel, string msg, Exception exception)
        {
            return $"[{(DateTimeFormat != null ? DateTimeFormat(DateTime.Now) : DateTime.Now.ToLongDateTimeWithTimezone())}] [{logLevel.ToString().ToUpper()}]{(classType != null ? $" [{classType.FullName}]" : string.Empty)} => {msg}{(exception != null ? $"{Environment.NewLine}{exception.ToString()}" : string.Empty)}";
        }

        protected virtual void DefaultHandleInternalException(Exception exception)
        {
            if (exception == null) return;

            try
            {
                var msg = GetFormattedMessage(this.GetType(), LogLevel.Fatal, "Internal error.", exception);
                using (var sw = File.AppendText(Path.Combine(LogFileDirectory(LogLevel.Fatal), "internal.log")))
                {
                    sw.WriteLine(msg);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
