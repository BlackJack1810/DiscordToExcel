using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper.API
{
    //Helper Class to store the API response from a single Raid
    internal class ApiResponseSingle
    {
        [System.Text.Json.Serialization.JsonPropertyName("signUps")]
        public List<SignUps> SignUps { get; set; } = new List<SignUps>();
    }
}
