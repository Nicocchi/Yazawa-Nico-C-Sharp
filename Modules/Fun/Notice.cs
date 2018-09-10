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

namespace Yazawa_Nico.Modules.Fun
{
    public class Notice : ModuleBase<SocketCommandContext>
    {
        #region Variabls
        Random rand = new Random();
        EmbedBuilder builder = new EmbedBuilder();
        string message = null;
        string allUsers = null;
        #endregion

        [Command("notice", RunMode = RunMode.Async)]
        [Summary("notice | Notice senpai. Requires bot SendMessages and EmbedLinks permissions.")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default([Remainder]string users = null)
        {
            try
            {
                var mentionedUser = Context.Message.MentionedUsers;
                List<string> usernames = new List<string>();

                if (!string.IsNullOrEmpty(users))
                {
                    // Add the mentioned users into a list
                    foreach (var usr in mentionedUser)
                    {
                        usernames.Add(usr.Username.ToString());
                    }

                    // Check whether the current user mentioned themselves, and set message accordingly
                    if (usernames.Contains(Context.User.Username))
                    {
                        usernames.Remove(Context.User.Username);
                        usernames.Add("themself");

                        string[] usrs = usernames.ToArray();
                        allUsers = String.Join(" & ", usrs);
                        
                    }
                    else 
                    {
                        string[] usrs = usernames.ToArray();
                        allUsers = String.Join(" & ", usrs);
                    }

                    message = $"**{Context.User.Username}** has noticed {allUsers}!!";
                }
                else
                {
                    message = $" **{Context.User.Username}**, you need to tag a user. Usage => `!notice <user>`";
                }

                builder.WithColor(new Color(255,73,167));
                builder.AddField("(≧▽≦)", message);
                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ReplyAsync("Sorry, failed to load image");
            }
            
        }
        
    }
}