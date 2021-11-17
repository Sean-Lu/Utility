using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sean.Utility.Serialize
{
    /// <summary>
    /// 二进制序列化\反序列化
    /// </summary>
    public class BinarySerializer
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        public static BinarySerializer Instance { get; } = new BinarySerializer();

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public byte[] Serialize(object item)
        {
            using (var ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, item);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 序列化对象到文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fileName">文件名。eg: @"D:\Demo.dat"</param>
        public void SerializeToFile<T>(T obj, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                _binaryFormatter.Serialize(fs, obj);
            }
        }
        #endregion

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public object Deserialize(byte[] serializedObject)
        {
            using (var ms = new MemoryStream(serializedObject))
            {
                return _binaryFormatter.Deserialize(ms);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] serializedObject)
        {
            using (var memoryStream = new MemoryStream(serializedObject))
            {
                return (T)_binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// 从文件中反序列化出对象
        /// </summary>
        /// <param name="fileName">文件名。eg: @"D:\Demo.dat"</param>
        /// <returns>返回反序列化后的对象</returns>
        public T DeserializeFromFile<T>(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                return (T)_binaryFormatter.Deserialize(fs);
            }
        }
        #endregion
    }
}
