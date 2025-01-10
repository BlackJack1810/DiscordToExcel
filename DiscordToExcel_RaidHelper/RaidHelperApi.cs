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
        // API token for authentication
        private readonly string apiToken;
        // HttpClient for communicating with the API
        private readonly HttpClient httpClient;

        public RaidHelperApi(string token)
        {
            apiToken = token;
            // Initialize HttpClient with the base address of the API
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.raidhelper.com/")
            };
            // Add the Authorization header with the API token
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
        }

        // Asynchronous method to retrieve raid events from the API
        public async Task<List<RaidEvent>> GetRaidEventsAsync()
        {
            // Send a GET request to the "events" endpoint
            var response = await httpClient.GetAsync("events");
            // Ensure the request was successful
            response.EnsureSuccessStatusCode();
            // Read the response content as a string
            var content = await response.Content.ReadAsStringAsync();
            // Deserialize the content into a list of RaidEvent objects
            return JsonSerializer.Deserialize<List<RaidEvent>>(content);
        }
    }
}

