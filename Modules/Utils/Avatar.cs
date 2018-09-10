using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Yazawa_Nico.Modules.Utils
{
    public class Avatar : ModuleBase<SocketCommandContext>
    {
        string url;

        [Command("avatar", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task AvatarU()
        {
            url = Context.User.GetAvatarUrl().ToString();
            var embed = new EmbedBuilder()
                .WithAuthor(author => {
                    author
                        .WithName($"{Context.User.Username}#{Context.User.Discriminator}'s avatar")
                        .WithUrl($"{Context.User.GetAvatarUrl().ToString()}")
                        .WithIconUrl(Context.User.GetAvatarUrl());
                })
                .WithDescription($"[Direct Link]({url})")
                .WithFooter(footer =>
                {
                    footer
                        .WithText($"Requested by {Context.User.Username}#{Context.User.Discriminator}")
                        .WithIconUrl(Context.Client.GetUser(Context.User.Id).GetAvatarUrl());
                })
                .WithImageUrl(Context.User.GetAvatarUrl().ToString())
                .WithColor(new Color(222, 77, 156));

            await ReplyAsync("", false, embed.Build());
        }

        [Command("avatar", RunMode = RunMode.Async)]
        [Summary("Display the mentioned users avatar")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task AvatarUser(SocketGuildUser user)
        {
            url = Context.User.GetAvatarUrl().ToString();
            if (user != null)
            {
                url = user.GetAvatarUrl().ToString();
                var embed = new EmbedBuilder()
                    .WithAuthor(author => {
                        author
                            .WithName($"{user.Username}#{user.Discriminator}'s avatar")
                            .WithUrl($"{user.GetAvatarUrl().ToString()}")
                            .WithIconUrl(user.GetAvatarUrl());
                    })
                    .WithDescription($"[Direct Link]({url})")
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText($"Requested by {Context.User.Username}#{Context.User.Discriminator}")
                            .WithIconUrl(Context.Client.GetUser(Context.User.Id).GetAvatarUrl());
                    })
                    .WithImageUrl(user.GetAvatarUrl().ToString())
                    .WithColor(new Color(222, 77, 156));

                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                await ReplyAsync("You need to mention a user. `eg. !avatar @user`");
            }
        }
    }
}