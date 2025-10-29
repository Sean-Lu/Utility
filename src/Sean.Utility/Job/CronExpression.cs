using System;
using System.Collections.Generic;
using System.Linq;

namespace Sean.Utility.Job;

// Cron表达式格式：秒 分 时 日 月 周 [年]
// 
// 字段取值范围：
// 
// - 秒（Second）：0-59
// - 分（Minute）：0-59
// - 时（Hour）：0-23
// - 日（Day of month）：1-31
// - 月（Month）：1-12 或 JAN-DEC
// - 周（Day of week）：1-7 或 SUN-SAT (SUN=1)
// - 年（Year）：可选，1970-2099
// 
// 特殊字符：
// 
// - *：表示所有值
// - ?：表示不指定的值
// - -：表示范围
// - ,：表示多个值
// - /：表示步长
// - L：表示最后一天（仅在日字段和周字段）
// - #：表示第几周的星期几（仅在周字段，如6#3表示第三个星期五）
// 
// Cron表达式示例：
// 
// - 每隔5秒执行一次： */5 * * * * ?
// - 每天凌晨1点执行任务： 0 0 1 * * ?
// - 每月最后一天的10点15分执行任务： 0 15 10 L * ?
// - 每周一到周五的10点15分执行任务： 0 15 10 ? * MON-FRI
// - 每月第三个星期五的10点15分执行任务： 0 15 10 ? * 6#3

public class CronExpression
{
    private int[] seconds;
    private int[] minutes;
    private int[] hours;
    private int[] daysOfMonth;
    private int[] months;
    private int[] daysOfWeek;
    private int[] years;

    // 特殊标记
    private bool hasLastDayOfMonth = false;
    private List<DayOfWeekSpec> dayOfWeekSpecs = new();

    public CronExpression(string cron)
    {
        string[] fields = cron.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (fields.Length != 6 && fields.Length != 7)
        {
            throw new ArgumentException("Invalid cron expression format. Expected 6 or 7 fields.");
        }

        seconds = ParseField(fields[0], 0, 59, FieldType.Second);
        minutes = ParseField(fields[1], 0, 59, FieldType.Minute);
        hours = ParseField(fields[2], 0, 23, FieldType.Hour);
        daysOfMonth = ParseDayOfMonthField(fields[3]);
        months = ParseMonthField(fields[4]);
        daysOfWeek = ParseDayOfWeekField(fields[5]);
        years = fields.Length == 7
            ? ParseField(fields[6], 1970, 2099, FieldType.Year)
            : ArrayHelper.Empty<int>();
    }

