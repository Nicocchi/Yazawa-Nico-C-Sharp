using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using Yazawa_Nico.Core.UserProfiles;

namespace Yazawa_Nico.Modules.Fun
{
    public class Send : ModuleBase<SocketCommandContext>
    {
        [Command("send", RunMode = RunMode.Async)]
        [Summary("send | Send Nico Nii's to a user. Requires bot SendMessages and EmbedLinks permissions.")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default(IGuildUser user, uint amount)
        {
            var currentUser = UserProfiles.GetAccount(Context.User);
            var transferUser = UserProfiles.GetAccount((SocketUser)user);

            if (currentUser.Points >= amount)
            {
                currentUser.Points -= amount;
                transferUser.Points += amount;
                UserProfiles.SaveProfiles();

                await ReplyAsync($":white_check_mark: | {Context.User.Username}, you sent {amount} to **{user.Username}**");
            }
            else
            {
                await ReplyAsync($":negative_squared_cross_mark: | {Context.User.Username}, you do not have enough Nico Nii's.");
            }
        }
    }
}