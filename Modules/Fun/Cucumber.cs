using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task Default(IGuildUser user,[Remainder]string item = null)
        {
            if (!String.IsNullOrWhiteSpace(item))
            {
                //SocketUser target = null;
                var users = new[] {""};
                var mentionedUser = Context.Message.MentionedUsers;
                List<string> meUsr= new List<string>();

                //target = mentionedUser ?? Context.User;

                foreach (var usr in mentionedUser)
                {
                    meUsr.Add(usr.Username.ToString());
                }

                string[] usrs = meUsr.ToArray();
                string allUsers = String.Join(",", usrs);
                
                await ReplyAsync($"**{allUsers}**, you got a :cucumber: from **{Context.User.Username}**\n \n (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ :cucumber:");
            }
            else
            {
                await ReplyAsync($"**{Context.User.Mention}** you need to give the cucumber to a user. `usage !cucumber <user>`");
            }
        }
    }
}
