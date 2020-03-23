using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pavan.NetCore.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pavan.NetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IConfiguration configuration;

        public MembershipController(ILDAPService ldapService, IConfiguration configuration)
        {
            LdapService = ldapService;
            this.configuration = configuration;
        }

        public ILDAPService LdapService { get; }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return LdapService.GetMembersInGroup(configuration.GetValue<string>("GroupName"));
        }
    }
}
