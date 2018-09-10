using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace Yazawa_Nico.Core.GuildAccounts
{
    public class GuildProfile
    {
        public ulong ID { get; set; } // Guild ID

        public string GreetMessage { get; set; } // User join message

        public string LeaveMessage { get; set; } // User leave message

        public bool GreetModule { get; set; } // User join message module

        public bool LeaveModule { get; set; } // User leave message module

        public ulong GreetChannel { get; set; } // Greet message channel

        public ulong LeaveChannel { get; set; } // Leave message channel

        public uint NumberofWarnings { get; set; } // Max number of warnings

        public bool WarningsBan { get; set; } // Ban from warnings

        public bool WarningsKick { get; set; } // Kick from warnings

        public bool WarningsMute { get; set; } // Mute from warnings

        public bool BanModule { get; set; } // Ban module

        public bool KickModule { get; set; } // Kick module

        public bool MuteModule { get; set; } // Mute module

        public bool WarnModule { get; set; } // Warn module

        public bool AfkModule { get; set; } // Afk module

        public bool PurgeModule { get; set; } // Purge module

        public bool LevelModule { get; set; } // Level module

        public string CmdPrefixDefault { get; set; } // Default Prefix
        public string CmdPrefix { get; set; } // Custom Command Prefix



    }
}