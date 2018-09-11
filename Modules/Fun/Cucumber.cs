using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Yazawa_Nico.Modules.Fun
{
    public class Cucumber : ModuleBase<SocketCommandContext>
    {
        [Command("cucumber", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default(IGuildUser user)
        {
            IGuildUser target = user;
            
            await ReplyAsync($"**{target}**, you got a :cucumber: from **{Context.User.Username}**\n \n (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ :cucumber:");
        }
    }
}
