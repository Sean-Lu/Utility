using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Sean.Utility.Config;

/// <summary>
/// XML文件操作
/// </summary>
public static class XmlHelper
{
    #region XML文档创建
    /// <summary>
    /// 创建一个XML文档
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
    /// <param name="version">XML文档版本号(必须为:"1.0")</param>
    /// <param name="encoding">XML文档编码方式</param>
    /// <param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
    public static bool CreateXmlDocument(string xmlFilePath, string rootNodeName, string version = "1.0", string encoding = "utf-8", string standalone = null)
    {
        if (string.IsNullOrWhiteSpace(xmlFilePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(xmlFilePath));

        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
        xmlDoc.AppendChild(xmlDeclaration);
        if (!string.IsNullOrWhiteSpace(rootNodeName))
        {
            XmlNode root = xmlDoc.CreateElement(rootNodeName);
            xmlDoc.AppendChild(root);
        }
        xmlDoc.Save(xmlFilePath);
        return true;
    }
    #endregion

    #region XML文档节点或属性的新增、修改
    /// <summary>
    /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点)
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
    /// <param name="innerText">节点文本值</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <param name="value">属性值</param>
    public static bool CreateXmlNode(string xmlFilePath, string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
    {
        if (string.IsNullOrWhiteSpace(xmlFilePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(xmlFilePath));

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode == null)
        {
            return false;
        }

        //存不存在此节点都创建
        XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
        subElement.InnerXml = innerText;

        //如果属性和值参数都不为空则在此新节点上新增属性
        if (!string.IsNullOrWhiteSpace(xmlAttributeName) && !string.IsNullOrWhiteSpace(value))
        {
            XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
            xmlAttribute.Value = value;
            subElement.Attributes.Append(xmlAttribute);
        }

        xmlNode.AppendChild(subElement);
        xmlDoc.Save(xmlFilePath);
        return true;
    }

    /// <summary>
    /// 依据匹配XPath表达式的第一个节点来创建或更新它的子节点(如果节点存在则更新,不存在则创建)
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
    /// <param name="innerText">节点文本值</param>
    public static bool CreateOrUpdateXmlNode(string xmlFilePath, string xpath, string xmlNodeName, string innerText)
    {
        if (string.IsNullOrWhiteSpace(xmlFilePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(xmlFilePath));

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode == null)
        {
            return false;
        }

        bool isExistsNode = false; //标识节点是否存在
        //遍历xpath节点下的所有子节点
        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            if (node.Name.ToLower() == xmlNodeName.ToLower())
            {
                //存在此节点则更新
                node.InnerXml = innerText;
                isExistsNode = true;
                break;
            }
        }

        if (!isExistsNode)
        {
            //不存在此节点则创建
            XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
            subElement.InnerXml = innerText;
            xmlNode.AppendChild(subElement);
        }

        xmlDoc.Save(xmlFilePath);
        return true;
    }

    /// <summary>
    /// 依据匹配XPath表达式的第一个节点来创建或更新它的属性(如果属性存在则更新,不存在则创建)
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <param name="value">属性值</param>
    public static bool CreateOrUpdateXmlAttribute(string xmlFilePath, string xpath, string xmlAttributeName, string value)
    {
        if (string.IsNullOrWhiteSpace(xmlFilePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(xmlFilePath));

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode?.Attributes == null)
        {
            return false;
        }

        bool isExistsAttribute = false; //标识属性是否存在
        //遍历xpath节点中的所有属性
        foreach (XmlAttribute attribute in xmlNode.Attributes)
        {
            if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
            {
                //节点中存在此属性则更新
                attribute.Value = value;
                isExistsAttribute = true;
                break;
            }
        }

        if (!isExistsAttribute)
        {
            //节点中不存在此属性则创建
            XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
            xmlAttribute.Value = value;
            xmlNode.Attributes.Append(xmlAttribute);
        }

        xmlDoc.Save(xmlFilePath);
        return true;
    }
    #endregion

