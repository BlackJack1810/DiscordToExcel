namespace DiscordToExcel_RaidHelper.Datamodel
{
    // Participant class to represent a participant in a raid event
    // if there was a final setup posted with the composition tool, we use this class to display the final setup
    public class FinalSetup
    {
        [System.Text.Json.Serialization.JsonPropertyName("partyId")]
        public required int PartyId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("slotId")]
        public int SlotId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public required string NameDiscord { get; set; }
        public string? NameMain { get; set; }

    }
}