    /// <summary>
    /// 获取下一次执行时间
    /// </summary>
    /// <param name="fromTime"></param>
    /// <returns></returns>
    public DateTime GetNextExecutionTime(DateTime fromTime)
    {
        DateTime next = fromTime.AddSeconds(1);

        while (true)
        {
            // 年
            if (years.Length > 0 && !years.Contains(next.Year))
            {
                int nextYear = years.Where(y => y >= next.Year).OrderBy(y => y).FirstOrDefault();
                if (nextYear == 0) // 没有符合条件的年份
                {
                    nextYear = years.Min();
                }
                next = new DateTime(nextYear, 1, 1, 0, 0, 0);
                continue;
            }

            // 月
            if (months.Length > 0 && !months.Contains(next.Month))
            {
                int nextMonth = months.Where(m => m >= next.Month).OrderBy(m => m).FirstOrDefault();
                if (nextMonth == 0) // 需要跨年
                {
                    nextMonth = months.Min();
                    next = new DateTime(next.Year + 1, nextMonth, 1, 0, 0, 0);
                }
                else
                {
                    next = new DateTime(next.Year, nextMonth, 1, 0, 0, 0);
                }
                continue;
            }

            // 日处理（需要考虑月份和星期规则）
            if (!IsDayValid(next))
            {
                next = next.AddDays(1);
                next = new DateTime(next.Year, next.Month, next.Day, 0, 0, 0);
                continue;
            }

            // 时
            if (hours.Length > 0 && !hours.Contains(next.Hour))
            {
                int nextHour = hours.Where(h => h >= next.Hour).OrderBy(h => h).FirstOrDefault();
                if (nextHour == 0) // 需要跨天
                {
                    nextHour = hours.Min();
                    next = new DateTime(next.Year, next.Month, next.Day, nextHour, 0, 0).AddDays(1);
                }
                else
                {
                    next = new DateTime(next.Year, next.Month, next.Day, nextHour, 0, 0);
                }
                continue;
            }

            // 分
            if (minutes.Length > 0 && !minutes.Contains(next.Minute))
            {
                int nextMinute = minutes.Where(m => m >= next.Minute).OrderBy(m => m).FirstOrDefault();
                if (nextMinute == 0) // 需要跨小时
                {
                    nextMinute = minutes.Min();
                    next = new DateTime(next.Year, next.Month, next.Day, next.Hour, nextMinute, 0).AddHours(1);
                }
                else
                {
                    next = new DateTime(next.Year, next.Month, next.Day, next.Hour, nextMinute, 0);
                }
                continue;
            }

            // 秒
            if (seconds.Length > 0 && !seconds.Contains(next.Second))
            {
                int nextSecond = seconds.Where(s => s >= next.Second).OrderBy(s => s).FirstOrDefault();
                if (nextSecond == 0) // 需要跨分钟
                {
                    nextSecond = seconds.Min();
                    next = new DateTime(next.Year, next.Month, next.Day, next.Hour, next.Minute, nextSecond).AddMinutes(1);
                }
                else
                {
                    next = new DateTime(next.Year, next.Month, next.Day, next.Hour, next.Minute, nextSecond);
                }
                continue;
            }

            break;
        }

        return next;
    }

    private bool IsDayValid(DateTime date)
    {
        // 处理日字段
        bool dayOfMonthValid = true;
        if (daysOfMonth.Length > 0 || hasLastDayOfMonth)
        {
            dayOfMonthValid = daysOfMonth.Contains(date.Day) || (hasLastDayOfMonth && date.Day == DateTime.DaysInMonth(date.Year, date.Month));
        }

        // 处理周字段
        bool dayOfWeekValid = true;
        if (daysOfWeek.Length > 0 || dayOfWeekSpecs.Count > 0)
        {
            dayOfWeekValid = daysOfWeek.Contains((int)date.DayOfWeek + 1) || dayOfWeekSpecs.Any(spec => spec.IsMatch(date));
        }

        // Cron标准：如果日字段和周字段都有约束，需要满足其中一个
        // 如果只有一个有约束，只需要满足那个约束
        if ((daysOfMonth.Length > 0 || hasLastDayOfMonth) && (daysOfWeek.Length > 0 || dayOfWeekSpecs.Count > 0))
        {
            return dayOfMonthValid || dayOfWeekValid;
        }
        else if (daysOfMonth.Length > 0 || hasLastDayOfMonth)
        {
            return dayOfMonthValid;
        }
        else if (daysOfWeek.Length > 0 || dayOfWeekSpecs.Count > 0)
        {
            return dayOfWeekValid;
        }

        return true;
    }

    private int[] ParseField(string field, int minValue, int maxValue, FieldType fieldType)
    {
        if (string.IsNullOrEmpty(field) || field == "?")
        {
            return ArrayHelper.Empty<int>();
        }

        List<int> values = new List<int>();
        string[] parts = field.Split(',');

        foreach (string part in parts)
        {
            if (part == "*")
            {
                for (int i = minValue; i <= maxValue; i++)
                {
                    values.Add(i);
                }
            }
            else if (part.Contains("/"))
            {
                ParseStep(part, minValue, maxValue, fieldType, values);
            }
            else if (part.Contains("-"))
            {
                ParseRange(part, minValue, maxValue, fieldType, values);
            }
            else
            {
                int value = ParseValue(part, fieldType);
                if (value >= minValue && value <= maxValue)
                {
                    values.Add(value);
                }
            }
        }

        return values.Distinct().OrderBy(x => x).ToArray();
    }

