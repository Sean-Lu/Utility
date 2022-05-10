#if !NETSTANDARD
using System.Web;

namespace Sean.Utility.Web.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <param name="httpContext">示例：HttpContext.Current</param>
        /// <returns></returns>
        public static string GetRemoteIpAddress(this HttpContext httpContext)
        {
            var clientIp = string.Empty;
            if (httpContext != null)
            {
                clientIp = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIp) || clientIp.ToLower() == "unknown")
                {
                    clientIp = httpContext.Request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(clientIp))
                    {
                        clientIp = httpContext.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    clientIp = clientIp.Split(',')[0];
                }
            }

            return clientIp;
        }
    }
}
#endif