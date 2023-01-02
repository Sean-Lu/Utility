using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;

namespace Sean.Utility.Serialize
{
    /// <summary>
    /// json序列化\反序列化（基于 <see cref="DataContractJsonSerializer"/> ）
    /// <para>支持：</para>
    /// <para>- Serializes DataSets and DataTables【数据集\数据表】</para>
    /// <para>- Serializes anonymous types【匿名类型】</para>
    /// <para>- Serializes .NET 4.0 dynamic objects【动态类型】</para>
    /// <para>更多信息：[Json.NET vs .NET Serializers](https://www.newtonsoft.com/json/help/html/jsonnetvsdotnetserializers.htm)</para>
    /// </summary>
    public class JsonSerializer : IJsonSerializer
    {
        public JsonSerializer()
        {
#if NETSTANDARD || NET45_OR_GREATER
            Settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:sszzz"),
                UseSimpleDictionaryFormat = true
            };
#if !NETSTANDARD
            //Settings.DataContractSurrogate = new xxxSurrogate();
#endif
#endif
            DefaultEncoding = Encoding.UTF8;
        }

        public static JsonSerializer Instance { get; } = new JsonSerializer();

#if NETSTANDARD || NET45_OR_GREATER
        public DataContractJsonSerializerSettings Settings { get; set; }
#endif
        /// <summary>
        /// 默认编码格式：<see cref="Encoding.UTF8"/>
        /// </summary>
        public Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (typeof(T) == typeof(DataTable))
            {
                var data = (obj as DataTable).ToDictionary();
                return SerializeObject(data);
            }
            else if (typeof(T) == typeof(DataSet))
            {
                var data = (obj as DataSet).ToDictionary();
                return SerializeObject(data);
            }
            else if (typeof(T).IsAnonymousType())// 匿名类型
            {
                var dic = new Dictionary<string, object>();
                foreach (var property in typeof(T).GetProperties())
                {
                    dic.Add(property.Name, property.GetValue(obj, null));
                }
                return SerializeObject(dic);
            }
            else if (typeof(T).GetCustomAttributes(typeof(SerializableAttribute), false).Any())// 解决 k__BackingField 的问题：正则查找替换
            {
                var json = SerializeObject(obj);
                var startStr = "\"<";
                var endStr = ">k__BackingField\":";
                var pattern = $"(?<=({startStr}))(.*?)(?=({endStr}))";
                if (Regex.IsMatch(json, pattern))
                {
                    var matches = Regex.Matches(json, pattern);
                    var list = new List<string>();
                    foreach (Match match in matches)
                    {
                        var value = match.Value;
                        if (!list.Contains(value))
                        {
                            list.Add(value);
                        }
                    }

                    list.ForEach(c =>
                    {
                        json = json.Replace($"{startStr}{c}{endStr}", $"\"{c}\":");
                    });
                    return json;
                }
                return json;
            }

            return SerializeObject(obj);
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            if (typeof(T) == typeof(DataTable))
            {
                var data = DeserializeObject<List<Dictionary<string, object>>>(json).ToDataTable();
                return (T)Convert.ChangeType(data, typeof(T));
            }
            else if (typeof(T) == typeof(DataSet))
            {
                var data = DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(json).ToDataSet();
                return (T)Convert.ChangeType(data, typeof(T));
            }
            else if (typeof(T) == typeof(object))// dynamic动态类型
            {
                if (json.StartsWith("["))
                {
                    var data = DeserializeObject<List<ExpandoObject>>(json);
                    return (T)Convert.ChangeType(data, typeof(List<ExpandoObject>));
                }
                else
                {
                    var data = DeserializeObject<ExpandoObject>(json);
                    return (T)Convert.ChangeType(data, typeof(ExpandoObject));
                }
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                var types = typeof(T).GetGenericArguments();
                if (types.Length == 1 && types[0] == typeof(object))// dynamic动态类型（泛型）
                {
                    var data = DeserializeObject<List<ExpandoObject>>(json);
                    var result = data.Select(c => Convert.ChangeType(c, typeof(ExpandoObject))).ToList();
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }

            return DeserializeObject<T>(json);
        }

        private string SerializeObject<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
#if NETSTANDARD || NET45_OR_GREATER
                var serializer = new DataContractJsonSerializer(typeof(T), Settings);
#else
                var serializer = new DataContractJsonSerializer(typeof(T));
#endif
                serializer.WriteObject(ms, obj);
                return DefaultEncoding.GetString(ms.ToArray());
            }
        }
        private T DeserializeObject<T>(string json)
        {
            using (var stream = new MemoryStream(DefaultEncoding.GetBytes(json)))
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
