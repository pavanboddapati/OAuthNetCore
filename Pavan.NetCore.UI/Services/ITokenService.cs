using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pavan.NetCore.UI.Models;
using System.Text.Json;

namespace Pavan.NetCore.UI.Services
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }

    public class OktaTokenService : ITokenService
    {
        private readonly IOptions<OktaApiClient> options;
        private OktaToken token = new OktaToken();

        public OktaTokenService(IOptions<OktaApiClient> options)
        {
            this.options = options;
        }
        public async Task<string> GetToken()
        {
            if(!this.token.IsValidAndNotExpiring)
            {
                this.token = await this.GetNewAccessToken();
            }
            return this.token.AccessToken;
        }

        private async Task<OktaToken> GetNewAccessToken()
        {
            var tokenFromOkta = new OktaToken();
            var client = new HttpClient();

            var client_id = this.options.Value.ClientId;
            var client_secret = this.options.Value.ClientSecret;

            var clientCredentials = Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCredentials));

            var postMessage = new Dictionary<string,string>();
            postMessage.Add("grant_type", "client_credentials");
            postMessage.Add("scope", "access_token");

            var request = new HttpRequestMessage(HttpMethod.Post, this.options.Value.TokenUrl){
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                tokenFromOkta = JsonSerializer.Deserialize<OktaToken>(json);
                tokenFromOkta.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);
            }
            else{
                throw new ApplicationException("unable to retrieve token");
            }
            return tokenFromOkta;
        }
    }
}
