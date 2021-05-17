using System;
using Sean.Utility.Enums;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="LogLevel"/>
    /// </summary>
    public static class LogLevelExtensions
    {
        public static ConsoleColor GetConsoleColor(this LogLevel logLevel)
        {
            if (logLevel >= LogLevel.Error)
            {
                return ConsoleColor.Red;
            }

            if (logLevel == LogLevel.Warn)
            {
                return ConsoleColor.Yellow;
            }

            return ConsoleColor.Green;
        }
    }
}
