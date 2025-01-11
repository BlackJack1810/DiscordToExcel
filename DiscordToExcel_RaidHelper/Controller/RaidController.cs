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

        public RaidController()
        {
            raidHelperApi = new RaidHelperApi("v7ydHm1q2ZKzthA5svdhPCv5e2s6HUo5Zlpcj8LL");
        }
        //Load all raid events and participants
        public async Task<List<AllCurrentRaids>> LoadRaidEventsAsync()
        {
            var events = await raidHelperApi.GetRaidEventsAsync();
            return await raidHelperApi.GetRaidParticipantsAsync(events);
        }


    }
}
