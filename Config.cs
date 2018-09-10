using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Yazawa_Nico
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";
        
        public static BotConfig bot;

        static Config()
        {
            if(!Directory.Exists(configFolder)) Directory.CreateDirectory(configFolder);

            if (!File.Exists(configFolder + "/" + configFile))
            {
                // Create the botConfig file
                bot = new BotConfig();
                bot.botToken = "";
                bot.cmdPrefix = "!";
                bot.selfID = 0;
                bot.dblToken = "";
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                // Read the botConfig file
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }

        public struct BotConfig
        {
            public string botToken;
            public string cmdPrefix;

            public ulong selfID;

            public string dblToken;
        }
    }
}