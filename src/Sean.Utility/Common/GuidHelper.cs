using System;

namespace Sean.Utility.Common
{
    /// <summary>
    /// GUID：全局唯一标识符
    /// </summary>
    public class GuidHelper
    {
        /// <summary>
        /// 生成GUID字符串（默认小写）
        /// </summary>
        /// <returns></returns>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 生成GUID字符串（默认小写，不带分隔符）
        /// </summary>
        /// <returns></returns>
        public static string NewGuidWithoutSeparator()
        {
            return NewGuid().Replace("-", string.Empty);
        }
        /// <summary>
        /// 生成GUID字符串（大写）
        /// </summary>
        /// <returns></returns>
        public static string NewGuidUpper()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
        /// <summary>
        /// 生成GUID字符串（大写，不带分隔符）
        /// </summary>
        /// <returns></returns>
        public static string NewGuidUpperWithoutSeparator()
        {
            return NewGuidUpper().Replace("-", string.Empty);
        }
    }
}
