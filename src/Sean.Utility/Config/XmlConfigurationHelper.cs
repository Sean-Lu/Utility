using System;
using System.Xml;

namespace Sean.Utility.Config
{
    /// <summary>
    /// 配置文件操作
    /// </summary>
    public class XmlConfigurationHelper
    {
        #region AppSetting

        #region GetAppSetting
        /// <summary>
        /// 对[appSettings]节点依据Key值读取到Value值，返回字符串
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="key">要读取的Key值</param>
        /// <returns>返回Value值的字符串</returns>
        public static string GetAppSetting(string configFilePath, string key)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            return XmlHelper.GetXmlAttributeValue(configFilePath, "//appSettings", $"//add[@key=\'{key}\']", "value");
        }
        #endregion

        #region UpdateOrCreateAppSetting
        /// <summary>
        /// 更新或新增[appSettings]节点的子节点值，存在则更新子节点Value,不存在则新增子节点，返回成功与否布尔值
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="key">子节点Key值</param>
        /// <param name="value">子节点value值</param>
        /// <returns>返回成功与否布尔值</returns>
        public static bool UpdateOrCreateAppSetting(string configFilePath, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                var node = doc.SelectSingleNode("//appSettings");
                if (node == null)
                {
                    return false;
                }

                XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
                if (element != null)
                {
                    //存在则更新子节点
                    element.SetAttribute("value", value);
                }
                else
                {
                    //不存在则新增子节点
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("key", key);
                    subElement.SetAttribute("value", value);
                    node.AppendChild(subElement);
                }

                doc.Save(configFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region DeleteAppSetting
        /// <summary>
        /// 删除[appSettings]节点中包含Key值的子节点，返回成功与否布尔值
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="key">要删除的子节点Key值</param>
        /// <returns>返回成功与否布尔值</returns>
        public static bool DeleteAppSetting(string configFilePath, string key)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                XmlNode node = doc.SelectSingleNode("//appSettings");
                XmlElement element = (XmlElement)node?.SelectSingleNode($"//add[@key=\'{key}\']");
                if (element == null)
                    return false;

                element.ParentNode?.RemoveChild(element);
                doc.Save(configFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #endregion

        #region ConnectionString

        #region GetConnectionString
        /// <summary>
        /// 对[connectionStrings]节点依据name值读取到connectionString值，返回字符串
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="name">要读取的name值</param>
        /// <returns>返回connectionString值的字符串</returns>
        public static string GetConnectionString(string configFilePath, string name)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            return XmlHelper.GetXmlAttributeValue(configFilePath, "//connectionStrings", $"//add[@name=\'{name}\']", "connectionString");
        }
        /// <summary>
        /// 对[connectionStrings]节点依据name值读取到connectionString值，返回字符串
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="name">要读取的name值</param>
        /// <returns>返回connectionString值的字符串</returns>
        public static string GetProviderName(string configFilePath, string name)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            return XmlHelper.GetXmlAttributeValue(configFilePath, "//connectionStrings", $"//add[@name=\'{name}\']", "providerName");
        }
        #endregion

        #region UpdateOrCreateConnectionString
        /// <summary>
        /// 更新或新增[connectionStrings]节点的子节点值，存在则更新子节点,不存在则新增子节点，返回成功与否布尔值
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="name">子节点name值</param>
        /// <param name="connectionString">子节点connectionString值</param>
        /// <param name="providerName">子节点providerName值</param>
        /// <returns>返回成功与否布尔值</returns>
        public static bool UpdateOrCreateConnectionString(string configFilePath, string name, string connectionString, string providerName)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                XmlNode node = doc.SelectSingleNode("//connectionStrings");
                if (node == null)
                {
                    return false;
                }

                XmlElement element = (XmlElement)node.SelectSingleNode("//add[@name='" + name + "']");
                if (element != null)
                {
                    //存在则更新子节点
                    element.SetAttribute("connectionString", connectionString);
                    if (!string.IsNullOrWhiteSpace(providerName))
                        element.SetAttribute("providerName", providerName);
                }
                else
                {
                    //不存在则新增子节点
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("name", name);
                    subElement.SetAttribute("connectionString", connectionString);
                    if (!string.IsNullOrWhiteSpace(providerName))
                        subElement.SetAttribute("providerName", providerName);
                    node.AppendChild(subElement);
                }

                doc.Save(configFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region DeleteConnectionString
        /// <summary>
        /// 删除[connectionStrings]节点中包含name值的子节点，返回成功与否布尔值
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="name">要删除的子节点name值</param>
        /// <returns>返回成功与否布尔值</returns>
        public static bool DeleteConnectionString(string configFilePath, string name)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(configFilePath));

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                XmlNode node = doc.SelectSingleNode("//connectionStrings");
                XmlElement element = (XmlElement)node?.SelectSingleNode("//add[@name='" + name + "']");
                if (element == null)
                    return false;

                node.RemoveChild(element);
                doc.Save(configFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #endregion
    }
}
