#if !NETSTANDARD
using System.Configuration;
using System.Web.Configuration;

namespace Sean.Utility.Web.Config
{
    /// <summary>
    /// 配置文件操作（Web.config）
    /// </summary>
    public class WebConfigHelper
    {
        #region AppSetting

        /// <summary>
        /// 获取AppSetting
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 修改AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSetting(string key, string value)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.AppSettings.Settings[key].Value = value; 
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 新增AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 删除AppSetting
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppSetting(string key)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
        }

        #endregion

        #region ConnectionString

        /// <summary>
        /// 获取ConnectionString
        /// </summary>
        /// <param name="name"></param>
        public static string GetConnectionString(string name)
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// 修改ConnectionString
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public static void SetConnectionString(string name, string connectionString)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 新增ConnectionString
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public static void AddConnectionString(string name, string connectionString)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString));
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 删除ConnectionString
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveConnectionString(string name)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            config.ConnectionStrings.ConnectionStrings.Remove(name);
            config.Save(ConfigurationSaveMode.Modified);
        }

        #endregion
    }
}
#endif