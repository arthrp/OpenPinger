using OpenPinger.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger
{
    public class EndpointStatusDbSingleton
    {
        public EndpointStatusDbSingleton()
        {
            Statuses = new ConcurrentDictionary<string, EndpointResponse>();
        }

        /// <summary>
        /// (url, response)
        /// </summary>
        public ConcurrentDictionary<string, EndpointResponse> Statuses { get; private set; }
    }
}
