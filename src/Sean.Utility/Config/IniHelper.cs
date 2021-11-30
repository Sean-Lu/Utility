using System.Runtime.InteropServices;
using System.Text;

namespace Sean.Utility.Config
{
    /// <summary>
    /// ini文件操作
    /// </summary>
    public class IniHelper
    {
        #region DllImport
        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值：当无对应键值，则返回该值。</param>
        /// <param name="retValue">结果缓冲区</param>
        /// <param name="bufferSize">结果缓冲区大小</param>
        /// <param name="filePath">ini文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retValue, int bufferSize, string filePath);
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">ini文件路径</param>
        /// <returns>0：写入失败；1：写入成功</returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
        #endregion

        #region Methods
        private IniHelper() { }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <returns>返回读取值</returns>
        public static string Read(string filePath, string section, string key)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            GetPrivateProfileString(section, key, "", stringBuilder, stringBuilder.MaxCapacity, filePath);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>true：成功；false：失败。</returns>
        public static bool Write(string filePath, string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, filePath) > 0;
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <returns>true：成功；false：失败。</returns>
        public static bool DeleteKey(string filePath, string section, string key)
        {
            return WritePrivateProfileString(section, key, null, filePath) > 0;
        }

        /// <summary>
        /// 删除节
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="section">节</param>
        /// <returns>true：成功；false：失败。</returns>
        public static bool DeleteSection(string filePath, string section)
        {
            return WritePrivateProfileString(section, null, null, filePath) > 0;
        }
        #endregion
    }
}
