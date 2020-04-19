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
        public readonly EndpointInfo Info;

        public EndpointWatcher(HttpClient client, EndpointInfo info, IServiceProvider serviceProvider)
        {
            _client = client;
            Info = info;
            _endpointStatusDb = (EndpointStatusDbSingleton)serviceProvider.GetService(typeof(EndpointStatusDbSingleton));

            _timer = new Timer(info.PollIntervalMilliseconds) { AutoReset = true };
            _timer.Elapsed += PollEndpoint;
            _timer.Disposed += TimerDisposed;
            _timer.Enabled = true;
        }

        private void TimerDisposed(object sender, EventArgs e)
        {
            EndpointResponse res = null;
            var hasRemoved = _endpointStatusDb.Statuses.TryRemove(Info.Host, out res);
        }

        public void Terminate()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void PollEndpoint(object sender, ElapsedEventArgs e)
        {
            var r = _client.GetAsync("/").Result;

            var response = new EndpointResponse() { StatusCode = (int)r.StatusCode, LastChecked = DateTime.UtcNow };
            if(!_endpointStatusDb.Statuses.ContainsKey(Info.Host))
            {
                var wasAddedSuccessfully = _endpointStatusDb.Statuses.TryAdd(Info.Host, response);
            }
            else
            {
                _endpointStatusDb.Statuses[Info.Host] = response;
            }
            

            Console.WriteLine($"Polling endpoint - result {r.StatusCode}");
        }
    }
}
