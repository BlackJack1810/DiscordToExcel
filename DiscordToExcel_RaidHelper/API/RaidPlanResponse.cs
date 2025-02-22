using DiscordToExcel_RaidHelper.Datamodel;

public class RaidPlanResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("raidDrop")]
    public List<FinalSetup> RaidDrop { get; set; } = new();
}
