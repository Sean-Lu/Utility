using System.Text.RegularExpressions;

namespace Sean.Utility.Common
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 匹配正则表达式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string regex)
        {
            return Regex.IsMatch(input, regex);
        }

        /// <summary>
        /// 匹配正则表达式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static Match Match(string input, string regex)
        {
            return Regex.Match(input, regex);
        }
    }
}
