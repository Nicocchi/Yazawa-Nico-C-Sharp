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
    public class UserInfo : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Display the targeted user's profile
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Command("profile")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task UserProfileInfo([Remainder]string users = null)
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var marriages = UserProfiles.GetMarriages(target);
            var xp = UserProfiles.GetXP(target);
            var level = UserProfiles.GetLevel(target);
            var currency = UserProfiles.GetCurrency(target);
            var id = UserProfiles.GetId(target);

            var joinAt = Context.Guild.CurrentUser.JoinedAt;
            
            var userPf = UserProfiles.GetAccount(target);
            // Set username in profile for updated username
            userPf.Username = target.Username;
            UserProfiles.SaveProfiles();

            var embed = new EmbedBuilder()
                .WithColor(255,73,167)
                .WithThumbnailUrl(target.GetAvatarUrl())
                .WithAuthor($"{target.Username}'s Global Profile")
                .AddField("Level & EXP", level + " [" + level + "/" + xp + "]", true)
                .AddField("Nico Nii's", $"{currency} <:niconii:451873490883313664>", true)
                .AddField("ID", id)
                .AddField("Marriages <:nicolove:451873491130646528>", marriages, true)
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Server Member since {joinAt}");
                });
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}