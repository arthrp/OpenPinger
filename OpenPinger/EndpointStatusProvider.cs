using OpenPinger.Dtos;
using OpenPinger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenPinger
{
    public class EndpointStatusProvider
    {
        private readonly List<EndpointWatcher> _watchers = new List<EndpointWatcher>();
        private IServiceProvider _serviceProvider;
        private readonly EndpointStatusDbSingleton _endpointStatusDb;

        public EndpointStatusProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _endpointStatusDb = (EndpointStatusDbSingleton)serviceProvider.GetService(typeof(EndpointStatusDbSingleton));
        }

        public IEnumerable<EndpointStatus> GetCurrentState()
        {
            var s = ReadStatuses();

            return s;
        }

        public IEnumerable<EndpointStatus> AddWatcher(EndpointInfo endpointInfo)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(endpointInfo.Host);
            _watchers.Add(new EndpointWatcher(client, endpointInfo, _serviceProvider));
            //_statuses.Add(new EndpointStatus() { Host = endpointInfo.Host, PollIntervalMilliseconds = endpointInfo.PollIntervalMilliseconds, Result = EndpointCheckResult.OK });

            var s = ReadStatuses();
            return s;
        }

        public IEnumerable<EndpointStatus> RemoveWatcherAndGetData(string host)
        {
            RemoveWatcher(host);
            var s = ReadStatuses();

            return s;
        }

        public void RemoveWatcher(string host)
        {
            EndpointResponse res = null;
            var targetWatcher = _watchers.FirstOrDefault(ss => ss.Info.Host == host);
            targetWatcher.Terminate();

            _watchers.Remove(targetWatcher);
        }

        private IEnumerable<EndpointStatus> ReadStatuses()
        {
            var statusesResult = new List<EndpointStatus>();
            var statusesSnapshot = _endpointStatusDb.Statuses.ToList();

            foreach (var dbStatus in statusesSnapshot)
            {
                var endpointStatus = new EndpointStatus() { Host = dbStatus.Key, Response = dbStatus.Value };
                statusesResult.Add(endpointStatus);
            }

            return statusesResult;
        }
    }
}
