using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper.API
{
    //Helper Class to store the API response from all current Raids
    internal class ApiResponseAll
    {
        [System.Text.Json.Serialization.JsonPropertyName("postedEvents")]
        public List<AllCurrentRaids> PostedEvents { get; set; } = new List<AllCurrentRaids>();
    }
}
