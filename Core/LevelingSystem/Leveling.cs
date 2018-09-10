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
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Core.LevelingSystem
{
    internal static class Leveling
    {
        internal static async void UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
            
            var userProfile = UserProfiles.UserProfiles.GetAccount(user);
            uint oldLevel = userProfile.LevelNumber;
            userProfile.XP += 1;
            UserProfiles.UserProfiles.SaveProfiles();
            uint newLevel = userProfile.LevelNumber;

            if (oldLevel != userProfile.LevelNumber)
            {
                SocketGuild target = null;
                target = user.Guild;
                var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

                if (guildProfile.LevelModule)
                {
                    // The user leveled up
                    var embed = new EmbedBuilder();
                    embed.WithColor(255,73,167);
                    embed.WithTitle("LEVEL UP!!!");
                    embed.WithDescription($"{user.Username} just leveled up to **{newLevel}**!");

                    await channel.SendMessageAsync($"{user.Username} just leveled up to **{newLevel}**");
                }

                
                
            }
            /*
            // If the user has a timeout, ignore them
            var pf = UserProfiles.UserProfiles.GetAccount(user);
            //Console.WriteLine("Leveling: " + pf.dailyB.ToString());
            if (pf.dailyB)
            {
                //Console.WriteLine("Leveling: " + pf.dailyB.ToString());
                RepeatingTimer.socketUsr = user;
                await RepeatingTimer.StartTimer();
                var userProfile = UserProfiles.UserProfiles.GetAccount(user);
                uint oldLevel = userProfile.LevelNumber;
                userProfile.XP += 10;
                UserProfiles.UserProfiles.SaveProfiles();
                uint newLevel = userProfile.LevelNumber;

                if (oldLevel != userProfile.LevelNumber)
                {
                    // The user leveled up
                        var embed = new EmbedBuilder();
                        embed.WithColor(255,73,167);
                        embed.WithTitle("LEVEL UP!!!");
                        embed.WithDescription($"{user.Username} just leveled up to **{newLevel}**!");

                        await channel.SendMessageAsync("", embed: embed.Build());
                    
                }
                pf.dailyB = false;
                //Console.WriteLine("Leveling: " + pf.dailyB.ToString());
                UserProfiles.UserProfiles.SaveProfiles();
            }
            */
            
            
        }
    }
}