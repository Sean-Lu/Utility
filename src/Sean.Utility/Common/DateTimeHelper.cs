using System;
using Sean.Utility.Enums;

namespace Sean.Utility.Common
{
    /// <summary>
    /// 日期时间
    /// </summary>
    public class DateTimeHelper
    {
        private DateTimeHelper() { }

        #region 获取指定年份的天数

        /// <summary>获取指定日期所属年份的天数</summary>  
        /// <param name="dt">日期</param>  
        /// <returns>本天在当年的天数</returns>  
        public static int GetDaysOfYear(DateTime dt)
        {
            return GetDaysOfYear(dt.Year);
        }

        /// <summary>获取指定年份的天数</summary>  
        /// <param name="year">年份</param>  
        /// <returns>本年的天数</returns>  
        public static int GetDaysOfYear(int year)
        {
            return IsLeapYear(year) ? 366 : 365;
        }

        #endregion

        #region 获取指定月份的天数

        /// <summary>获取指定日期所属月份的天数</summary>  
        /// <param name="dt">日期</param>  
        /// <returns>天数</returns>  
        public static int GetDaysOfMonth(DateTime dt)
        {
            return GetDaysOfMonth(dt.Year, dt.Month);
        }

        /// <summary>获取指定月份的天数</summary>  
        /// <param name="year">年</param>  
        /// <param name="month">月</param>  
        /// <returns>天数</returns>  
        public static int GetDaysOfMonth(int year, int month)
        {
            int days = 0;
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    days = IsLeapYear(year) ? 29 : 28;
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }
            return days;
        }

        #endregion

        #region 获取指定日期的星期名称、编号

        /// <summary>获取指定日期的星期名称</summary>  
        /// <param name="dt">日期</param>
        /// <param name="english">是否返回英文格式。默认false（返回中文格式）</param>
        /// <returns>星期名称</returns>  
        public static string GetWeekNameOfDay(DateTime dt, bool english = false)
        {
            string week = dt.DayOfWeek.ToString();
            if (english)
                return week;
            switch (week)
            {
                case "Mondy":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;
            }
            return week;
        }

        /// <summary>获取指定日期的星期编号。范围：1~7</summary>  
        /// <param name="dt">日期</param>  
        /// <returns>星期数字编号</returns>  
        public static int GetWeekNumberOfDay(DateTime dt)
        {
            int week = -1;
            switch (dt.DayOfWeek.ToString())
            {
                case "Mondy":
                    week = 1;
                    break;
                case "Tuesday":
                    week = 2;
                    break;
                case "Wednesday":
                    week = 3;
                    break;
                case "Thursday":
                    week = 4;
                    break;
                case "Friday":
                    week = 5;
                    break;
                case "Saturday":
                    week = 6;
                    break;
                case "Sunday":
                    week = 7;
                    break;
            }
            return week;
        }

        #endregion

        #region 获取两个日期之间的时间间隔
        /// <summary>  
        /// 获取两个日期之间的时间间隔
        /// </summary>  
        /// <param name="startDate">开始日期</param>  
        /// <param name="endDate">结束日期</param>  
        /// <returns>时间间隔</returns>  
        public static TimeSpan GetDateDiff(DateTime startDate, DateTime endDate)
        {
            return endDate - startDate;
        }
        #endregion

        #region 判断指定年份是否是闰年

        /// <summary>判断指定日期所属年份是否是闰年</summary>  
        /// <param name="dt">日期</param>  
        /// <returns>是闰年：True ，不是闰年：False</returns>  
        public static bool IsLeapYear(DateTime dt)
        {
            return IsLeapYear(dt.Year);
        }

        /// <summary>判断指定年份是否是闰年</summary>  
        /// <param name="year">年份</param>  
        /// <returns>是闰年：True ，不是闰年：False</returns>  
        public static bool IsLeapYear(int year)
        {
            if ((year % 400 == 0) || (year % 4 == 0 && year % 100 != 0))
                return true;
            else
                return false;
        }

        #endregion

        #region 格式化日期时间
        /// <summary>  
        /// 格式化日期时间。
        /// 如果格式化失败，则返回null。
        /// </summary>  
        /// <param name="dateTime">待格式化对象</param>  
        /// <param name="format">格式。如"yyyy-MM-dd HH:mm:ss"</param>  
        /// <returns></returns>  
        public static string ToString(object dateTime, string format)
        {
            string strDate;
            try
            {
                strDate = Convert.ToDateTime(dateTime).ToString(format);
            }
            catch
            {
                strDate = null;
            }
            return strDate;
        }
        #endregion

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
        /// <param name="timeZoneInfo">时区</param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime time, TimestampType timestampType = TimestampType.JavaScript)
        {
            switch (timestampType)
            {
                case TimestampType.JavaScript:
                    //return (long)(time.ToUniversalTime() - Jan1st1970Utc).TotalMilliseconds;
                    return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
                case TimestampType.Unix:
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
        /// <param name="timeZoneInfo">时区</param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp, TimestampType timestampType = TimestampType.JavaScript)
        {
            switch (timestampType)
            {
                case TimestampType.JavaScript:
                    //return Jan1st1970Utc.AddMilliseconds(timestamp).ToLocalTime();
                    return new DateTime(timestamp * 10000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();
                case TimestampType.Unix:
                    //return Jan1st1970Utc.AddSeconds(timestamp).ToLocalTime();
                    return new DateTime(timestamp * 10000000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();
                default:
                    throw new NotSupportedException($"Unsupported type: {timestampType}");
            }
        }
        #endregion
    }
}