using System.Diagnostics;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Log
{
    /// <summary>
    /// Log configuration.
    /// </summary>
    /// <remarks>
    /// 日志配置
    /// </remarks>
    public class SimpleLocalLoggerOptions
    {
        public SimpleLocalLoggerOptions()
        {
            LogToDebug = false;
            LogToTrace = false;
            LogToConsole = true;
            LogToLocalFile = true;
            MinLogLevelForLocalFile = 0;
            MinLogLevelForConsole = 0;
            //MaxBackupFileCount = 10;
            MaxFileSize = 10 * 1024 * 1024;
        }

        /// <summary>
        /// Whether to output the log to the <see cref="Debug"/>, default value: false.
        /// </summary>
        public bool LogToDebug { get; set; }
        /// <summary>
        /// Whether to output the log to the <see cref="Trace"/>, default value: false.
        /// </summary>
        public bool LogToTrace { get; set; }
        /// <summary>
        /// Whether to output the log to the console, default value: true.
        /// </summary>
        /// <remarks>
        /// 是否将日志输出到控制台，默认值为true。
        /// </remarks>
        public bool LogToConsole { get; set; }
        /// <summary>
        /// Whether to output the log to a local file, default value: true.
        /// </summary>
        /// <remarks>
        /// 是否将日志输出到本地文件，默认值：true。
        /// </remarks>
        public bool LogToLocalFile { get; set; }

        public bool AnyLogTarget => LogToDebug || LogToTrace || LogToConsole || LogToLocalFile;

        public LogLevel MinLogLevelForDebug { get; set; }
        public LogLevel MinLogLevelForTrace { get; set; }
        /// <summary>
        /// Minimum log level (console), default value: 0.
        /// </summary>
        /// <remarks>
        /// 最低日志级别（控制台），默认值：0。
        /// </remarks>
        public LogLevel MinLogLevelForConsole { get; set; }
        /// <summary>
        /// The minimum log level (local file), default value: 0.
        /// </summary>
        /// <remarks>
        /// 最小日志级别（本地文件），默认值：0。
        /// </remarks>
        public LogLevel MinLogLevelForLocalFile { get; set; }

        /// <summary>
        /// The maximum number of file backups, less than or equal to 0 means unlimited, default: 10.
        /// </summary>
        /// <remarks>
        /// 文件备份的最大数量，小于或等于0表示无限制，默认值：10。
        /// </remarks>
        //public int MaxBackupFileCount { get; set; }
        /// <summary>
        /// Maximum size of a single log file (unit: Byte), less than or equal to 0 means unlimited, default: 10M.
        /// </summary>
        /// <remarks>
        /// 单个日志文件的最大大小（单位：字节），小于或等于0表示无限制，默认值：10M。
        /// </remarks>
        public long MaxFileSize { get; set; }
    }
}
