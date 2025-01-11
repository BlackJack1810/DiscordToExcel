namespace DiscordToExcel_RaidHelper.Datamodel
{
    // Participant class to represent a participant in a raid event
    // for now we only need the name of the participant from the signup object
    public class SignUps
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public required string NameDiscord { get; set; }
        public string? NameMain { get; set; }
    }
}