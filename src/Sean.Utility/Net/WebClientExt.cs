using System;
using System.Net;

namespace Sean.Utility.Net
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
        /// 默认设置<see cref="Timeout"/>为20秒
        /// </summary>
        public WebClientExt()
        {
            this.Timeout = 20000;
        }

        /// <summary>
        /// 自定义超时时间
        /// </summary>
        /// <param name="timeout">超时时间（单位：ms）</param>
        public WebClientExt(int timeout)
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
