using System.Linq;

namespace Sean.Utility.Extensions;

/// <summary>
/// Extensions for <see cref="string"/>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Prevent SQL injection attacks by piecing together the parameter values ​​of SQL strings.
    /// </summary>
    /// <remarks>
    /// 防止拼凑SQL字符串的参数值进行SQL注入攻击。
    /// </remarks>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static string PreventSqlInjection(this string parameter)
    {
        return string.IsNullOrWhiteSpace(parameter) ? parameter : parameter.Replace("'", "''");
    }

    /// <summary>
    /// Count the number of occurrences of the <paramref name = "search" /> substring in the <paramref name = "source" /> string.
    /// </summary>
    /// <remarks>
    /// 统计字符串中子串出现的次数
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public static int GetSubstringCount(this string source, string search)
    {
        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(search) || !source.Contains(search))
        {
            return 0;
        }

        return (source.Length - source.Replace(search, string.Empty).Length) / search.Length;
    }

    /// <summary>
    /// Whether it contains letters.
    /// </summary>
    /// <remarks>
    /// 是否包含字母
    /// </remarks>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool ContainLetter(this string content)
    {
        return !string.IsNullOrWhiteSpace(content) && content.Any(char.IsLetter);
    }
}