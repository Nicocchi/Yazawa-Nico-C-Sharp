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

namespace Yazawa_Nico.Modules.Currency
{
    public class Daily : ModuleBase<SocketCommandContext>
    {
        [Command("daily")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DailyA()
        {
            DateTime now = DateTime.Now;
            var userPf = UserProfiles.GetAccount(Context.User);
            DateTime daily = userPf.daily;
            int difference = DateTime.Compare(daily, now);

            if ((userPf.daily.ToString() == "0001-01-01 00:00:00") || (daily.DayOfYear < now.DayOfYear && difference < 0 || difference >= 0))
            {
                userPf.daily = DateTime.Now;
                userPf.Points += 200;
                UserProfiles.SaveProfiles();
                await Context.Channel.SendMessageAsync($":white_check_mark: {Context.User.Username}, you claimed 200 Nico Nii's!");
            }
            else
            {
                TimeSpan diff = now - daily; // This line compute the difference of time between the two dates

                // This line prevents "Your credits refresh in 00:18:57.0072170 !"
                TimeSpan di = new TimeSpan(23 - diff.Hours, 60 - diff.Minutes, 60 - diff.Seconds);
                string str = di.ToString(@"hh\:mm\:ss");

                await ReplyAsync($":negative_squared_cross_mark: {Context.User.Username}, You have already claimed your daily Nico Nii's~\n\nThey refresh in {di.Days} day(s) {di.Hours} hour(s) {di.Minutes} min(s) and {di.Seconds} second(s)!");
            }
        }
    }
}