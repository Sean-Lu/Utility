using Sean.Utility.Contracts;
using Sean.Utility.Serialize;

namespace Sean.Utility.Format
{
    public class JsonHelper
    {
        /// <summary>
        /// 获取或设置 <see cref="IJsonSerializer"/> ，默认值：<see cref="JsonSerializer.Instance"/>
        /// </summary>
        public static IJsonSerializer Serializer { get; set; } = JsonSerializer.Instance;

        /// <summary>
        /// json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            return Serializer.Serialize(obj);
        }
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string data)
        {
            return Serializer.Deserialize<T>(data);
        }
    }
}
