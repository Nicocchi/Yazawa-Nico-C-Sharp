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
    public class Senpai : ModuleBase<SocketCommandContext>
    {
        #region Variabls
        Random rand = new Random();
        EmbedBuilder builder = new EmbedBuilder();
        string fileLoc = Path.Combine(AppContext.BaseDirectory, "JSON/senpai.json"); // Location of the JSON file
        string imageUrl;
        string message = null;
        string allUsers = null;
        #endregion

        [Command("senpai", RunMode = RunMode.Async)]
        [Summary("senapi | Get senpai to notice you. Requires bot SendMessages and EmbedLinks permissions.")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default([Remainder]string users = null)
        {
            try
            {
                //LoadJson();

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
                        usernames.Add("yourself");

                        string[] usrs = usernames.ToArray();
                        allUsers = String.Join(" & ", usrs);
                        
                    }
                    else 
                    {
                        string[] usrs = usernames.ToArray();
                        allUsers = String.Join(" & ", usrs);
                    }

                    message = $"**{Context.User.Username}** is trying to get {allUsers} to notice them...\n\n Please notice me senpai ಠ_ಠ";
                }
                else
                {
                    message = $" **{Context.User.Username}**, you need to tag a user. Usage => `!senpai <user>`";
                }

                builder.WithColor(new Color(255,73,167));
                builder.AddField("ಠ_ಠ", message);
                //builder.WithImageUrl(imageUrl);
                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await ReplyAsync("Sorry, failed to load image");
            }
            
        }

        #region Methods
        public void LoadJson()
        {
            // Load and Parse the JSON file
            JObject o1 = JObject.Parse(File.ReadAllText(fileLoc));

            // Read JSON directly from a file
            using (StreamReader file = File.OpenText(fileLoc))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                JArray links = (JArray)o1["file"]; // Turn the string into an string[]
                IList<string> linksText = links.Select(c => (string)c).ToList(); // Convert string into a List<string>

                string[] predictionsTexts = linksText.ToArray(); // Convert to new array
                int randomIndex = rand.Next(predictionsTexts.Length); // Get a random int and select an index from the predictionsText array
                imageUrl = predictionsTexts[randomIndex]; // Set the imageUrl to the link
            }
        }
        #endregion
    }
}