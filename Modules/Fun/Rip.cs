using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Yazawa_Nico.Core;

namespace Yazawa_Nico.Modules.Fun
{
    

    public class Rip : ModuleBase<SocketCommandContext>
    {
        uint totalRips;
        uint todaysRips;

        [Command("f", RunMode = RunMode.Async)]
        [Alias("rip", "ded")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DefaultRip([Remainder]string msg = "")
        {
            SetRips();

            var builder = new EmbedBuilder();
            builder.WithTitle("");
            builder.WithColor(new Color(255,73,167));
            if (msg != "")
            {
                builder.WithDescription($"**{Context.User.Username}** has paid their respects for {msg}");
                builder.WithFooter(footer =>
            {
                footer
                    .WithText($"{todaysRips} Today, {totalRips} All");
            });
            }
            else
            {
                builder.WithDescription($"**{Context.User.Username}** has paid their respects.");
                builder.WithFooter(footer =>
            {
                footer
                    .WithText($"{todaysRips} Today, {totalRips} All");
            });
            }
            
            //builder.WithDescription($"{today} today, {globalRip} All");
            await ReplyAsync("", false, builder.Build());
        }

        // Need to refactor
        private void SetRips() 
        {
            const string globalFolder = "Resources";
            const string globalFile = "globals.json";
            string json;
            DateTime current = DateTime.UtcNow;

            // Check if the file/folder exists, if not, create then read the file
            if (!File.Exists(globalFolder + "/" + globalFile))
            {
                Globals.CreateFile();
                json = File.ReadAllText(globalFolder + "/" + globalFile);
            }
            else {
                json = File.ReadAllText(globalFolder + "/" + globalFile);
            }

            // Create a new globals object from the json
            var globals = JsonConvert.DeserializeObject<Globals.GlobalConfig>(json);

            totalRips = globals.totalRips;
            todaysRips = globals.todaysRips;
            var currentDate = globals.currentDate;

            int result = DateTime.Compare(currentDate, current);

            if (result == 0) {
                todaysRips += 1;
            }
            else {
                todaysRips = 1;
            }
            totalRips += 1;

            // Create a new GlobalConfig object and set the appropriate values 
            var globalConfig = new Globals.GlobalConfig();
            globalConfig.totalRips = totalRips;
            globalConfig.todaysRips = todaysRips;
            globalConfig.currentDate = current;

            // Save the current rips
            DataStorage.UpdateGlobals(globalConfig);
        }
    }
}
