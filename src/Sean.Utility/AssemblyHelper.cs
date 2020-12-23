using System;
using System.IO;
using System.Reflection;

namespace Sean.Utility
{
    /// <summary>
    /// 程序集操作
    /// </summary>
    public class AssemblyHelper
    {
        private AssemblyHelper() { }

        /// <summary>
        /// 获取调用方程序集的版本
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyVersion()
        {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
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
        /// 获取调用方程序集的文件名称（包含文件后缀）
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyFileName()
        {
            return Path.GetFileName(GetCallingAssemblyLocation());
        }
        /// <summary>
        /// 获取调用方程序集的文件路径（完整路径）
        /// </summary>
        /// <returns></returns>
        public static string GetCallingAssemblyLocation()
        {
            return Assembly.GetCallingAssembly().Location;
        }

        /// <summary>
        /// 获取当前应用程序域的基目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomainBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 获得应用程序的当前工作目录
        /// </summary>
        public static string GetCurWorkDir()
        {
            return Directory.GetCurrentDirectory();
        }
        /// <summary>
        /// 将应用程序的当前工作目录设置为指定的目录
        /// </summary>
        /// <param name="path">指定的目录</param>
        public static void SetCurWorkDir(string path)
        {
            Directory.SetCurrentDirectory(path);
        }
    }
}
