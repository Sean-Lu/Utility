using System;

namespace Demo.Framework.Extensions
{
    /// <summary>
    /// Extensions for <see cref="double"/>
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// 保留2位小数文本
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToHundredthString(this double source)
        {
            return Math.Round(source, 2).ToString("0.00");
        }
    }
}
