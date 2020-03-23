using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pavan.NetCore.UI.Services
{
    public interface ILDAPService
    {
        Task<IList<String>> GetGroupMembersInGroup(string token = "");
    }

    public class LDAPService : ILDAPService
    {
        private HttpClient client = new HttpClient();
        public async Task<IList<string>> GetGroupMembersInGroup(string token)
        {
            var groupMemberNames = new List<String>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://localhost:44302/api/Membership");

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                groupMemberNames = JsonSerializer.Deserialize<List<string>>(json);
            }
            else
            {
                groupMemberNames.Add("No Members found");
            }
            return groupMemberNames;
        }
    }
}
