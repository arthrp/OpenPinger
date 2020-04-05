using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger.Dtos
{
    public class EndpointStatus : EndpointInfo
    {
        public EndpointResponse Response { get; set; }
    }

    public enum EndpointCheckResult
    {
        OK, Fail
    }
}
