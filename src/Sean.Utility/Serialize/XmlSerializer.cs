using System.IO;
using System.Xml.Serialization;

namespace Sean.Utility.Serialize
{
    /// <summary>
    /// XML序列化\反序列化
    /// </summary>
    public class XmlSerializer<T>
    {
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(T));

        public static XmlSerializer<T> Instance { get; } = new XmlSerializer<T>();

        #region 序列化
        /// <summary>
        /// 序列化对象：实体类转换成XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(T obj)
        {
            using (var sw = new StringWriter())
            {
                _xmlSerializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 序列化对象到文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fileName">文件名。eg: @"D:\Demo.xml"</param>
        public void SerializeToFile(T obj, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                _xmlSerializer.Serialize(fs, obj);
            }
        }
        #endregion

        #region 反序列化
        /// <summary>
        /// 反序列化出对象：XML转换成实体类
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return (T)_xmlSerializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// 从文件中反序列化出对象
        /// </summary>
        /// <param name="fileName">文件名。eg: @"D:\Demo.xml"</param>
        /// <returns>返回反序列化后的对象</returns>
        public T DeserializeFromFile(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                return (T)_xmlSerializer.Deserialize(fs);
            }
        }
        #endregion
    }
}
