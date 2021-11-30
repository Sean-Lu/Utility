using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Sean.Utility.Serialize
{
    /// <summary>
    /// json序列化\反序列化
    /// </summary>
    public class JsonSerializer
    {
        private JsonSerializer()
        {
#if NETSTANDARD || NET45_OR_GREATER
            Settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:sszzz")
            };
#endif
            DefaultEncoding = Encoding.UTF8;
        }

        public static JsonSerializer Instance { get; } = new JsonSerializer();

#if NETSTANDARD || NET45_OR_GREATER
        public DataContractJsonSerializerSettings Settings;
#endif
        /// <summary>
        /// 默认编码格式：<see cref="Encoding.UTF8"/>
        /// </summary>
        public Encoding DefaultEncoding;

        /// <summary>
        /// 序列化为json对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
#if NETSTANDARD || NET45_OR_GREATER
            var serializer = new DataContractJsonSerializer(typeof(T), Settings);
#else
            var serializer = new DataContractJsonSerializer(typeof(T));
#endif
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return DefaultEncoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// json对象反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(string data)
        {
            using (var stream = new MemoryStream(DefaultEncoding.GetBytes(data)))
            {
#if NETSTANDARD || NET45_OR_GREATER
                var serializer = new DataContractJsonSerializer(typeof(T), Settings);
#else
                var serializer = new DataContractJsonSerializer(typeof(T));
#endif
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
