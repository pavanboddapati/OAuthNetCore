using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pavan.NetCore.UI.Models;
using Pavan.NetCore.UI.Services;

namespace Pavan.NetCore.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationService appService;

        public HomeController(ILogger<HomeController> logger, IApplicationService appService)
        {
            _logger = logger;
            this.appService = appService;
        }

        public async Task<IActionResult> Index()
        {
            var appNames = await appService.GetApplicationNames();
            return View(appNames);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Everyone")]
        public IActionResult Everyone()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
