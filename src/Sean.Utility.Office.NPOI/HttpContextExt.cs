#if NETSTANDARD
using Microsoft.AspNetCore.Http;

namespace Sean.Utility.Office.NPOI
{
    /// <summary>
    /// HttpContext扩展
    /// </summary>
    public class HttpContextExt
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 当前HTTP上下文
        /// </summary>
        public static HttpContext Current => _httpContextAccessor?.HttpContext;

        internal static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
#endif