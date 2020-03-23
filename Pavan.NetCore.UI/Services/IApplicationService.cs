using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Pavan.NetCore.UI.Services
{
    public interface IApplicationService
    {
        Task<IList<string>> GetApplicationNames(string token="");
    }

    public class ApplicationService : IApplicationService
    {
        private readonly ITokenService tokenService;
        private HttpClient client = new HttpClient();
        public ApplicationService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        public async Task<IList<string>> GetApplicationNames(string token = "")
        {
            var applicationNames = new List<string>();
            if (string.IsNullOrEmpty(token))
                token = await this.tokenService.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://localhost:44302/api/Application");

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                applicationNames = JsonSerializer.Deserialize<List<string>>(json);
            }
            else
            {
                applicationNames = new List<string> { response.StatusCode.ToString(), response.ReasonPhrase };
            }
            return applicationNames;
        }
    }
}