#if !NETSTANDARD
using System.Web;

namespace Sean.Utility.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRemoteIpAddress(this HttpContext httpContext)
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
    }
}
#endif