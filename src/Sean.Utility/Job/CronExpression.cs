using System;
using System.Collections.Generic;
using System.Linq;

namespace Sean.Utility.Job;

// 标准的Cron表达式通常包括：秒 分 时 日 月 周 年
//
// 秒（Second）：0-59
// 分（Minute）：0-59
// 时（Hour）：0-23
// 日（Day of month）：1-31
// 月（Month）：1-12 或 JAN-DEC
// 周（Day of week）：1-7 或 SUN-SAT
// 年（Year）：可选，1970-2099
//
// CronExpression类用于解析和计算Cron表达式的下一个发生时间，支持处理*（任意值）、?（不指定值）、,（列表）、-（范围）和/（步长）这些特殊字符。

public class CronExpression
{
    private int[] seconds;
    private int[] minutes;
    private int[] hours;
    private int[] daysOfMonth;
    private int[] months;
    private int[] daysOfWeek;
    private int[] year;

    public CronExpression(string cron)
    {
        string[] fields = cron.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (fields.Length != 6 && fields.Length != 7)
        {
            throw new ArgumentException("Invalid cron expression format. Expected 6 or 7 fields.");
        }

        seconds = ParseField(fields[0], 0, 59);
        minutes = ParseField(fields[1], 0, 59);
        hours = ParseField(fields[2], 0, 23);
        daysOfMonth = ParseField(fields[3], 1, 31);
        months = ParseField(fields[4], 0, 11);
        daysOfWeek = ParseField(fields[5], 0, 6);
        year = fields.Length == 7
            ? ParseField(fields[6], 1970, 2099)
#if NET40 || NET45
            : new List<int>().ToArray();
#else
            : Array.Empty<int>();
#endif
    }

    /// <summary>
    /// 获取下一次执行时间
    /// </summary>
    /// <param name="fromTime"></param>
    /// <returns></returns>
    public DateTime GetNextExecutionTime(DateTime fromTime)
    {
        DateTime nextExecutionTime = fromTime.AddSeconds(1);

        #region 秒
        if (seconds.Length != 0 && !seconds.Contains(nextExecutionTime.Second))
        {
            do
            {
                nextExecutionTime = nextExecutionTime.AddSeconds(1);
            } while (!seconds.Contains(nextExecutionTime.Second));
        }
        #endregion

        #region 分钟
        if (minutes.Length != 0 && !minutes.Contains(nextExecutionTime.Minute))
        {
            do
            {
                nextExecutionTime = nextExecutionTime.AddMinutes(1);
            } while (!minutes.Contains(nextExecutionTime.Minute));
        }
        #endregion

        #region 小时
        if (hours.Length != 0 && !hours.Contains(nextExecutionTime.Hour))
        {
            do
            {
                nextExecutionTime = nextExecutionTime.AddHours(1);
            } while (!hours.Contains(nextExecutionTime.Hour));
        }
        #endregion

        #region 日
        if (daysOfMonth.Length != 0 && !daysOfMonth.Contains(nextExecutionTime.Day))
        {
            do
            {
                nextExecutionTime = nextExecutionTime.AddDays(1);
            } while (!daysOfMonth.Contains(nextExecutionTime.Day));
        }
        #endregion

        #region 月
        if (months.Length != 0 && !months.Contains(nextExecutionTime.Month))
        {
            do
            {
                nextExecutionTime = nextExecutionTime.AddMonths(1);
            } while (!months.Contains(nextExecutionTime.Month));
        }
        #endregion

        #region 年
        if (year.Length != 0 && !year.Contains(nextExecutionTime.Year))
        {
            var maxYear = year.Max();
            if (nextExecutionTime.Year + 1 > maxYear)
            {
                nextExecutionTime = nextExecutionTime.AddYears(maxYear - nextExecutionTime.Year);
            }
            else
            {
                do
                {
                    nextExecutionTime = nextExecutionTime.AddYears(1);
                } while (!year.Contains(nextExecutionTime.Year));
            }
        }
        #endregion

        return nextExecutionTime;
    }

    private bool IsMatch(DateTime time)
    {
        return (seconds.Length == 0 || seconds.Contains(time.Second)) &&
               (minutes.Length == 0 || minutes.Contains(time.Minute)) &&
               (hours.Length == 0 || hours.Contains(time.Hour)) &&
               (daysOfMonth.Length == 0 || daysOfMonth.Contains(time.Day)) &&
               (months.Length == 0 || months.Contains(time.Month)) &&
               (daysOfWeek.Length == 0 || daysOfWeek.Contains((int)time.DayOfWeek + 1)) &&
               (year.Length == 0 || year.Contains(time.Year));
    }

    private int[] ParseField(string field, int minValue, int maxValue)
    {
        List<int> values = new List<int>();

        string[] parts = field.Split(',', ' ');

        foreach (string part in parts)
        {
            if (part == "*")
            {
                for (int i = minValue; i <= maxValue; i++)
                {
                    values.Add(i);
                }
            }
            else if (part == "?")
            {
                // It is typically used in the day-of-month and day-of-week fields to indicate no specific value  
                //throw new NotImplementedException("The '?' character is not supported in this CronExpression implementation.");
            }
            else if (part.Contains("-"))
            {
                string[] range = part.Split('-');
                int start = int.Parse(range[0]);
                int end = int.Parse(range[1]);

                if (start > end)
                {
                    throw new ArgumentException("Invalid range in cron expression.");
                }

                for (int i = start; i <= end; i++)
                {
                    if (i >= minValue && i <= maxValue)
                    {
                        values.Add(i);
                    }
                }
            }
            else if (part.Contains("/"))
            {
                string[] stepParts = part.Split('/');
                int step = int.Parse(stepParts[1]);
                int start = stepParts[0] == "" ? minValue : int.Parse(stepParts[0]);

                for (int i = start; i <= maxValue; i += step)
                {
                    values.Add(i);
                }
            }
            else
            {
                int value = int.Parse(part);
                if (value >= minValue && value <= maxValue)
                {
                    values.Add(value);
                }
            }
        }

        return values.ToArray();
    }
}