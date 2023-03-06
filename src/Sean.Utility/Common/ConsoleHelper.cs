using System;
using System.Collections.Generic;

namespace Sean.Utility
{
    /// <summary>
    /// <see cref="Console"/>
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void Write(string data, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Output(data, Console.Write, foregroundColor, backgroundColor);
        }
        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void Write(IEnumerable<string> data, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Output(data, Console.Write, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void WriteLine(string data, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Output(data, Console.WriteLine, foregroundColor, backgroundColor);
        }
        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        public static void WriteLine(IEnumerable<string> data, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Output(data, Console.WriteLine, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="action"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        private static void Output(string data, Action<string> action, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            var setForegroundColor = foregroundColor.HasValue;
            var setBackgroundColor = backgroundColor.HasValue;

            if (setForegroundColor)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }
            if (setBackgroundColor)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }

            action?.Invoke(data);

            if (setForegroundColor || setBackgroundColor)
            {
                Console.ResetColor();
            }
        }
        /// <summary>
        /// 输出内容显示自定义颜色
        /// </summary>
        /// <param name="data"></param>
        /// <param name="action"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundColor"></param>
        private static void Output(IEnumerable<string> data, Action<string> action, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            var setForegroundColor = foregroundColor.HasValue;
            var setBackgroundColor = backgroundColor.HasValue;

            if (setForegroundColor)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }
            if (setBackgroundColor)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }

            foreach (var str in data)
            {
                action?.Invoke(str);
            }

            if (setForegroundColor || setBackgroundColor)
            {
                Console.ResetColor();
            }
        }
    }
}
