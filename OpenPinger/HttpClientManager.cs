using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenPinger
{
    public class HttpClientManager
    {
        private IHttpClientFactory _clientFactory;

        public HttpClientManager(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
