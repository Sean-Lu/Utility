#if !(NETSTANDARD || NET5_0_OR_GREATER)
using System.Configuration;

namespace Sean.Utility.Config
{
    public static class ConfigurationHelper
    {
        #region AppSetting
        /// <summary>
        /// 获取AppSetting
        /// </summary>
        /// <param name="key"></param>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 修改AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 新增AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 删除AppSetting
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppSetting(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion

        #region ConnectionString
        /// <summary>
        /// 获取ConnectionString
        /// </summary>
        /// <param name="name"></param>
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// 修改ConnectionString
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public static void SetConnectionString(string name, string connectionString)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// 新增ConnectionString
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public static void AddConnectionString(string name, string connectionString)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// 删除ConnectionString
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveConnectionString(string name)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Remove(name);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
        #endregion
    }
}
#endif