    #region XML文档节点或属性的查询

    #region GetXmlNode
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点XmlNode.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns>返回XmlNode</returns>
    public static XmlNode GetXmlNode(string xmlFilePath, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        return xmlDoc.SelectSingleNode(xpath);
    }
    #endregion

    #region GetXmlNodeInnerText
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点XmlNode的文本值.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns>返回字符串</returns>
    public static string GetXmlNodeInnerText(string xmlFilePath, string xpath)
    {
        XmlNode xmlNode = GetXmlNode(xmlFilePath, xpath);
        return xmlNode != null ? xmlNode.InnerText : string.Empty;
    }
    #endregion

    #region GetXmlNodeLocalName
    /// <summary>
    /// 获取节点的本地名称。
    /// 注：注释内容"<!--注释-->"的本地名称为"#comment"。
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns></returns>
    public static string GetXmlNodeLocalName(string xmlFilePath, string xpath)
    {
        return GetXmlNodeLocalName(GetXmlNode(xmlFilePath, xpath));
    }
    /// <summary>
    /// 获取节点的本地名称。
    /// 注：注释内容"<!--注释-->"的本地名称为"#comment"。
    /// </summary>
    /// <param name="xmlNode">节点XmlNode</param>
    /// <returns></returns>
    public static string GetXmlNodeLocalName(XmlNode xmlNode)
    {
        return xmlNode.LocalName;
    }
    #endregion

    #region GetXmlNodeList
    /// <summary>
    /// 选择匹配XPath表达式的节点列表XmlNodeList.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns>返回XmlNodeList</returns>
    public static XmlNodeList GetXmlNodeList(string xmlFilePath, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
        return xmlNodeList;
    }
    #endregion

    #region GetXmlNodeListInnerText
    /// <summary>
    /// 选择匹配XPath表达式的节点列表XmlNodeList的文本值.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns>返回XmlNodeList</returns>
    public static List<string> GetXmlNodeListInnerText(string xmlFilePath, string xpath)
    {
        XmlNodeList xmlNodeList = GetXmlNodeList(xmlFilePath, xpath);
        return xmlNodeList.Cast<XmlNode>().Select(xmlNode => xmlNode.InnerText).ToList();
    }
    #endregion

