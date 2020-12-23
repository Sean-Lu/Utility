#if !NETSTANDARD
using System.Web;
using System.Web.SessionState;

namespace Sean.Utility.Web
{
    /// <summary>
    /// Session操作
    /// </summary>
    public class SessionHelper
    {
        private SessionHelper() { }

        /// <summary>
        /// 私有静态Session操作对象
        /// </summary>
        private static HttpSessionState _session = HttpContext.Current.Session;

        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name">session名</param>
        /// <returns>session对象</returns>
        public static object GetSession(string name)
        {
            return _session[name];
        }

        /// <summary>
        /// 根据session名获取session数字（默认为0）
        /// </summary>
        /// <param name="name">session名</param>
        /// <returns></returns>
        public static int GetSessionNum(string name)
        {
            int result = 0;
            if (_session[name] != null)
            {
                int.TryParse(_session[name].ToString(), out result);
            }
            return result;
        }

        /// <summary>
        /// 根据session名获取session字符串（默认为""）
        /// </summary>
        /// <param name="name">session名</param>
        /// <returns></returns>
        public static string GetSessionStr(string name)
        {
            string result = "";
            if (_session[name] != null)
            {
                result = _session[name].ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取会话的唯一标识符 SessionID。
        /// </summary>
        /// <returns></returns>
        public static string GetSessionId()
        {
            return _session.SessionID;
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session名</param>
        /// <param name="value">session值</param>
        public static void SetSession(string name, object value)
        {
            _session.Remove(name);
            _session.Add(name, value);
        }

        /// <summary>
        /// 添加session
        /// </summary>
        /// <param name="name">session名</param>
        /// <param name="value">session值</param>
        public static void AddSession(string name, object value)
        {
            // 方法一：
            _session.Add(name, value);

            // 方法二：
            //_session[name] = value;
        }

        /// <summary>
        /// 删除一个指定的session
        /// </summary>
        /// <param name="name">session名</param>
        public static void RemoveSession(string name)
        {
            _session.Remove(name);
        }

        /// <summary>
        /// 清空所有的session
        /// </summary>
        public static void Clear()
        {
            //Session.RemoveAll()通用调用Clear()方法
            _session.Clear();
        }

        /// <summary>
        /// (全局)设置session过期时间
        ///  Timeout属性不能设置为超过 525,600 分钟(1年)的值。 默认值为 20 分钟。 
        ///  <param name="expires">调动有效期(分钟)</param>
        /// <remarks>同时可以在web.config中system.web节点中使用sessionState配置timeout属性</remarks>
        /// </summary>
        public static void SetTimeOut(int expires)
        {
            _session.Timeout = expires;
        }
    }
}
#endif