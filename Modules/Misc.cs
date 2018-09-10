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
using Yazawa_Nico.Core.UserProfiles;

namespace Yazawa_Nico.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        // [Command("levelis")]
        // [RequireBotPermission(GuildPermission.SendMessages)]
        // public async Task WhatLevelIs(uint xp)
        // {
        //     uint level = (uint)Math.Sqrt(xp / 50);
        //     await Context.Channel.SendMessageAsync("The level is " + level);
        // }

        // [Command("xpis")]
        // [RequireBotPermission(GuildPermission.SendMessages)]
        // public async Task WhatXPIs(uint level)
        // {
        //     uint xp = (uint)Math.Pow(level, 2) * 50;
        //     await Context.Channel.SendMessageAsync("The XP is " + xp);
        // }

        // [Command("hello")]
        // public async Task Hello()
        // {
        //     string css = "<style>\n    h1{\n        color: red;\n    }\n</style>\n";
        //     string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);
        //     var converter = new HtmlToImageConverter
        //     {
        //         Width = 250,
        //         Height = 70
        //     };
        //     var jpgBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
        //     await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");
        // }

        // [Command("image")]
        // public async Task ImageGen()
        // {
            
        //     var html = String.Format("<body>Hello world: {0}</body>",DateTime.Now);
        //     var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();
        //     var jpegBytes = htmlToImageConv.GenerateImage(html, NReco.ImageGenerator.ImageFormat.Jpeg);

        //     await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "hello1.jpg");
            
        // }

        /*
        [Command("whatlevelis")]
        public async Task WhatLevelIs(uint xp)
        {
            uint level = (uint)Math.Sqrt(xp / 50);
            await Context.Channel.SendMessageAsync("The level is " + level);
        }

        [Command("react")]
        public async Task HandleReactionMessage()
        {
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            Global.MessageToTrack = msg.Id;
        }

        [Command("hello")]
        public async Task Hello()
        {
            string css = "<style>\n    h1{\n        color: red;\n    }\n</style>\n";
            string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);
            var converter = new HtmlToImageConverter
            {
                Width = 250,
                Height = 70
            };
            var jpgBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");
        }

        [Command("myStats")]
        public async Task MyStats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();

            target = mentionedUser ?? Context.User;

            var profile = UserProfiles.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} has {profile.XP} XP and {profile.Points} points");
        }

        [Command("addXP")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddXP(uint xp)
        {
            var profile = UserProfiles.GetAccount(Context.User);
            profile.XP += xp; // Give XP
            UserProfiles.SaveProfiles(); // Save modification
            await Context.Channel.SendMessageAsync($"You gaind {xp} XP");
        }

        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            string user = Context.User.Username;
            string titlMsg = Utilities.GetFormattedAlert("ECHO_&NAME", user);
            var embed = new EmbedBuilder();
            embed.WithTitle(titlMsg);
            embed.WithDescription(message);
            embed.WithColor(new Color(255,73,167));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("choose")]
        public async Task ChooseOne([Remainder]string message)
        {
            string[] options = message.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Choice");
            embed.WithDescription(selection);
            embed.WithColor(new Color(255,73,167));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
            DataStorage.AddPairToStorage(Context.User.Username + DateTime.Now.ToLongDateString(), selection);
        }

        [Command("secret")]
        public async Task RevealSecret([Remainder] string arg = "")
        {
            if (!UserIsSecretOwner((SocketGuildUser)Context.User)) 
            {
                await Context.Channel.SendMessageAsync(":x: You need the SecretOwner role to do that command " + Context.User.Mention);
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET"));
        }

        private bool UserIsSecretOwner(SocketGuildUser user)
        {
            string targetRoleName = "SecretOwner";
            var result = from r in user.Guild.Roles
                        where r.Name == targetRoleName
                        select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }

        [Command("data")]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data Has " + DataStorage.GetPairsCount() + " pairs.");
        }
        */
    }
}