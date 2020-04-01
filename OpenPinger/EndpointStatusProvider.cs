using OpenPinger.Dtos;
using OpenPinger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger
{
    public class EndpointStatusProvider
    {
        private volatile List<EndpointStatus> _statuses;

        public EndpointStatusProvider()
        {
            _statuses = new List<EndpointStatus>()
            {
                new EndpointStatus() { Host = "http://www.example.com", Result = EndpointCheckResult.OK }
            };
        }

        public IEnumerable<EndpointStatus> GetCurrentState()
        {
            return _statuses;
        }

        public IEnumerable<EndpointStatus> AddStatus(EndpointStatus status)
        {
            _statuses.Add(status);

            return _statuses;
        }
    }
}
