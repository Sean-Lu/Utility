#if !NETSTANDARD
using System;
using System.Web;

namespace Sean.Utility.Web
{
    /// <summary>  
    /// Cookie缓存操作
    /// </summary>  
    public class CookieHelper
    {
        private CookieHelper() { }

        /// <summary>
        /// Cookie是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsExists(string name)
        {
            return GetCookie(name) != null;
        }

        /// <summary>
        /// 获取 Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }
        /// <summary>
        /// 获取 Cookie 的值
        /// </summary>
        /// <param name="name">名</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            return GetCookie(name)?.Value;
        }

        /// <summary>
        /// 添加一个 Cookie（24小时后过期）
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        public static void Add(string name, string value)
        {
            Add(name, value, DateTime.Now.AddDays(1));
        }
        /// <summary>
        /// 添加一个 Cookie
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期日期和时间</param>
        /// <param name="domain">此 Cookie 与其关联的域</param>
        /// <param name="httpOnly">true代表客户端只能读，不能写。只有服务端可写，防止被篡改</param>
        public static void Add(string name, string value, DateTime? expires, string domain = null, bool httpOnly = true)
        {
            var cookie = GetCookie(name) ?? new HttpCookie(name);
            cookie.Value = value;
            if (expires.HasValue)
            {
                cookie.Expires = expires.Value;
            }
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            cookie.HttpOnly = httpOnly;

            Add(cookie);
        }
        /// <summary>
        /// 添加一个 Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void Add(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 更新 Cookie 集合中现有 Cookie 的值。（24小时后过期）
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        public static void Set(string name, string value)
        {
            Set(name, value, DateTime.Now.AddDays(1));
        }
        /// <summary>
        /// 更新 Cookie 集合中现有 Cookie 的值。
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期日期和时间</param>
        /// <param name="domain">此 Cookie 与其关联的域</param>
        /// <param name="httpOnly">true代表客户端只能读，不能写。只有服务端可写，防止被篡改</param>
        public static void Set(string name, string value, DateTime? expires, string domain = null, bool httpOnly = true)
        {
            var cookie = GetCookie(name) ?? new HttpCookie(name);
            cookie.Value = value;
            if (expires.HasValue)
            {
                cookie.Expires = expires.Value;
            }
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            cookie.HttpOnly = httpOnly;

            Set(cookie);
        }
        /// <summary>
        /// 更新 Cookie 集合中现有 Cookie 的值。
        /// </summary>
        /// <param name="cookie"></param>
        public static void Set(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 清除 Cookie 集合中的所有 Cookie。
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }

        /// <summary>
        /// 从集合中移除具有指定名称的 Cookie。
        /// </summary>
        /// <param name="name">名</param>
        public static void Remove(string name)
        {
            if (!IsExists(name))
            {
                return;
            }

            HttpContext.Current.Response.Cookies.Remove(name);
        }
        /// <summary>  
        /// 清除指定域名下所有Cookie  
        /// </summary>  
        /// <param name="domain">域名</param>  
        public static void RemoveByDomain(string domain)
        {
            foreach (HttpCookie httpCookie in HttpContext.Current.Request.Cookies)
            {
                if (httpCookie.Domain == domain)
                {
                    Remove(httpCookie.Name);
                }
            }
        }
    }
}
#endif