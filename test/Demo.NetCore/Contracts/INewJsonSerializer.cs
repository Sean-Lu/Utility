using Newtonsoft.Json;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Contracts
{
    public interface INewJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        string Serialize(object obj, JsonSerializerSettings settings);

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        T Deserialize<T>(string json, JsonSerializerSettings settings);
    }
}
