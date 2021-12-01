using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Sean.Utility.Enums;
using Sean.Utility.Format;
using Sean.Utility.Serialize;

namespace Sean.Utility.Net.Http
{
    /// <summary>
    /// Http Client
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 默认值：application/json
        /// </summary>
        public static string DefaultMediaType { get; set; }
        /// <summary>
        /// 获取或设置请求超时前等待的时间间隔
        /// </summary>
        public static TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        private static Dictionary<string, string> _defaultHeaders;

        private static Dictionary<HttpRequestType, HttpMethod> _requestTypeMapDic;

        private HttpHelper() { }

        static HttpHelper()
        {
            ResetDefaultSettings();

            _requestTypeMapDic = new Dictionary<HttpRequestType, HttpMethod>
            {
                {HttpRequestType.Get, HttpMethod.Get},
                {HttpRequestType.Post, HttpMethod.Post},
                {HttpRequestType.Put, HttpMethod.Put},
                {HttpRequestType.Delete, HttpMethod.Delete},
            };
        }

        /// <summary>
        /// 初始化，重置默认参数
        /// </summary>
        public static void ResetDefaultSettings()
        {
            if (_defaultHeaders == null)
            {
                _defaultHeaders = new Dictionary<string, string>();
            }
            else
            {
                _defaultHeaders.Clear();
            }
            DefaultMediaType = "application/json";
            DefaultTimeout = TimeSpan.FromSeconds(20);
        }

        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddDefaultHeader(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (_defaultHeaders == null)
            {
                _defaultHeaders = new Dictionary<string, string>();
            }
            if (_defaultHeaders.ContainsKey(name))
            {
                _defaultHeaders.Remove(name);
            }
            _defaultHeaders.Add(name, value);
        }
        /// <summary>
        /// 清除请求头信息
        /// </summary>
        public static void ClearDefaultHeader()
        {
            _defaultHeaders?.Clear();
        }

