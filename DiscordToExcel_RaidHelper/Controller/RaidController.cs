using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper.Controller
{
    public class RaidController
    {
        private RaidHelperApi raidHelperApi;
        private string ServerID;
        private string APIKey;
        private AppSettings _appSettings;

        public RaidController(String APIKey, String ServerID)
        {
            this.ServerID = ServerID;
            this.APIKey = APIKey;
        }
        //Load all raid events and participants
        public async Task<List<AllCurrentRaids>> LoadRaidEventsAsync()
        {
            // check for ServerID and APIKey
            _appSettings = SettingsManager.LoadSettings();
            ServerID = _appSettings.ServerId;
            APIKey = _appSettings.RaidHelperApi;
            if (!string.IsNullOrEmpty(ServerID) && !string.IsNullOrEmpty(APIKey))
            {                               
                raidHelperApi = new RaidHelperApi(APIKey);
                var events = await raidHelperApi.GetRaidEventsAsync(ServerID);
                return await raidHelperApi.GetRaidParticipantsAsync(events);
            } else
            {
                return new List<AllCurrentRaids>();
            }

        }


    }
}
