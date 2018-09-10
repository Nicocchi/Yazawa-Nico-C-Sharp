using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Yazawa_Nico
{
    internal static class Global
    {
        internal static DiscordSocketClient client { get; set; }
        internal static ulong MessageToTrack { get; set;}
    }
}