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
    public class BotInfo : ModuleBase<SocketCommandContext>
    {
        [Command("botinfo", RunMode = RunMode.Async)]
        [Summary("Display bot information")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task BotInformation()
        {
            using (var process = Process.GetCurrentProcess())
            {
                var embed = new EmbedBuilder();
                var application = await Context.Client.GetApplicationInfoAsync(); //for lib version
                embed.WithColor(new Color(222, 77, 156));
                embed.WithTitle(application.Name);
                embed.WithFooter(footer =>
                {
                    footer
                        .WithText($"{Context.Client.CurrentUser.Username}")
                        .WithIconUrl(Context.Client.GetUser(Context.Client.CurrentUser.Id).GetAvatarUrl());
                });
                embed.AddField("Creator","Nicocchi#2525", false)
                .AddField(y =>
                {
                    y.Name = "Stats";
                    y.Value = (Context.Client as DiscordSocketClient).Guilds.Count.ToString() + " servers\n" +
                        (Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count).ToString() + " channels\n" +
                        (Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count).ToString() + " users";
                    y.IsInline = true;
                })
                .AddField(y =>
                {
                    y.Name = "Uptime";
                    var time = DateTime.Now - process.StartTime;
                    var sb = new StringBuilder();
                    if (time.Days > 0)
                    {
                        sb.Append($"{time.Days}d ");
                    }
                    if (time.Hours > 0)
                    {
                        sb.Append($"{time.Hours}h ");
                    }
                    if (time.Minutes > 0)
                    {
                        sb.Append($"{time.Minutes}m ");
                    }
                    sb.Append($"{time.Seconds}s ");
                    y.Value = sb.ToString();
                    y.IsInline = true;
                })
                .AddField(y =>
                {
                    y.Name = "Version";
                    y.Value = "2.0";
                    y.IsInline = true;
                })
                .AddField(y =>
                {
                    y.Name = "Library";
                    y.Value = DiscordConfig.Version;
                    y.IsInline = true;
                })
                .AddField(y =>
                {
                    y.Name = "Support server";
                    y.Value = "https://discord.gg/uSaxHgr";
                    y.IsInline = true;
                });

                await this.ReplyAsync("", embed: embed.Build());
            }
        }
    }
}