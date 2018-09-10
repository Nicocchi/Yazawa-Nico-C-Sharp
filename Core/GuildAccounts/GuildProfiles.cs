using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace Yazawa_Nico.Core.GuildAccounts
{
    public static class GuildProfiles
    {
        private static List<GuildProfile> profiles;

        private static string profilesFile = "Resources/guildProfiles.json";

        static GuildProfiles()
        {
            if (DataStorage.SaveExists(profilesFile))
            {
                profiles = DataStorage.LoadGuildProfiles(profilesFile).ToList();
            }
            else
            {
                profiles = new List<GuildProfile>();
                SaveProfiles();
            }
        }

        // Save guild profiles
        public static void SaveProfiles()
        {
            DataStorage.SaveGuildProfiles(profiles, profilesFile);
        }

        // Get the guild profile
        public static GuildProfile GetAccount(SocketGuild guild)
        {
            return GetorCreateProfile(guild.Id);
        }

        // Searches through the profiles and returns the profile and creates a new profile if profile does not exist
        private static GuildProfile GetorCreateProfile(ulong id)
        {
            var result = from p in profiles
                        where p.ID == id
                        select p;

            var profile = result.FirstOrDefault();
            if (profile == null) profile = CreateGuildProfile(id);
            return profile;
        }

        private static GuildProfile CreateGuildProfile(ulong id)
        {
            var newProfile = new GuildProfile()
            {
                ID = id,
                NumberofWarnings = 3,
                GreetMessage = "Welcome @user!",
                LeaveMessage = "Sorry to see you leave @user...",
                GreetModule = false,
                LeaveModule = false,
                GreetChannel = 0,
                LeaveChannel = 0,
                WarningsBan = false,
                WarningsKick = true,
                WarningsMute = false,
                BanModule = true,
                KickModule = true,
                MuteModule = true,
                WarnModule = true,
                AfkModule = true,
                PurgeModule = true,
                LevelModule = true,
                CmdPrefixDefault = "!",
                CmdPrefix = "!"

            };

            profiles.Add(newProfile);
            SaveProfiles();
            return newProfile;
        }
    }
}