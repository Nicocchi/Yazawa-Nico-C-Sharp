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

namespace Yazawa_Nico.Modules.Fun
{
    public class Choose : ModuleBase<SocketCommandContext>
    {
        [Command("choose")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ChooseOne([Remainder]string message)
        {
            string[] options = message.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            await Context.Channel.SendMessageAsync($"I choose **{selection}**");
        }
    }
}