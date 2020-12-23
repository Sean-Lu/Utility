using System;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Type"/>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