    private void ParseRange(string part, int minValue, int maxValue, FieldType fieldType, List<int> values)
    {
        string[] rangeParts = part.Split('-');
        if (rangeParts.Length != 2)
        {
            throw new ArgumentException($"Invalid range format: {part}");
        }

        int start = ParseValue(rangeParts[0], fieldType);
        int end = ParseValue(rangeParts[1], fieldType);

        if (start > end)
        {
            throw new ArgumentException($"Invalid range in cron expression: {part}");
        }

        for (int i = start; i <= end; i++)
        {
            if (i >= minValue && i <= maxValue)
            {
                values.Add(i);
            }
        }
    }

    private void ParseStep(string part, int minValue, int maxValue, FieldType fieldType, List<int> values)
    {
        string[] stepParts = part.Split('/');
        if (stepParts.Length != 2)
        {
            throw new ArgumentException($"Invalid step format: {part}");
        }

        string rangePart = stepParts[0];
        int step = int.Parse(stepParts[1]);

        if (step <= 0)
        {
            throw new ArgumentException($"Step must be positive: {step}");
        }

        int start, end;

        if (rangePart == "*" || string.IsNullOrEmpty(rangePart))
        {
            start = minValue;
            end = maxValue;
        }
        else if (rangePart.Contains("-"))
        {
            string[] range = rangePart.Split('-');
            start = ParseValue(range[0], fieldType);
            end = ParseValue(range[1], fieldType);
        }
        else
        {
            start = ParseValue(rangePart, fieldType);
            end = maxValue;
        }

        if (start < minValue) start = minValue;
        if (end > maxValue) end = maxValue;

        for (int i = start; i <= end; i += step)
        {
            if (i >= minValue && i <= maxValue)
            {
                values.Add(i);
            }
        }
    }

    private int[] ParseDayOfMonthField(string field)
    {
        if (field == "?" || string.IsNullOrEmpty(field))
        {
            return ArrayHelper.Empty<int>();
        }

        // 检查是否包含L（最后一天）
        if (field.Contains("L"))
        {
            hasLastDayOfMonth = true;

            // 如果只有L，返回空数组
            if (field == "L")
            {
                return ArrayHelper.Empty<int>();
            }

            // 如果包含L和其他内容（如"15L"），解析数字部分
            string numberPart = field.Replace("L", "");
            if (!string.IsNullOrEmpty(numberPart))
            {
                return ParseField(numberPart, 1, 31, FieldType.DayOfMonth);
            }

            return ArrayHelper.Empty<int>();
        }

        return ParseField(field, 1, 31, FieldType.DayOfMonth);
    }

    private int[] ParseMonthField(string field)
    {
        if (string.IsNullOrEmpty(field) || field == "?")
        {
            return ArrayHelper.Empty<int>();
        }

        return ParseField(field, 1, 12, FieldType.Month);
    }

