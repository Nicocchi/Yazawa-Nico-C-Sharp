using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yazawa_Nico.Core.UserProfiles;

namespace Yazawa_Nico.Modules.Games
{
    public class RPS : ModuleBase<SocketCommandContext>
    {
        [Command("rps", RunMode = RunMode.Async)]
        [Summary("rps | Play a game of RPS")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Default(uint amount = 0)
        {
            var user = UserProfiles.GetAccount(Context.User);
            if (!user.IsRPS)
            {
                if (user.Points >= amount)
                {
                    var embed = new EmbedBuilder()
                    .WithColor(new Color(255,73,167))
                    .AddField("Let's play a game", "Choose between **rock**, **paper** or **scissors**.\n\n\"((≡|≡))_／ ＼_((≡|≡) )\"");
                    await ReplyAsync("", false, embed.Build());
                    
                    if (amount > 0)
                    {
                        user.IsRPSGamble = true;
                        user.GambleAmount = amount;
                        user.Points -= amount;
                    }
                    user.IsRPS = true;
                    UserProfiles.SaveProfiles();
                }
                else
                {
                    await ReplyAsync($":negative_squared_cross_mark: {Context.User.Username}, you do not have enough Nico Nii's to play.");
                }
                
            }
            else
            {
                await ReplyAsync($":negative_squared_cross_mark: | {Context.User.Username}, you are already playing RPS!");
            }
            
        }
    }
}