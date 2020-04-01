using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger.Dtos
{
    public class EndpointStatus
    {
        public string Host { get; set; }
        public EndpointCheckResult Result { get; set; }
    }

    public enum EndpointCheckResult
    {
        OK, Fail
    }
}
