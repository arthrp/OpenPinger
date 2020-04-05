using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger.Dtos
{
    public class EndpointResponse
    {
        public int StatusCode { get; set; }

        public bool IsSuccess { get { return StatusCode == 200; } }

        public string StatusText { get { return IsSuccess ? "OK" : "FAIL"; } }

        public string StatusCssClass { get { return IsSuccess ? "status-ok" : "status-fail"; } }

        public DateTime LastChecked { get; set; }
    }
}
