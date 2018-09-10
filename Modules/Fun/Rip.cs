using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Yazawa_Nico.Modules.Fun
{
    public class Rip : ModuleBase<SocketCommandContext>
    {
        [Command("f", RunMode = RunMode.Async)]
        [Alias("rip", "ded")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DefaultRip([Remainder]string msg = "")
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("");
            builder.WithColor(new Color(255,73,167));
            if (msg != "")
            {
                builder.AddField("RIP", $"**{Context.User.Username}** has paid their respects for {msg}");
            }
            else
            {
                builder.AddField("RIP", $"**{Context.User.Username}** has paid their respects.");
            }
            
            //builder.WithDescription($"{today} today, {globalRip} All");
            await ReplyAsync("", false, builder.Build());
        }
    }
}
