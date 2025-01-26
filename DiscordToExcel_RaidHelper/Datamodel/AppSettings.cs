using DiscordToExcel_RaidHelper.Datamodel;

public class AppSettings
{
    public string ServerId { get; set; }
    public string RaidHelperApi { get; set; }
    public List<NameMapping> NameMappings { get; set; } = new();
}
