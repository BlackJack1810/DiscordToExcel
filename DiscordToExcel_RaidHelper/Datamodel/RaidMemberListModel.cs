﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordToExcel_RaidHelper.Datamodel
{
    // Model Class to create the SignUps structure
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
                    NameMain = $"Group {group}",
                    Classname = "header",
                    IsGroupHeader = true 
                });
                for (int i = 1; i <= 5; i++)
                {
                    raidMembers.Add(new SignUps
                    {
                        NameDiscord = "Placeholder",
                        NameMain = "",
                        IsGroupHeader = false,
                        IsOnBench = false
                    });
                }
            }
            return raidMembers;
        }
    }
}
