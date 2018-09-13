using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Yazawa_Nico.Core 
{
    public class Globals
    {
        private const string globalFolder = "Resources";
        private const string globalFile = "globals.json";
        public static GlobalConfig globalConfig;

        static Globals()
        {
            if(!Directory.Exists(globalFolder)) Directory.CreateDirectory(globalFolder);

            CreateFile();
        }

        public static void CreateFile() {
            if (!File.Exists(globalFolder + "/" + globalFile))
            {
                // Create the GlobalConfig file
                globalConfig = new GlobalConfig();
                globalConfig.currentDate = DateTime.UtcNow.Date;
                globalConfig.totalRips = 0;
                globalConfig.todaysRips = 0;
                string json = JsonConvert.SerializeObject(globalConfig, Formatting.Indented);
                File.WriteAllText(globalFolder + "/" + globalFile, json);
            }
            else
            {
                // Read the GlobalConfig file
                string json = File.ReadAllText(globalFolder + "/" + globalFile);
                globalConfig = JsonConvert.DeserializeObject<GlobalConfig>(json);
            }
        }

        public struct GlobalConfig
        {
            public DateTime currentDate;
            public uint totalRips;
            public uint todaysRips;
        }
    }
}