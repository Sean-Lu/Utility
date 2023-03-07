using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sean.Utility.Net.Http
{
    /// <summary>
    /// <see cref="HttpClient"/>
    /// </summary>
    public class HttpClientWrapper
    {
        /// <summary>
        /// <see cref="HttpMethod.Post"/>，默认值：application/json
        /// </summary>
        public string DefaultMediaType { get; set; } = "application/json";
        /// <summary>
        /// 获取或设置请求超时前等待的时间间隔，默认值：20秒
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(20);
        /// <summary>
        /// 伪造http_referer
        /// </summary>
        public string Referrer { get; set; }
        /// <summary>
        /// <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/>
        /// </summary>
        public bool EnsureSuccessStatusCode { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(string cookie)
        {
            if (Headers == null)
            {
                Headers = new Dictionary<string, string>();
            }

            var key = "Cookie";
            if (Headers.ContainsKey(key))
            {
                Headers[key] = cookie;
            }
            else
            {
                Headers.Add(key, cookie);
            }
        }

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="httpMethod">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParameters">请求参数</param>
        /// <param name="handleResponse">自定义处理HTTP响应结果</param>
        /// <returns></returns>
        public string Request(HttpMethod httpMethod, string url, IDictionary<string, object> requestParameters, Action<HttpResponseMessageWrapper> handleResponse = null)
        {
            return RequestAsync(httpMethod, url, requestParameters, handleResponse).Result;
        }

        public T Request<T>(HttpMethod httpMethod, string url, IDictionary<string, object> requestParameters, Action<HttpResponseMessageWrapper> handleResponse = null)
        {
            return RequestAsync<T>(httpMethod, url, requestParameters, handleResponse).Result;
        }

        /// <summary>
        /// HTTP请求（异步）
        /// </summary>
        /// <param name="httpMethod">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="requestParameters">请求参数</param>
        /// <param name="handleResponse">自定义处理HTTP响应结果</param>
        /// <returns></returns>
        public async Task<string> RequestAsync(HttpMethod httpMethod, string url, IDictionary<string, object> requestParameters, Action<HttpResponseMessageWrapper> handleResponse = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

            var message = new HttpRequestMessage(httpMethod, url);
            if (httpMethod == HttpMethod.Get)
            {
                if (requestParameters != null && requestParameters.Any())
                {
                    message.RequestUri = new Uri(GetFullUrl(url, requestParameters));
                }
            }
            else
            {
                message.Content = new StringContent(requestParameters != null ? JsonConvert.SerializeObject(requestParameters) : string.Empty);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            }

            using (var handler = new HttpClientHandler())
            {
                if (Headers.ContainsKey("Cookie"))
                {
                    handler.UseCookies = false; //这里为false表示不采用HttpClient的默认Cookie,而是采用httpRequestmessage的Cookie
                }

                AddRequestHeaderInfo(message, Headers);

                if (!string.IsNullOrWhiteSpace(Referrer))
                {
                    message.Headers.Referrer = new Uri(Referrer);
                }

                using (var httpClient = new HttpClient(handler) { Timeout = DefaultTimeout })
                {
                    using (var response = await httpClient.SendAsync(message))
                    {
                        if (handleResponse != null)
                        {
                            var httpResponseMessageWrapper = new HttpResponseMessageWrapper(response);
                            handleResponse(httpResponseMessageWrapper);
                            if (httpResponseMessageWrapper.IsHandled)
                            {
                                return null;
                            }
                        }

                        if (response != null)
                        {
                            if (EnsureSuccessStatusCode)
                            {
                                response.EnsureSuccessStatusCode();
                            }

                            //if (response.IsSuccessStatusCode)
                            {
                                return await response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }

            return null;
        }

        public async Task<T> RequestAsync<T>(HttpMethod httpMethod, string url, IDictionary<string, object> requestParameters, Action<HttpResponseMessageWrapper> handleResponse = null)
        {
            T result = default;
            await RequestAsync(httpMethod, url, requestParameters, wrapper =>
            {
                var response = wrapper.Response;
                if (response != null)
                {
                    var requestResult = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<T>(requestResult);

                    wrapper.IsHandled = true;
                }
            });
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">文件的url路径</param>
        /// <param name="saveDir">文件的保存目录</param>
        /// <param name="fileName">保存的文件名称（如果为空则使用默认文件名称）</param>
        /// <returns></returns>
        public void DownloadFile(string url, string saveDir, string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Path.GetFileName(url);
            }
            var filePath = Path.Combine(saveDir, fileName);

            #region 方式1：WebClient
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, filePath);
            }
            #endregion

            #region 方式2：HttpWebRequest
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
        /// 添加请求头信息
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="headerDic">请求头信息</param>
        private void AddRequestHeaderInfo(HttpClient httpClient, IDictionary<string, string> headerDic)
        {
            var httpHeaders = httpClient.DefaultRequestHeaders;
            AddRequestHeaderInfo(httpHeaders, headerDic);
        }
        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="headerDic">请求头信息</param>
        private void AddRequestHeaderInfo(HttpRequestMessage message, IDictionary<string, string> headerDic)
        {
            var httpHeaders = message.Headers;
            AddRequestHeaderInfo(httpHeaders, headerDic);
        }
        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <param name="headerDic"></param>
        private void AddRequestHeaderInfo(HttpRequestHeaders httpHeaders, IDictionary<string, string> headerDic)
        {
            if (httpHeaders == null) throw new ArgumentNullException(nameof(httpHeaders));

            if (headerDic != null && headerDic.Any())
            {
                foreach (var item in headerDic)
                {
                    var name = item.Key;
                    var value = item.Value;
                    //if (httpHeaders.Contains(name))
                    //{
                    //    httpHeaders.Remove(name);
                    //}
                    httpHeaders.Add(name, value);
                }
            }
        }

        /// <summary>
        /// 获取完整url链接地址（Get请求）
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="requestParameters">请求参数</param>
        /// <returns>返回完整的url链接地址</returns>
        private string GetFullUrl(string url, IDictionary<string, object> requestParameters)
        {
            var result = url;
            if (requestParameters != null && requestParameters.Any())
            {
                var getPara = GetUrlParams(requestParameters);
                if (!string.IsNullOrWhiteSpace(getPara))
                {
                    result = $"{url.TrimEnd('/')}{(url.IndexOf("?") != -1 ? "&" : "?")}{getPara}";
                }
            }
            return result;
        }

        private string GetUrlParams(IDictionary<string, object> requestParameters)
        {
            if (requestParameters != null && requestParameters.Any())
            {
                var list = new List<string>();

                #region 解析
                foreach (var keyValuePair in requestParameters)
                {
                    var key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (value == null)
                    {
                        continue;
                    }

                    if (value.GetType().IsValueType)
                    {
                        list.Add($"{key}={value}");
                    }
                    else if (value is string str)
                    {
                        list.Add($"{key}={Uri.EscapeDataString(str)}");
                    }
                    else
                    {
                        list.Add($"{key}={Uri.EscapeDataString(value.ToString())}");
                    }
                }
                #endregion

                if (list.Any())
                {
                    return string.Join("&", list);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 是否是合法的url
        /// </summary>
        /// <param name="url">待校验url地址</param>
        /// <param name="uriKind"></param>
        /// <returns></returns>
        private bool IsValidUrl(string url, UriKind uriKind = UriKind.Absolute)
        {
            return Uri.TryCreate(url, uriKind, out _);
        }
        #endregion
    }
}
