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
    [Group("warn")]
    public class WarnModuleEnable : ModuleBase<SocketCommandContext>
    {
        [Command("setamount")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnAmount(uint amount)
        {
            
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            uint previousAmount = guildProfile.NumberofWarnings;
            if (amount <= 0)
            {
                await Context.Channel.SendMessageAsync($":negative_squared_cross_mark:  Warn amount cannot be set to {amount}. It must be higher than `0` Setting it back to {previousAmount}");
            }
            else
            {
                guildProfile.NumberofWarnings = amount;
                GuildProfiles.SaveProfiles();

                await Context.Channel.SendMessageAsync($":white_check_mark: Warn amount has been set to {amount}");
            }

            
        }

        // Enable WarnBan Module
        [Command("banenable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnBanEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.WarningsBan)
            {
                guildProfile.WarningsBan = true;
                guildProfile.WarningsKick = false;
                guildProfile.WarningsMute = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Ban Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Ban Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable WarnBan Module
        [Command("bandisable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnBanDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.WarningsBan)
            {
                guildProfile.WarningsBan = false;
                guildProfile.WarningsKick = false;
                guildProfile.WarningsMute = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Ban Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Ban Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable WarnKick Module
        [Command("kickenable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnKickEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.WarningsKick)
            {
                guildProfile.WarningsBan = false;
                guildProfile.WarningsKick = true;
                guildProfile.WarningsMute = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Kick Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Kick Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable WarnKick Module
        [Command("kickdisable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnKickDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.WarningsKick)
            {
                guildProfile.WarningsBan = false;
                guildProfile.WarningsKick = false;
                guildProfile.WarningsMute = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Kick Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Kick Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable WarnMute Module
        [Command("Muteenable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnMuteEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.WarningsMute)
            {
                guildProfile.WarningsBan = false;
                guildProfile.WarningsKick = false;
                guildProfile.WarningsMute = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable WarnMute Module
        [Command("mutedisable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnMuteDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.WarningsMute)
            {
                guildProfile.WarningsBan = false;
                guildProfile.WarningsKick = false;
                guildProfile.WarningsMute = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

    }
}