using System.IO;

namespace Sean.Utility.Serialize
{
    /// <summary>
    /// XML序列化\反序列化
    /// </summary>
    public class XmlSerializer
    {
        public static XmlSerializer Instance { get; } = new XmlSerializer();

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            using (var sw = new StringWriter())
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                xmlSerializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        public void SerializeToFile<T>(T obj, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize<T>(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeserializeFromFile<T>(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }

    public class XmlSerializer<T> : XmlSerializer
    {

    }
}
