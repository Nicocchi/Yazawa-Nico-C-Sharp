using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;
using Yazawa_Nico.Core;

namespace Yazawa_Nico.Core
{
    public static class DataStorage
    {
        // Save all userProfiles
        public static void SaveUserProfiles(IEnumerable<UserProfile> profiles, string filePath)
        {
            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Get all userProfiles
        public static IEnumerable<UserProfile> LoadUserProfiles(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserProfile>>(json);
        }

        // Save all guildProfiles
        public static void SaveGuildProfiles(IEnumerable<GuildProfile> profiles, string filePath)
        {
            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Get all guildProfiles
        public static IEnumerable<GuildProfile> LoadGuildProfiles(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuildProfile>>(json);
        }

        // Check if save exists
        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void UpdateGlobals(Globals.GlobalConfig globalConfig)
        {
            const string globalFolder = "Resources";
            const string globalFile = "globals.json";

            string json = JsonConvert.SerializeObject(globalConfig, Formatting.Indented);
            File.WriteAllText(globalFolder + "/" + globalFile, json);
            Console.Write(json);

            string json1 = File.ReadAllText(globalFolder + "/" + globalFile);
            var json2 = JsonConvert.DeserializeObject<Globals.GlobalConfig>(json1);
            Console.Write(json1);
        }

    }
}