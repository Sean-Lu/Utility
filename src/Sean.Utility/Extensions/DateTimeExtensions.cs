using System;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="DateTime"/>
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert the date into a string in the format "yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <remarks>
        /// 将日期转成 "yyyy-MM-dd HH:mm:ss" 格式的字符串
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToLongDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Convert the date into a string in the format "yyyy-MM-ddTHH:mm:sszzz" (with time zone)
        /// </summary>
        /// <remarks>
        /// 将日期转成 "yyyy-MM-ddTHH:mm:sszzz" 格式的字符串（带时区）
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToLongDateTimeWithTimezone(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        /// <summary>
        /// Convert the date into a string in "yyyy-MM-dd" format
        /// </summary>
        /// <remarks>
        /// 将日期转成 "yyyy-MM-dd" 格式的字符串
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Convert the date into a string in "HH:mm:ss" format
        /// </summary>
        /// <remarks>
        /// 将日期转成 "HH:mm:ss" 格式的字符串
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToHourMinuteSecond(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Convert the date into a string in "HH:mm" format
        /// </summary>
        /// <remarks>
        /// 将日期转成 "HH:mm" 格式的字符串
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToHourMinute(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        /// <summary>
        /// Return the time zone, example (China time zone): +08:00
        /// </summary>
        /// <remarks>
        /// 返回时区，示例（中国时区）：+08:00
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string Timezone(this DateTime dateTime)
        {
            return dateTime.ToString("zzz");
        }

        /// <summary>
        /// Set time type
        /// </summary>
        /// <remarks>
        /// 设置时间类型
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <param name="kind">时间类型，默认为本地时间</param>
        /// <returns></returns>
        public static DateTime SetDateTimeKind(this DateTime dateTime, DateTimeKind kind = DateTimeKind.Local)
        {
            return new DateTime(dateTime.Ticks, kind);
        }
        /// <summary>
        /// Set time type (if not specified)
        /// </summary>
        /// <remarks>
        /// 设置时间类型（如果未指定）
        /// </remarks>
        /// <param name="dateTime"></param>
        /// <param name="kind">时间类型，默认为本地时间</param>
        /// <returns></returns>
        public static DateTime SetDateTimeKindIfUnspecified(this DateTime dateTime, DateTimeKind kind = DateTimeKind.Local)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return new DateTime(dateTime.Ticks, kind);
            }

            return dateTime;
        }
    }
}
