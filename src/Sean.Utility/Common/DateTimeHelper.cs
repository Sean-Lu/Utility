using System;
using Sean.Utility.Enums;

namespace Sean.Utility
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// <para>返回指定日期在该年是第几周。</para>
        /// <para>年的第一周从该年的第一天开始，到所指定周的下一个首日前结束。</para>
        /// <para>一个星期的第一天为星期一。</para>
        /// </summary>
        /// <param name="datetime">日期时间</param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime datetime)
        {
            return GetWeekOfYear(datetime, DayOfWeek.Monday);
        }

        /// <summary>
        /// 返回指定日期在该年是第几周。
        /// ***年的第一周从该年的第一天开始，到所指定周的下一个首日前结束。***
        /// </summary>
        /// <param name="datetime">日期时间</param>
        /// <param name="firstDayOfWeek">System.DayOfWeek 值之一，表示一个星期的第一天。</param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            return new System.Globalization.GregorianCalendar().GetWeekOfYear(datetime, System.Globalization.CalendarWeekRule.FirstDay, firstDayOfWeek);
        }

        /// <summary>  
        /// 返回指定日期所属周的第一天(以星期天为第一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetFirstDayOfWeekSun(DateTime datetime)
        {
            //星期天为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string firstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(firstDay);
        }

        /// <summary>  
        /// 返回指定日期所属周的第一天(以星期一为第一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetFirstDayOfWeekMon(DateTime datetime)
        {
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string firstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(firstDay);
        }

        /// <summary>
        /// 返回指定日期所在月份的第一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime datetime)
        {
            return Convert.ToDateTime(datetime.ToString("yyyy-MM-01"));
        }

        /// <summary>  
        /// 返回指定日期所属周的最后一天(以星期六为最后一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetLastDayOfWeekSat(DateTime datetime)
        {
            //星期六为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            int daydiff = (7 - weeknow) - 1;

            //本周最后一天  
            string lastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(lastDay);
        }

        /// <summary>  
        /// 返回指定日期所属周的最后一天(以星期天为最后一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetLastDayOfWeekSun(DateTime datetime)
        {
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            string lastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(lastDay);
        }

        /// <summary>
        /// 返回指定日期所在月份的最后一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime datetime)
        {
            return GetFirstDayOfMonth(datetime.AddMonths(1)).AddDays(-1);
        }

        #region DateTime与时间戳转换
        //private static readonly DateTime Jan1st1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// DateTime转换为时间戳
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="timestampType">时间戳类型</param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime time, TimestampType timestampType = TimestampType.TotalMilliseconds)
        {
            switch (timestampType)
            {
                case TimestampType.TotalMilliseconds:
                    //return (long)(time.ToUniversalTime() - Jan1st1970Utc).TotalMilliseconds;
                    return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
                case TimestampType.TotalSeconds:
                    //return (long)(time.ToUniversalTime() - Jan1st1970Utc).TotalSeconds;
                    return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000000;
                default:
                    throw new NotSupportedException($"Unsupported type: {timestampType}");
            }
        }
        /// <summary>
        /// 时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="timestampType">时间戳类型</param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp, TimestampType timestampType = TimestampType.TotalMilliseconds)
        {
            switch (timestampType)
            {
                case TimestampType.TotalMilliseconds:
                    //return Jan1st1970Utc.AddMilliseconds(timestamp).ToLocalTime();
                    return new DateTime(timestamp * 10000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();
                case TimestampType.TotalSeconds:
                    //return Jan1st1970Utc.AddSeconds(timestamp).ToLocalTime();
                    return new DateTime(timestamp * 10000000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();
                default:
                    throw new NotSupportedException($"Unsupported type: {timestampType}");
            }
        }
        #endregion
    }
}