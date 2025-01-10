namespace DiscordToExcel_RaidHelper.Datamodel
{
    // Participant class to represent a participant in a raid event
    public class SignUps
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string NameDiscord { get; set; }
        public string NameMain { get; set; }
    }
}