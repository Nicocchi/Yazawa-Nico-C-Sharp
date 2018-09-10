using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using Newtonsoft.Json;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Modules.Info
{
    public class ServerInfo : ModuleBase<SocketCommandContext>
    {
        [Command("serverinfo", RunMode = RunMode.Async)]
        [Alias("sinfo", "servinfo")]
        [Remarks("Info about a server")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task GuildInfo()
        {
            EmbedBuilder embedBuilder;
            embedBuilder = new EmbedBuilder();
            embedBuilder.WithColor(new Color(222, 77, 156));

            var gld = Context.Guild as SocketGuild;
            var client = Context.Client as DiscordSocketClient;


            if (!string.IsNullOrWhiteSpace(gld.IconUrl))
                embedBuilder.ThumbnailUrl = gld.IconUrl;
            var O = gld.Owner.Username;

            var V = gld.VoiceRegionId;
            var C = gld.CreatedAt;
            var N = gld.DefaultMessageNotifications;
            var VL = gld.VerificationLevel;
            var XD = gld.Roles.Count;
            var X = gld.MemberCount;
            var Z = client.ConnectionState;

            var auth = new EmbedAuthorBuilder()

            {

                Name = gld.Name,
                IconUrl = gld.IconUrl,

            };

            var embed = new EmbedBuilder()
                .WithColor(new Color(222, 77, 156))
                .WithAuthor(auth)
                .WithTitle($"{gld.Name}")
                .WithThumbnailUrl(gld.IconUrl)
                .AddField("Name", $"{gld.Name}", true)
                .AddField("ID", $"{gld.Id}", true)
                .AddField("Owner", $"{O}", true)
                .AddField("Region", $"{V}", true)
                .AddField("Channels", $"{gld.Channels.Count}", true)
                .AddField("Members", $"{X}", true)
                .AddField("Roles", $"{XD}", true)
                .WithFooter($"Server created | {C}  | Requested by {Context.User.Username}");

            await ReplyAsync("", false, embed.Build());
        }
    }
}