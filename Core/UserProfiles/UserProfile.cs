using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace Yazawa_Nico.Core.UserProfiles
{
    public class UserProfile
    {
        public ulong ID { get; set; } // User ID

        public string Username { get; set; } // User username

        public uint Points { get; set; } // Amount of points/credits/currency the user has

        public uint XP { get; set; } // Amount of exp the user has

        // Level number
        public uint LevelNumber
        {
            get
            {
                // Calculating the level
                // y = x ^ 2 * 50 || XP = LEVEL ^ 2 * 50
                return (uint)Math.Sqrt(XP / 50);
            }
        }

        public DateTime daily { get; set; } // Time of daily

        public bool dailyB{ get; set; } // Bool to determine if can get exp or not

        public bool IsMuted { get; set; }

        public bool Afk { get; set; } // Is the user Afk?

        public string AfkMessage {get; set; } // Afk message

        public bool IsRPS { get; set; } // Is playing regular RPS game?

        public bool IsRPSGamble { get; set; } // Is playing gamble RPS game? 

        public uint GambleAmount { get; set; } // Amount for gambling

        public List<ulong> MarriageProposals { get; set; } // List of users proposed to user

        public List<ulong> SentMarriageProposals { get; set; } // List of sent proposals from user

        public List<ulong> Marriages { get; set; } // List of marriages of user

        public uint MarriageSlots { get; set; } // Amount of available slots

        public bool IsBuyingSlot { get; set;} // Buy Marriage Slot
    }
}