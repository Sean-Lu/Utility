using Sean.Utility.Enums;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalLoggerOptions
    {
        /// <summary>
        /// Whether to output the log to a local file, default value: true.
        /// </summary>
        /// <remarks>
        /// 是否将日志输出到本地文件，默认值：true。
        /// </remarks>
        bool LogToLocalFile { get; }
        /// <summary>
        /// Whether to output the log to the console, default value: true.
        /// </summary>
        /// <remarks>
        /// 是否将日志输出到控制台，默认值为true。
        /// </remarks>
        bool LogToConsole { get; }

        /// <summary>
        /// The minimum log level (local file), default value: 0.
        /// </summary>
        /// <remarks>
        /// 最小日志级别（本地文件），默认值：0。
        /// </remarks>
        LogLevel MinLogLevelForLocalFile { get; }
        /// <summary>
        /// Minimum log level (console), default value: 0.
        /// </summary>
        /// <remarks>
        /// 最低日志级别（控制台），默认值：0。
        /// </remarks>
        LogLevel MinLogLevelForConsole { get; }

        //int MaxBackupFileCount { get; }
        /// <summary>
        /// Maximum size of a single log file (unit: Byte), less than or equal to 0 means unlimited, default: 10M.
        /// </summary>
        /// <remarks>
        /// 单个日志文件的最大大小（单位：字节），小于或等于0表示无限制，默认值：10M。
        /// </remarks>
        long MaxFileSize { get; }
    }
}