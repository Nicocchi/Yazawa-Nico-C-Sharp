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
    public class Give : ModuleBase<SocketCommandContext>
    {
        [Command("give", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task GiveCucumber(IGuildUser user,[Remainder]string item = null)
        {
            if (!String.IsNullOrWhiteSpace(item))
            {
                SocketUser target = null;
                var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
                List<string> meUsr= new List<string>();

                target = mentionedUser ?? Context.User;
                
                await Context.Channel.SendMessageAsync($"**{target}**, you got a **{item}** from **{Context.User.Username}**\n \n (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ **{item}**");
            }
            else
            {
                await ReplyAsync($"**{Context.User.Mention}** you need to give an item to a user. `usage !give <user> [item]`");
            }
        }
    }
}
