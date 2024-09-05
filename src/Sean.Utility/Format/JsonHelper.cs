using Sean.Utility.Contracts;
using Sean.Utility.Serialize;

namespace Sean.Utility.Format;

/// <summary>
/// json序列化\反序列化
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 获取或设置 <see cref="IJsonSerializer"/>
    /// </summary>
    public static IJsonSerializer Serializer { get; set; } = new JsonSerializer();

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