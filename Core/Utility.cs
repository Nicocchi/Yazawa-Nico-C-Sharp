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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Yazawa_Nico.Modules.Core
{
    public class Utility
    {
        public static string LoadJson(string jsonFile)
        {
            Random rand = new Random();

            string fileLoc = Path.Combine(AppContext.BaseDirectory, jsonFile);

            // Load and Parse the JSON file
            JObject o1 = JObject.Parse(File.ReadAllText(fileLoc));

            // Read JSON directly from a file
            using (StreamReader file = File.OpenText(fileLoc))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                JArray links = (JArray)o1["file"]; // Turn the string into an string[]
                IList<string> linksText = links.Select(c => (string)c).ToList(); // Convert string into a List<string>

                string[] predictionsTexts = linksText.ToArray(); // Convert to new array
                int randomIndex = rand.Next(predictionsTexts.Length); // Get a random int and select an index from the predictionsText array
                return predictionsTexts[randomIndex]; // Set the imageUrl to the link
            }
        }
    }
}