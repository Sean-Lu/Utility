using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Demo.NetCore.Helper
{
    /// <summary>
    /// json数据转换（基于 <see cref="JsonConvert"/> ）
    /// </summary>
    public class JsonHelper
    {
        #region Json序列化
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Serialize(object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Json序列化（忽略null值）
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string SerializeIgnoreNullValue(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// Json序列化（自定义DateTime序列化格式化）
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <param name="dateFormatString"></param>
        /// <returns></returns>
        public static string SerializeFormatDateTime(object obj, string dateFormatString = "yyyy-MM-dd HH:mm:ss")
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { DateFormatString = dateFormatString });
        }

        public static string SerializeFormatIndented(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
        #endregion

        #region Json反序列化
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
        #endregion

        /// <summary>
        /// Json压缩
        /// </summary>
        /// <param name="json">待压缩的json字符串</param>
        /// <param name="result">压缩后的json字符串，如果执行失败则返回压缩前的原始json字符串</param>
        /// <returns>是否执行成功</returns>
        public static bool JsonMinify(string json, out string result)
        {
            result = json;
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                result = Serialize(Deserialize<object>(json));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
