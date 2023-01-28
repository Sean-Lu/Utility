using Demo.NetCore.Contracts;
using Newtonsoft.Json;

namespace Demo.NetCore.Impls
{
    /// <summary>
    /// json序列化\反序列化（基于 <see cref="JsonConvert"/> ）
    /// </summary>
    public class NewJsonSerializer : INewJsonSerializer
    {
        public static NewJsonSerializer Instance { get; } = new NewJsonSerializer();

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public string Serialize(object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
