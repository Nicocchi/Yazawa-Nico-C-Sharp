using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Yazawa_Nico.Modules.Games
{
    public class Wisdom : ModuleBase<SocketCommandContext>
    {
        Random rand = new Random();
        string fileLoc = Path.Combine(AppContext.BaseDirectory, "JSON/wisdom.json"); // Location of the JSON file
        string wisdomtext;


        [Command("wisdom", RunMode = RunMode.Async)]
        [Summary("Gives some wisdom")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WisdomT()
        {
            LoadJson();
            await ReplyAsync(wisdomtext);
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
                wisdomtext = predictionsTexts[randomIndex]; // Set the wisdomtext to the quote
            }
        }
        #endregion
    }
}