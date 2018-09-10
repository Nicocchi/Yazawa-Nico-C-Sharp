using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Web;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using Yazawa_Nico.Core.GuildAccounts;
using Yazawa_Nico.Core.UserProfiles;

namespace Yazawa_Nico.Modules.Utils
{
    public class Afk : ModuleBase<SocketCommandContext>
    {
        [Command("afk")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task AfkUser([Remainder] string message = "")
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.AfkModule)
            {
                var userProfile = UserProfiles.GetAccount(Context.User);
                
                userProfile.Afk = true;
                userProfile.AfkMessage = message;
                UserProfiles.SaveProfiles();
                await Context.Channel.SendMessageAsync($"Marking you as away **{Context.User.Username}**");

            }

        }
    }
}