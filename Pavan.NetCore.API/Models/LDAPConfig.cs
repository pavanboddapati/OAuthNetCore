using System;
namespace Pavan.NetCore.Api.Models
{
    public class LDAPConfig
    {
        public string LDAPServer { get; set; }
        public string DomainUser { get; set; }
        public string DomainPassword { get; set; }
        public string SearchBase { get; set; }
        public string SearchFilter { get; set; }
        public string AdminCn { get; set; }
    }
}
