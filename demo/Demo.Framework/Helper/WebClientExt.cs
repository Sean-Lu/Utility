using System;
using System.Net;

namespace Demo.Framework.Helper
{
    /// <summary>
    /// WebClient扩展，支持设置超时时间
    /// </summary>
    public class WebClientExt : WebClient
    {
        /// <summary>
        /// 超时时间（单位：ms）
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 自定义超时时间
        /// </summary>
        /// <param name="timeout">超时时间（单位：ms）</param>
        public WebClientExt(int timeout = 30000)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            if (result != null)
            {
                result.Timeout = this.Timeout;
            }
            return result;
        }
    }
}
