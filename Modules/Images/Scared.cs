using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yazawa_Nico.Modules.Core;

namespace Yazawa_Nico.Modules.Images
{
    public class Scared : ModuleBase<SocketCommandContext>
    {
        [Command("scared", RunMode = RunMode.Async)]
        [Summary("scared | Be scared. Requires bot SendMessages and EmbedLinks permissions.")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default([Remainder]string users = null)
        {
            string fileLoc = "JSON/scared.json"; // Location of the JSON file
            string message = "";
            string allUsers = "";

            try
            {
                string imageUrl = Utility.LoadJson(fileLoc);

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

                    message = $"**{Context.User.Username}** is scared by {allUsers}!";
                }
                else
                {
                    message = $" **{Context.User.Username}** is scared.";
                }

                EmbedBuilder builder = new EmbedBuilder()
                    .WithColor(new Color(255,73,167))
                    .AddField("〣( ºΔº )〣", message)
                    .WithImageUrl(imageUrl);
                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load image. Error: {ex.Message}");
                await ReplyAsync("Sorry, failed to load image");
            }
            
        }

    }
}
