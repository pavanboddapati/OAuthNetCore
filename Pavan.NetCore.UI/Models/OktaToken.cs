using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pavan.NetCore.UI.Models
{
    public class OktaToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn{ get; set; }

        public DateTime ExpiresAt { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(60);        }
        }
    }
}