    private int[] ParseDayOfWeekField(string field)
    {
        if (field == "?" || string.IsNullOrEmpty(field))
        {
            return ArrayHelper.Empty<int>();
        }

        List<int> values = new List<int>();
        string[] parts = field.Split(',');

        foreach (string part in parts)
        {
            // 解析#语法（如6#3表示第三个星期五）
            if (part.Contains("#"))
            {
                var hashParts = part.Split('#');
                if (hashParts.Length == 2)
                {
                    int dayOfWeek = ParseValue(hashParts[0], FieldType.DayOfWeek);
                    int weekNumber = int.Parse(hashParts[1]);
                    if (weekNumber >= 1 && weekNumber <= 5)
                    {
                        dayOfWeekSpecs.Add(new DayOfWeekSpec { DayOfWeek = dayOfWeek, WeekNumber = weekNumber });
                        //values.Add(dayOfWeek);
                    }
                }
                continue;
            }

            // 处理L（最后一个星期几）
            if (part.Contains("L"))
            {
                string numberPart = part.Replace("L", "");
                if (!string.IsNullOrEmpty(numberPart))
                {
                    int dayOfWeek = ParseValue(numberPart, FieldType.DayOfWeek);
                    // 对于"5L"这样的表达式，我们将其视为普通的日子，并在IsDayValid中特殊处理
                    values.Add(dayOfWeek);
                }
                continue;
            }

            // 普通解析
            if (part == "*")
            {
                for (int i = 1; i <= 7; i++)
                {
                    values.Add(i);
                }
            }
            else if (part.Contains("-"))
            {
                ParseRange(part, 1, 7, FieldType.DayOfWeek, values);
            }
            else if (part.Contains("/"))
            {
                ParseStep(part, 1, 7, FieldType.DayOfWeek, values);
            }
            else
            {
                int value = ParseValue(part, FieldType.DayOfWeek);
                if (value >= 1 && value <= 7)
                {
                    values.Add(value);
                }
            }
        }

        return values.Distinct().OrderBy(x => x).ToArray();
    }

    private int ParseValue(string value, FieldType fieldType)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }

        // 处理月份和星期的英文缩写
        switch (fieldType)
        {
            case FieldType.Month:
                return ParseMonth(value);
            case FieldType.DayOfWeek:
                return ParseDayOfWeek(value);
            default:
                throw new ArgumentException($"Invalid value '{value}' for field type {fieldType}");
        }
    }

    private int ParseMonth(string monthStr)
    {
        return monthStr.ToUpper() switch
        {
            "JAN" => 1,
            "FEB" => 2,
            "MAR" => 3,
            "APR" => 4,
            "MAY" => 5,
            "JUN" => 6,
            "JUL" => 7,
            "AUG" => 8,
            "SEP" => 9,
            "OCT" => 10,
            "NOV" => 11,
            "DEC" => 12,
            _ => throw new ArgumentException($"Invalid month value: {monthStr}")
        };
    }

    private int ParseDayOfWeek(string dayStr)
    {
        return dayStr.ToUpper() switch
        {
            "SUN" => 1,
            "MON" => 2,
            "TUE" => 3,
            "WED" => 4,
            "THU" => 5,
            "FRI" => 6,
            "SAT" => 7,
            _ => throw new ArgumentException($"Invalid day of week value: {dayStr}")
        };
    }

    private enum FieldType
    {
        Second,
        Minute,
        Hour,
        DayOfMonth,
        Month,
        DayOfWeek,
        Year
    }

    private class DayOfWeekSpec
    {
        /// <summary>
        /// 1-7 (SUN=1, MON=2, ..., SAT=7)
        /// </summary>
        public int DayOfWeek { get; set; }
        /// <summary>
        /// 1-5 (第几个出现的星期几)
        /// </summary>
        public int WeekNumber { get; set; }

        public bool IsMatch(DateTime date)
        {
            // 检查星期几是否匹配
            if ((int)date.DayOfWeek + 1 != DayOfWeek)
                return false;

            // 计算是该月的第几个出现的指定星期几
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            // 找到该月第一个指定的星期几
            int targetDow = DayOfWeek - 1; // 转换为C#的DayOfWeek (0-6)
            int daysUntilFirstOccurrence = (targetDow - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
            int firstOccurrenceDay = 1 + daysUntilFirstOccurrence;

            // 如果日期小于第一个出现日，肯定不匹配
            if (date.Day < firstOccurrenceDay)
                return false;

            // 计算是第几个出现（基于实际出现次数）
            int occurrenceNumber = (date.Day - firstOccurrenceDay) / 7 + 1;

            return occurrenceNumber == WeekNumber;
        }
    }
}