using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper
{
    public class RaidHelperApi
    {
        private readonly string apiToken;
        private readonly HttpClient httpClient;

        public RaidHelperApi(string token)
        {
            apiToken = token;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.raidhelper.com/")
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
        }

        public async Task<List<RaidEvent>> GetRaidEventsAsync()
        {
            var response = await httpClient.GetAsync("events");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RaidEvent>>(content);
        }
    }
}