        #region Get
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        public static string Get(string url, string requestParas = null, Action<string> resultCallback = null)
        {
            var result = Request(HttpRequestType.Get, url, requestParas);
            resultCallback?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        public static string Get(string url, IDictionary<string, string> requestParasDic, Action<string> resultCallback = null)
        {
            return Get(url, JsonConvert.SerializeObject(requestParasDic), resultCallback);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="type">返回数据类型，支持：json、xml</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        /// <typeparam name="T"></typeparam>
        public static T Get<T>(string url, string requestParas = null, HttpRequestResultType type = HttpRequestResultType.Json, Action<T> resultCallback = null) where T : class, new()
        {
            var result = Request<T>(HttpRequestType.Get, url, requestParas, httpRequestResultType: type);
            resultCallback?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数</param>
        /// <param name="type">返回数据类型，支持：json、xml</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        /// <typeparam name="T"></typeparam>
        public static T Get<T>(string url, IDictionary<string, string> requestParasDic, HttpRequestResultType type = HttpRequestResultType.Json, Action<T> resultCallback = null) where T : class, new()
        {
            return Get(url, JsonConvert.SerializeObject(requestParasDic), type, resultCallback);
        }
        #endregion

        #region Post
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数（json格式）</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        public static string Post(string url, string postData = null, Action<string> resultCallback = null)
        {
            var result = Request(HttpRequestType.Post, url, postData);
            resultCallback?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postDataDic">请求参数</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        public static string Post(string url, IDictionary<string, string> postDataDic, Action<string> resultCallback = null)
        {
            return Post(url, JsonConvert.SerializeObject(postDataDic), resultCallback);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数（json格式）</param>
        /// <param name="type">返回数据类型，支持：json、xml</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        /// <returns></returns>
        public static T Post<T>(string url, string postData = null, HttpRequestResultType type = HttpRequestResultType.Json, Action<T> resultCallback = null) where T : class, new()
        {
            var result = Request<T>(HttpRequestType.Post, url, postData, httpRequestResultType: type);
            resultCallback?.Invoke(result);
            return result;
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postDataDic">请求参数</param>
        /// <param name="type">返回数据类型，支持：json、xml</param>
        /// <param name="resultCallback">处理返回结果的委托</param>
        /// <returns></returns>
        public static T Post<T>(string url, IDictionary<string, string> postDataDic, HttpRequestResultType type = HttpRequestResultType.Json, Action<T> resultCallback = null) where T : class, new()
        {
            return Post<T>(url, JsonConvert.SerializeObject(postDataDic), type, resultCallback);
        }
        #endregion

        #region Request
        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <returns></returns>
        public static string Request(HttpRequestType type, string url, string requestParas = null, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true)
        {
            return RequestAsync(type, url, requestParas, timeout, headerDic, cookie, referrer, ensureSuccess).Result;
        }
        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数（json格式）</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <returns></returns>
        public static string Request(HttpRequestType type, string url, IDictionary<string, string> requestParasDic, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true)
        {
            return Request(type, url, JsonConvert.SerializeObject(requestParasDic), timeout, headerDic, cookie, referrer, ensureSuccess);
        }

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <param name="httpRequestResultType"></param>
        /// <returns></returns>
        public static T Request<T>(HttpRequestType type, string url, string requestParas = null, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true, HttpRequestResultType httpRequestResultType = HttpRequestResultType.Json) where T : class, new()
        {
            return RequestAsync<T>(type, url, requestParas, timeout, headerDic, cookie, referrer, ensureSuccess, httpRequestResultType).Result;
        }
        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <param name="httpRequestResultType"></param>
        /// <returns></returns>
        public static T Request<T>(HttpRequestType type, string url, IDictionary<string, string> requestParasDic, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true, HttpRequestResultType httpRequestResultType = HttpRequestResultType.Json) where T : class, new()
        {
            return Request<T>(type, url, JsonConvert.SerializeObject(requestParasDic), timeout, headerDic, cookie, referrer, ensureSuccess, httpRequestResultType);
        }

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onComplete"></param>
        public static void Request(WebRequest request, Action<HttpStatusCode, string> onComplete = null)
        {
            RequestAsync(request, onComplete).Wait();
        }
        #endregion

        #region Request（异步）
        /// <summary>
        /// HTTP请求（异步）
        /// </summary>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <returns></returns>
        public static async Task<string> RequestAsync(HttpRequestType type, string url, string requestParas = null, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

            CheckUrl(url);

            var message = new HttpRequestMessage(GetHttpMethod(type), url);
            if (type == HttpRequestType.Get)
            {
                if (!string.IsNullOrWhiteSpace(requestParas))
                {
                    message.RequestUri = new Uri(GetFullUrl(url, requestParas));
                }
            }
            else
            {
                message.Content = new StringContent(requestParas ?? string.Empty);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            }

            AddRequestHeaderInfo(ref message, headerDic, true);

            using (var handler = new HttpClientHandler())
            {
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    handler.UseCookies = false; //这里为false表示不采用HttpClient的默认Cookie,而是采用httpRequestmessage的Cookie

                    message.Headers.Add("Cookie", cookie); //利用HttpRequestMessage设置cookie
                }

                if (!string.IsNullOrWhiteSpace(referrer))
                {
                    message.Headers.Referrer = new Uri(referrer);
                }

                using (var httpClient = new HttpClient(handler) { Timeout = timeout ?? DefaultTimeout })
                {
                    using (var response = await httpClient.SendAsync(message))
                    {
                        if (response != null)
                        {
                            if (ensureSuccess)
                            {
                                response.EnsureSuccessStatusCode();
                            }

                            if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// HTTP请求（异步）
        /// </summary>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <returns></returns>
        public static async Task<string> RequestAsync(HttpRequestType type, string url, IDictionary<string, string> requestParasDic, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true)
        {
            return await RequestAsync(type, url, JsonConvert.SerializeObject(requestParasDic), timeout, headerDic, cookie, referrer, ensureSuccess);
        }

        /// <summary>
        /// HTTP请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParas">请求参数（json格式）</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <param name="httpRequestResultType"></param>
        /// <returns></returns>
        public static async Task<T> RequestAsync<T>(HttpRequestType type, string url, string requestParas = null, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true, HttpRequestResultType httpRequestResultType = HttpRequestResultType.Json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

            var requestResult = await RequestAsync(HttpRequestType.Post, url, requestParas, timeout, headerDic, cookie, referrer, ensureSuccess);
            if (string.IsNullOrWhiteSpace(requestResult))
            {
                return default(T);
            }

            T result;
            switch (httpRequestResultType)
            {
                case HttpRequestResultType.Json:
                    result = JsonConvert.DeserializeObject<T>(requestResult);
                    break;
                case HttpRequestResultType.Xml:
                    result = XmlSerializer.Instance.Deserialize<T>(requestResult);
                    break;
                default:
                    throw new Exception($"Unsupported HttpRequestResultType: {httpRequestResultType.ToString()}");
            }
            return result;
        }
        /// <summary>
        /// HTTP请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParasDic">请求参数</param>
        /// <param name="timeout">请求超时时间</param>
        /// <param name="headerDic">请求头</param>
        /// <param name="cookie">请求时携带的cookie</param>
        /// <param name="referrer">伪造http_referer</param>
        /// <param name="ensureSuccess">如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，是否引发异常。</param>
        /// <param name="httpRequestResultType"></param>
        /// <returns></returns>
        public static async Task<T> RequestAsync<T>(HttpRequestType type, string url, IDictionary<string, string> requestParasDic, TimeSpan? timeout = null, IDictionary<string, string> headerDic = null, string cookie = null, string referrer = null, bool ensureSuccess = true, HttpRequestResultType httpRequestResultType = HttpRequestResultType.Json) where T : class, new()
        {
            return await RequestAsync<T>(type, url, JsonConvert.SerializeObject(requestParasDic), timeout, headerDic, cookie, referrer, ensureSuccess, httpRequestResultType);
        }

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onComplete"></param>
        public static async Task RequestAsync(WebRequest request, Action<HttpStatusCode, string> onComplete = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            WebResponse response;

            try
            {
                response = await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }

            if (response == null)
            {
                // 请求返回为空
                onComplete?.Invoke(HttpStatusCode.NotFound, null);
                return;
            }

            var result = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = await reader.ReadToEndAsync();
                    }
                    responseStream.Close();
                }
            }

            onComplete?.Invoke(((HttpWebResponse)response).StatusCode, result);
        }
        #endregion

        #region 参数转换
        /// <summary>
        /// json转get参数
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonToGetPara(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return json;
            }

            var jsonMini = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(json));// 压缩json
            if (string.IsNullOrWhiteSpace(jsonMini))
            {
                throw new ArgumentException("参数格式错误", nameof(json));
            }

            var paras = jsonMini.Trim().Trim('{', '}').Split(new[] { ",\"" }, StringSplitOptions.RemoveEmptyEntries);
            if (paras.Length < 1)
            {
                return string.Empty;
            }

            var list = new List<string>();
            //for (var i = paras.Length - 1; i >= 0; i--)
            for (var i = 0; i < paras.Length; i++)
            {
                if (paras[i].IndexOf("\":null") != -1)
                {
                    //Array.Clear(paras, i, 1);
                    continue;
                }

                var para = paras[i].Split(new[] { "\":" }, StringSplitOptions.RemoveEmptyEntries);
                if (para.Length == 2)
                {
                    var key = para[0].Trim('"');
                    var value = para[1].Trim('"');
                    list.Add($"{key}={HttpUtility.UrlEncode(value)}");
                }
            }

            return string.Join("&", list);
        }
        /// <summary>
        /// get参数转json
        /// </summary>
        /// <param name="getPara"></param>
        /// <returns></returns>
        public static string GetParaToJson(string getPara)
        {
            if (string.IsNullOrWhiteSpace(getPara))
            {
                return getPara;
            }
            var paras = getPara.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var para in paras)
            {
                if (para.IndexOf("=") > 0)
                {
                    var key = para.Substring(0, para.IndexOf("="));
                    var value = para.Substring(para.IndexOf("=") + 1);
                    sb.Append($"\"{key}\":{(value == "null" ? "null" : $"\"{HttpUtility.UrlDecode(value)}\"")},");
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
        #endregion

#if !NETSTANDARD
        /// <summary>
        /// 获取访问者的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIpAddress()
        {
            var clientIp = string.Empty;
            if (HttpContext.Current != null)
            {
                clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIp) || clientIp.ToLower() == "unknown")
                {
                    clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(clientIp))
                    {
                        clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    clientIp = clientIp.Split(',')[0];
                }
            }

            return clientIp;
        }
#endif

        /// <summary>
        /// 校验url地址是否规范
        /// </summary>
        /// <param name="url">待校验url地址</param>
        /// <returns></returns>
        public static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        /// <summary>
        /// 获取完整url链接地址（Get请求）
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="requestParas">请求参数</param>
        /// <param name="json">请求参数是否为json格式</param>
        /// <returns>返回完整的url链接地址</returns>
        public static string GetFullUrl(string url, string requestParas, bool json = true)
        {
            var result = url;
            if (!string.IsNullOrWhiteSpace(requestParas))
            {
                var getPara = json ? JsonToGetPara(requestParas) : requestParas;
                if (!string.IsNullOrWhiteSpace(getPara))
                {
                    result = $"{url.TrimEnd('/')}{(url.IndexOf("?") != -1 ? "&" : "?")}{getPara}";
                }
            }
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">文件的url路径</param>
        /// <param name="saveDir">文件的保存目录</param>
        /// <param name="fileName">保存的文件名称（如果为空则使用默认文件名称）</param>
        /// <returns></returns>
        public static void DownloadFile(string url, string saveDir, string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

            CheckUrl(url);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Path.GetFileName(url);
            }
            var filePath = Path.Combine(saveDir, fileName);

            #region 方法1：WebClient
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, filePath);
            }
            #endregion

            #region 方法2：HttpWebRequest
            //HttpWebRequest httpWebRequest = null;
            //try
            //{
            //    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            //    {
            //        using (var sr = httpWebResponse.GetResponseStream())
            //        {
            //            using (var sw = new FileStream(filePath, FileMode.Create))
            //            {
            //                var totalLength = 0;
            //                var buffer = new byte[1024];
            //                if (sr != null)
            //                {
            //                    var readLength = sr.Read(buffer, 0, buffer.Length);
            //                    while (readLength > 0)
            //                    {
            //                        totalLength += readLength;
            //                        sw.Write(buffer, 0, readLength);
            //                        readLength = sr.Read(buffer, 0, buffer.Length);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //finally
            //{
            //    httpWebRequest?.Abort();
            //}
            #endregion
        }

        #region Private Mathods
        /// <summary>
        /// 获取HTTP请求方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static HttpMethod GetHttpMethod(HttpRequestType type)
        {
            if (!_requestTypeMapDic.ContainsKey(type))
            {
                throw new Exception($"Unsupported HttpRequestType: {type.ToString()}");
            }

            return _requestTypeMapDic[type];
        }

        /// <summary>
        /// 校验URL链接
        /// </summary>
        /// <param name="url"></param>
        private static void CheckUrl(string url)
        {
            if (!IsUrlValid(url))
            {
                throw new ArgumentException("url地址不规范", nameof(url));
            }

            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        }

        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="headerDic">请求头信息</param>
        /// <param name="addDefaultHeaders">是否添加默认的请求头信息</param>
        private static void AddRequestHeaderInfo(HttpClient httpClient, IDictionary<string, string> headerDic = null, bool addDefaultHeaders = true)
        {
            if (httpClient == null)
            {
                return;
            }

            var httpHeaders = httpClient.DefaultRequestHeaders;
            AddHeaders(ref httpHeaders, headerDic, addDefaultHeaders);
        }
        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="headerDic">请求头信息</param>
        /// <param name="addDefaultHeaders">是否添加默认的请求头信息</param>
        private static void AddRequestHeaderInfo(ref HttpRequestMessage message, IDictionary<string, string> headerDic = null, bool addDefaultHeaders = true)
        {
            if (message == null)
            {
                return;
            }

            var httpHeaders = message.Headers;
            AddHeaders(ref httpHeaders, headerDic, addDefaultHeaders);
        }
        private static void AddHeaders(ref HttpRequestHeaders httpHeaders, IDictionary<string, string> headerDic, bool addDefaultHeaders = true)
        {
            if (httpHeaders == null)
            {
                return;
            }

            var dic = new Dictionary<string, string>();
            if (addDefaultHeaders && _defaultHeaders != null && _defaultHeaders.Count > 0)
            {
                foreach (var defaultHeader in _defaultHeaders)
                {
                    dic.Add(defaultHeader.Key, defaultHeader.Value);
                }
            }
            if (headerDic != null && headerDic.Count > 0)
            {
                foreach (var header in headerDic)
                {
                    var name = header.Key;
                    var value = header.Value;
                    if (dic.ContainsKey(name))
                    {
                        dic.Remove(name);
                    }
                    dic.Add(name, value);
                }
            }

            if (dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    var name = item.Key;
                    var value = item.Value;
                    if (httpHeaders.Contains(name))
                    {
                        httpHeaders.Remove(name);
                    }
                    httpHeaders.Add(name, value);
                }
            }
        }
        #endregion
    }
}
