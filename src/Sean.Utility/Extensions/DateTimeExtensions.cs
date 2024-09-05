using System;

namespace Sean.Utility.Extensions;

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
        switch (dateTime.Kind)
        {
            case DateTimeKind.Local:
                return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            case DateTimeKind.Utc:
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            default:
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");// DateTimeKind.Unspecified
        }
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
        if (dateTime.Kind != DateTimeKind.Unspecified)
        {
            return dateTime;
        }

        return DateTime.SpecifyKind(dateTime, kind);
    }
}