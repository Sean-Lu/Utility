#if !NETSTANDARD
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Sean.Utility.Web
{
    /// <summary>
    /// Cache缓存操作
    /// </summary>
    public class CacheHelper
    {
        private CacheHelper() { }

        /// <summary>
        /// 获取已存在的所有数据缓存key
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetAllKeys()
        {
            ArrayList list = new ArrayList();
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                list.Add(cacheEnum.Key.ToString());
            }
            return list;
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[key];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCache(string key, object value)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期时间(绝对过期)。单位为秒</param>
        public static void SetCache(string key, object value, int timeout)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, DateTime.Now.AddSeconds(timeout), TimeSpan.Zero, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 设置数据缓存  
        /// </summary>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <param name="timeout">过期时间(相对过期)</param>  
        public static void SetCache(string key, object value, TimeSpan timeout)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
        }
        /// <summary>
        /// 设置数据缓存。如SetCache("mydata", list, DateTime.Now.AddSeconds(30), TimeSpan.Zero)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteExpiration">所插入对象将过期并被从缓存中移除的时间。若要避免可能出现的本地时间方面的问题（如从标准时间更改为夏时制），请对此参数值使用 UtcNow，不要使用Now。如果使用绝对过期，则slidingExpiration 参数必须为 NoSlidingExpiration。</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象过期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20分钟之后将过期并被从缓存中移除。如果使用可调过期，则 absoluteExpiration 参数必须为NoAbsoluteExpiration。</param>
        public static void SetCache(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存。
        /// 返回结果:从 Cache 移除的项。如果未找到键参数中的值，则返回 null。
        /// </summary>
        /// <param name="key">键</param>
        public static object RemoveCache(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache.Remove(key);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}
#endif