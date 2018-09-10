using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Yazawa_Nico.Modules.Games
{
    public class EightBall : ModuleBase<SocketCommandContext>
    {
        string[] predictionsTexts = new string[]
        {
            "It is very unlikely",
            "I don't think so...",
            "Yes !",
            "I don't know",
            "No.",
            "Most likely",
            "N-ni... Nico Nii ~",
            "It's fate!"
        };

        Random rand = new Random();

        [Command("8ball", RunMode = RunMode.Async)]
        [Summary("Gives a prediction")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task EightBallA([Remainder] string input)
        {
            int randomIndex = rand.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            await ReplyAsync(Context.User.Mention + ", " + text);
        }
    }
}