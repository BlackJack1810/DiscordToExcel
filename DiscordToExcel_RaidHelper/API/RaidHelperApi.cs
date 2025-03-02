using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
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
            
            // Read the responseSignUps contentSignUps as a string
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize the contentSignUps into the ApiResponse model
            var apiResponse = JsonSerializer.Deserialize<ApiResponseAll>(content);

            // Aktuelle Zeit als Unix-Timestamp
            long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Filtere nur zukünftige Events basierend auf Unix-Timestamp
            return apiResponse?.PostedEvents?
                .Where(e => e.StartTimeUnix > currentUnixTime)
                .ToList() ?? new List<AllCurrentRaids>();
        }

        // Asynchronous method to retrieve raid participants from the API for a specific raid event
        public async Task<List<AllCurrentRaids>> GetRaidParticipantsAsync(List<AllCurrentRaids> Raids)
        {
            // Loop through the raids and add the participants to the raids List
            foreach (var raid in Raids)
            {
                // API-Calls
                var responseRaidPlan = await httpClient.GetAsync($"https://raid-helper.dev/api/raidplan/{raid.ID}");
                responseRaidPlan.EnsureSuccessStatusCode();

                var responseSignUps = await httpClient.GetAsync($"/api/v2/events/{raid.ID}");
                responseSignUps.EnsureSuccessStatusCode();

                if (responseRaidPlan.IsSuccessStatusCode)
                {
                    var contentRaidPlan = await responseRaidPlan.Content.ReadAsStringAsync();


                    // Falls die JSON leer ist, gibt es keinen Plan
                    if (!string.IsNullOrWhiteSpace(contentRaidPlan) && contentRaidPlan != "{}")
                    {
                        var apiResponseRaidPlan = JsonSerializer.Deserialize<RaidPlanResponse>(contentRaidPlan);
                        var finalSetupList = apiResponseRaidPlan?.RaidDrop ?? new List<FinalSetup>();

                        if (finalSetupList != null)
                        {
                            raid.FinalSetup = finalSetupList;
                            continue; // if a final setup exists, skip the next API call
                        }
                    }
                }
                if (responseSignUps.IsSuccessStatusCode)
                {
                    // Read the response contentas a string
                    var contentSignUps = await responseSignUps.Content.ReadAsStringAsync();

                    // Deserialize the contentSignUps into the ApiResponse model
                    var apiResponseSignUps = JsonSerializer.Deserialize<ApiResponseSingle>(contentSignUps);
                    var signUps = apiResponseSignUps?.SignUps ?? new List<SignUps>();

                    // Remove sign-ups with "Absence" or "Tentative" in the className attribute
                    signUps = signUps.Where(s => s.Classname != "Absence" && s.Classname != "Tentative").ToList();

                    raid.SignUps.AddRange(signUps);
                }

            }
            // Return only the posted events
            return Raids;
        }
    }
}

