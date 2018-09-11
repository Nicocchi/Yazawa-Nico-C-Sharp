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
using Newtonsoft.Json;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Modules.Moderation
{
    [Group("greetmsg")]
    public class UserJoinedMsg : ModuleBase<SocketCommandContext>
    {
        [Command("setchannel")]
        [Summary("Set the channel for the greet message")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetChannel(ulong id)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            guildProfile.GreetChannel = id;
            GuildProfiles.SaveProfiles();

            await Context.Channel.SendMessageAsync($":white_check_mark:  Greet channel has been set to `{id}`");
        }

        [Command("settext")]
        [Summary("Set the text for the greet message")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetText([Remainder]string message)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            guildProfile.GreetMessage = message;
            GuildProfiles.SaveProfiles();

            await Context.Channel.SendMessageAsync($":white_check_mark:  Greet message has been set to `{message}`");
        }

        // Enable Greet Module
        [Command("enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GreetEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.GreetModule)
            {
                guildProfile.GreetModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Greet Module
        [Command("disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GreetDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.GreetModule)
            {
                guildProfile.GreetModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }
    }
}