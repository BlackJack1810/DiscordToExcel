using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DiscordToExcel_RaidHelper.API
{
    public class RaidHelperApi
    {
        // HttpClient for communicating with the API
        private readonly HttpClient httpClient;

        public RaidHelperApi(string token)
        {
            // Initialize HttpClient with the base address of the API
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://raid-helper.dev")
            };
            // Add the Authorization header with the API token
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
        }

        // Asynchronous method to retrieve raid events from the API
        public async Task<List<AllCurrentRaids>> GetRaidEventsAsync(String ID)
        {
            // Send a GET request to the "events" endpoint. The number after "servers/" is the server ID.
            var response = await httpClient.GetAsync("/api/v3/servers/" + ID + "/events");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();
            
            // Read the response content as a string
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize the content into the ApiResponse model
            var apiResponse = JsonSerializer.Deserialize<ApiResponseAll>(content);

            // Return only the posted events
            return apiResponse?.PostedEvents ?? new List<AllCurrentRaids>();
        }

        // Asynchronous method to retrieve raid participants from the API for a specific raid event
        public async Task<List<AllCurrentRaids>> GetRaidParticipantsAsync(List<AllCurrentRaids> Raids)
        {
            // Loop through the raids and add the participants to the raids List
            foreach (var raid in Raids)
            {
                var response = await httpClient.GetAsync($"/api/v2/events/{raid.ID}");
                
                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                // Deserialize the content into the ApiResponse model
                var apiResponse = JsonSerializer.Deserialize<ApiResponseSingle>(content);
                var signUps = apiResponse?.SignUps ?? new List<SignUps>();
                raid.SignUps.AddRange(signUps);

            }
            // Return only the posted events
            return Raids;
        }
    }
}

