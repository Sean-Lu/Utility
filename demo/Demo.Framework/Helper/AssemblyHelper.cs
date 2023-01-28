using System.Reflection;

namespace Demo.Framework.Helper
{
    /// <summary>
    /// 程序集操作
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// 获取调用方程序集的版本
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyVersion()
        {
            return Assembly.GetCallingAssembly().GetName().Version?.ToString();
        }
        /// <summary>
        /// 获取调用方程序集的名称（不包含后缀）
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyName()
        {
            return Assembly.GetCallingAssembly().GetName().Name;
        }
        /// <summary>
        /// 获取调用方程序集的文件路径（完整路径）
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyLocation()
        {
            return Assembly.GetCallingAssembly().Location;
        }
    }
}
