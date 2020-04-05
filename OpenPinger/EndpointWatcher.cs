using OpenPinger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace OpenPinger
{
    public class EndpointWatcher
    {
        private readonly HttpClient _client;
        private readonly Timer _timer;
        //private readonly IServiceProvider _serviceProvider;
        private readonly EndpointStatusDbSingleton _endpointStatusDb;
        private readonly EndpointInfo _info;

        public EndpointWatcher(HttpClient client, EndpointInfo info, IServiceProvider serviceProvider)
        {
            _client = client;
            _info = info;
            _endpointStatusDb = (EndpointStatusDbSingleton)serviceProvider.GetService(typeof(EndpointStatusDbSingleton));

            _timer = new Timer(info.PollIntervalMilliseconds) { AutoReset = true };
            _timer.Elapsed += PollEndpoint;
            _timer.Enabled = true;
        }

        private void PollEndpoint(object sender, ElapsedEventArgs e)
        {
            var r = _client.GetAsync("/").Result;

            var response = new EndpointResponse() { StatusCode = (int)r.StatusCode, LastChecked = DateTime.UtcNow };
            if(!_endpointStatusDb.Statuses.ContainsKey(_info.Host))
            {
                var wasAddedSuccessfully = _endpointStatusDb.Statuses.TryAdd(_info.Host, response);
            }
            else
            {
                _endpointStatusDb.Statuses[_info.Host] = response;
            }
            

            Console.WriteLine($"Polling endpoint - result {r.StatusCode}");
        }
    }
}
