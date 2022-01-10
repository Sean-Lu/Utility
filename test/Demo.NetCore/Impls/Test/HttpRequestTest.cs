using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Sean.Utility.Contracts;
using Sean.Utility.Net.Http;

namespace Demo.NetCore.Impls.Test
{
    public class HttpRequestTest : ISimpleDo
    {
        public void Execute()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// https

            var url = "http://xxxxx";
            var paramsDic = new Dictionary<string, object>();
            var httpClient = new HttpClientWrapper();
            var requestResult = httpClient.Request(HttpMethod.Get, url, paramsDic);
        }
    }
}
