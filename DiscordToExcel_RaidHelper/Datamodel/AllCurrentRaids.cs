﻿namespace DiscordToExcel_RaidHelper.Datamodel
{
    //Raid class to represent a raid event
    public class AllCurrentRaids
    {
        [System.Text.Json.Serialization.JsonPropertyName("title")]
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("startTime")]
        public long StartTimeUnix { get; set; } // Unix-TimeStamp for the start time

        public DateTime StartTime => DateTimeOffset.FromUnixTimeSeconds(StartTimeUnix).DateTime;

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string IdRaw { get; set; }  
        public long ID => long.TryParse(IdRaw, out var id) ? id : 0;

        [System.Text.Json.Serialization.JsonPropertyName("signUpCount")]
        public string SignUpCountRaw { get; set; }

        public int SignUpCount => int.TryParse(SignUpCountRaw, out var count) ? count : 0;
        
        // Add a property to hold the list of sign-ups
        public List<SignUps> SignUps { get; set; } = new List<SignUps>();
    }
}