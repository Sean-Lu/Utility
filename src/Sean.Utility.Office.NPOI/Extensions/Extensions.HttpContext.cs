#if NETSTANDARD
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Sean.Utility.Office.NPOI.Extensions
{
    /// <summary>
    /// HttpContext扩展
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// 注入HttpContextAccessor
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// 使用HttpContextExt，需要先注入HttpContextAccessor
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpContextExt(this IApplicationBuilder builder)
        {
            var httpContextAccessor = builder.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContextExt.Configure(httpContextAccessor);
            return builder;
        }
    }
}
#endif