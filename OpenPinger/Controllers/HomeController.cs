using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenPinger.Dtos;
using OpenPinger.Models;

namespace OpenPinger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EndpointStatusProvider _provider;

        public HomeController(ILogger<HomeController> logger, EndpointStatusProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public IActionResult Index()
        {
            var data = _provider.GetCurrentState();
            var model = new EndpointStatusesViewModel() { Statuses = data };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost("addEndpoint")]
        public IActionResult AddEndpoint(EndpointInfo statusModel)
        {
            if(!IsValid(statusModel))
            {
                var prevData = _provider.GetCurrentState();
                var prevModel = new EndpointStatusesViewModel() { Statuses = prevData };
                return View("Index", prevModel);
            }

            statusModel.PollIntervalMilliseconds = 3000;
            var data = _provider.AddWatcher(statusModel);
            var model = new EndpointStatusesViewModel() { Statuses = data };

            return RedirectToAction("Index");
        }

        [HttpPost("deleteEndpoint")]
        public IActionResult RemoveEndpoint(string hostName)
        {
            var data = _provider.RemoveWatcherAndGetData(hostName);
            var model = new EndpointStatusesViewModel() { Statuses = data };

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsValid(EndpointInfo model)
        {
            return !string.IsNullOrWhiteSpace(model.Host);
        }
    }
}
