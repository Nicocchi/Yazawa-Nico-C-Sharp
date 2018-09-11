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
    [Group("leavemsg")]
    public class UserLeaveMsg : ModuleBase<SocketCommandContext>
    {
        [Command("setchannel")]
        [Summary("Set the channel for the leave message")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetChannel(ulong id)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            guildProfile.LeaveChannel = id;
            GuildProfiles.SaveProfiles();

            await Context.Channel.SendMessageAsync($":white_check_mark:  Leave channel has been set to `{id}`");
        }

        [Command("settext")]
        [Summary("Set the text for the leave message")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetText([Remainder]string message)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            guildProfile.LeaveMessage = message;
            GuildProfiles.SaveProfiles();

            await Context.Channel.SendMessageAsync($":white_check_mark:  Leave message has been set to `{message}`");
        }

        // Enable Leave Module
        [Command("enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LeaveEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.LeaveModule)
            {
                guildProfile.LeaveModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Leave Module
        [Command("disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LeaveDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.LeaveModule)
            {
                guildProfile.LeaveModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }
    }
}