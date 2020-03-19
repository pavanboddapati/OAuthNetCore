using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Pavan.NetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private List<String> appNames = new List<string> { "ARAM", "AZOB", "PPOST", "AMCT" };
        [HttpGet]
        public IActionResult GetApps()
        {
            return Ok(appNames);
        }
    }
}