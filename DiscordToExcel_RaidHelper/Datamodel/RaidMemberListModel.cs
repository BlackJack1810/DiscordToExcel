using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper.Datamodel
{
    // Model Class to create the SignUps_Structure structure
    internal class RaidMemberListModel
    {
        public ObservableCollection<SignUps> CreateRaidMemberList()
        {
            var raidMembers = new ObservableCollection<SignUps>();

            // add the groups
            for (int group = 1; group <= 5; group++)
            {
                raidMembers.Add(new SignUps { 
                    NameDiscord = $"Group {group}",
                    NameMain = String.Empty,
                    IsGroupHeader = true 
                });
                for (int i = 1; i <= 5; i++)
                {
                    raidMembers.Add(new SignUps
                    {
                        NameDiscord = "Platzhalter für \"NameDiscord\"",
                        NameMain = "Platzhalter für \"NameMain\"",
                        IsGroupHeader = false
                    });
                }
            }
            return raidMembers;
        }
    }
}
