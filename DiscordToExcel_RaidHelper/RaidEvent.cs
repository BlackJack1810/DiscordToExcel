namespace DiscordToExcel_RaidHelper
{
    //Raid class to represent a raid event
    public class RaidEvent
    {
        public string Name { get; set; }
        public List<Participant> Participants { get; set; } = new List<Participant>();
    }
}