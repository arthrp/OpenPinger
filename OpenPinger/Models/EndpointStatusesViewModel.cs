using OpenPinger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPinger.Models
{
    public class EndpointStatusesViewModel
    {
        public IEnumerable<EndpointStatus> Statuses { get; set; }
    }

    //public class EndpointStatus
    //{
    //    public string Host { get; set; }
    //    public EndpointCheckResult Result { get; set; }
    //}


}
