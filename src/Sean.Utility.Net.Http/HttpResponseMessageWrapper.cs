using System.Net.Http;

namespace Sean.Utility.Net.Http
{
    public class HttpResponseMessageWrapper
    {
        public HttpResponseMessage Response => _response;

        /// <summary>
        /// <see cref="HttpResponseMessage"/> 是否已经被处理
        /// </summary>
        public bool IsHandled { get; set; }

        private HttpResponseMessage _response;

        public HttpResponseMessageWrapper(HttpResponseMessage response)
        {
            _response = response;
        }
    }
}
