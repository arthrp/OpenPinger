using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger.Dtos
{
    public class EndpointInfo
    {
        public string Host { get; set; }

        public int PollIntervalMilliseconds { get; set; }
    }
}
