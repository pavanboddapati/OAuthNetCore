using System;
namespace Pavan.NetCore.UI.Models
{
    public class OktaApiClient
    {
        public OktaApiClient()
        { }

        public string TokenUrl { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }

	}
}