    #region GetXmlNodeChildNodes
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点XmlNode.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
    /// <returns>返回XmlNode</returns>
    public static XmlNodeList GetXmlNodeChildNodes(string xmlFilePath, string xpath)
    {
        return GetXmlNodeChildNodes(GetXmlNode(xmlFilePath, xpath));
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点XmlNode.
    /// </summary>
    /// <param name="xmlNode">节点XmlNode</param>
    /// <returns>返回XmlNode</returns>
    public static XmlNodeList GetXmlNodeChildNodes(XmlNode xmlNode)
    {
        return xmlNode.ChildNodes;
    }
    #endregion

    #region GetXmlAttribute
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static XmlAttribute GetXmlAttribute(string xmlFilePath, string xpath, string xmlAttributeName)
    {
        return GetXmlAttribute(xmlFilePath, xpath, null, xmlAttributeName);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="parentXPath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="elementXPath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static XmlAttribute GetXmlAttribute(string xmlFilePath, string parentXPath, string elementXPath, string xmlAttributeName)
    {
        var xmlNode = GetXmlNode(xmlFilePath, parentXPath);
        if (xmlNode != null && !string.IsNullOrWhiteSpace(elementXPath))
        {
            xmlNode = xmlNode.SelectSingleNode(elementXPath);
        }
        return GetXmlAttribute(xmlNode, xmlAttributeName);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
    /// </summary>
    /// <param name="xmlNode">节点XmlNode</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static XmlAttribute GetXmlAttribute(XmlNode xmlNode, string xmlAttributeName)
    {
        if (xmlNode?.Attributes == null || xmlNode.Attributes.Count < 1)
        {
            return null;
        }

        return xmlNode.Attributes[xmlAttributeName];
    }
    #endregion

    #region GetXmlAttributeInnerText
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的文本值.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeInnerText(string xmlFilePath, string xpath, string xmlAttributeName)
    {
        var xmlAttribute = GetXmlAttribute(xmlFilePath, xpath, xmlAttributeName);
        return GetXmlAttributeInnerText(xmlAttribute);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的文本值.
    /// </summary>
    /// <param name="xmlNode">节点XmlNode</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeInnerText(XmlNode xmlNode, string xmlAttributeName)
    {
        var xmlAttribute = GetXmlAttribute(xmlNode, xmlAttributeName);
        return GetXmlAttributeInnerText(xmlAttribute);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的文本值.
    /// </summary>
    /// <param name="xmlAttribute">属性XmlAttribute</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeInnerText(XmlAttribute xmlAttribute)
    {
        return xmlAttribute?.InnerText ?? string.Empty;
    }
    #endregion

    #region GetXmlAttributeValue
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的值.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeValue(string xmlFilePath, string xpath, string xmlAttributeName)
    {
        var xmlAttribute = GetXmlAttribute(xmlFilePath, xpath, xmlAttributeName);
        return GetXmlAttributeValue(xmlAttribute);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的值.
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="parentXPath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="elementXPath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeValue(string xmlFilePath, string parentXPath, string elementXPath, string xmlAttributeName)
    {
        var xmlAttribute = GetXmlAttribute(xmlFilePath, parentXPath, elementXPath, xmlAttributeName);
        return GetXmlAttributeValue(xmlAttribute);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的值.
    /// </summary>
    /// <param name="xmlNode">节点XmlNode</param>
    /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeValue(XmlNode xmlNode, string xmlAttributeName)
    {
        var xmlAttribute = GetXmlAttribute(xmlNode, xmlAttributeName);
        return GetXmlAttributeValue(xmlAttribute);
    }
    /// <summary>
    /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute的值.
    /// </summary>
    /// <param name="xmlAttribute">属性XmlAttribute</param>
    /// <returns>返回xmlAttributeName</returns>
    public static string GetXmlAttributeValue(XmlAttribute xmlAttribute)
    {
        return xmlAttribute?.Value ?? string.Empty;
    }
    #endregion

    #endregion

    #region XML文档节点或属性的删除
    /// <summary>
    /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    public static bool DeleteXmlNode(string xmlFilePath, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode?.ParentNode == null)
        {
            return false;
        }

        xmlNode.ParentNode.RemoveChild(xmlNode);
        xmlDoc.Save(xmlFilePath);
        return true;
    }

    /// <summary>
    /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    /// <param name="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>
    public static bool DeleteXmlAttribute(string xmlFilePath, string xpath, string xmlAttributeName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode?.Attributes == null)
        {
            return false;
        }

        //遍历xpath节点中的所有属性
        bool isExistsAttribute = false;
        XmlAttribute xmlAttribute = null;
        foreach (XmlAttribute attribute in xmlNode.Attributes)
        {
            if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
            {
                //节点中存在此属性
                xmlAttribute = attribute;
                isExistsAttribute = true;
                break;
            }
        }

        if (!isExistsAttribute)
        {
            return false;
        }

        //删除节点中的属性
        xmlNode.Attributes.Remove(xmlAttribute);

        xmlDoc.Save(xmlFilePath);
        return true;
    }

    /// <summary>
    /// 删除匹配XPath表达式的第一个节点中的所有属性
    /// </summary>
    /// <param name="xmlFilePath">XML文档路径</param>
    /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
    public static bool DeleteAllXmlAttribute(string xmlFilePath, string xpath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
        if (xmlNode?.Attributes == null)
        {
            return false;
        }

        xmlNode.Attributes.RemoveAll();
        xmlDoc.Save(xmlFilePath);
        return true;
    }
    #endregion
}