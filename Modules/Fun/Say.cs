using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Yazawa_Nico.Modules.Images
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        [Command("say", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireContext(ContextType.Guild)]
        public async Task Sayd([Remainder] string content)
        {
            int x = 0;
            var messages = await (Context.Channel as ITextChannel).GetMessagesAsync(x + 1).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

            await ReplyAsync($"{content}");
        }

    }
}
