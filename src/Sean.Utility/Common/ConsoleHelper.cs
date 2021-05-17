using System;

namespace Sean.Utility.Common
{
    public class ConsoleHelper
    {
        public static void Write(object msg, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            CustomOutputMsg(msg, Console.Write, foregroundColor, backgroundColor);
        }

        public static void WriteLine(object msg = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (msg == null)
            {
                Console.WriteLine();
                return;
            }

            CustomOutputMsg(msg, Console.WriteLine, foregroundColor, backgroundColor);
        }

        private static void CustomOutputMsg(object msg, Action<object> action, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (foregroundColor == null && backgroundColor == null)
            {
                action?.Invoke(msg);
                return;
            }

            var originForegroundColor = Console.ForegroundColor;
            var originBackgroundColor = Console.BackgroundColor;

            if (foregroundColor != null)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }
            if (backgroundColor != null)
            {
                Console.ForegroundColor = backgroundColor.Value;
            }

            action?.Invoke(msg);

            if (foregroundColor != null)
            {
                Console.ForegroundColor = originForegroundColor;
            }
            if (backgroundColor != null)
            {
                Console.ForegroundColor = originBackgroundColor;
            }
        }
    }
}
