namespace DiscordToExcel_RaidHelper
{
    public class RaidEvent
    {
        public string Name { get; set; }
        public List<Participant> Participants { get; set; } = new List<Participant>();
    }
}