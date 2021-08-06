using System;
using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

namespace ProjectName
{
    public class Program
    {
        /// <summary>
        /// Creates a new user bot client.
        /// </summary>
        /// <param name="args">Program arguments</param>
        static void Main()
        {
            // Gets all of the configuration from the file
            JObject config = JObject.Parse(File.ReadAllText("./config/config.json"));
            string token = config["token"].Value<string>(),
                   prefix = config["prefix"].Value<string>();
            // Creates new client
            using GuildedBotClient client = new(token);
            // When the client connects to Guilded, output "Connected"
            client.Connected += (o, e) => Console.WriteLine("Connected");
            // When the client is ready, output "I successfully logged in!"
            client.Prepared += (o, e) => Console.WriteLine($"I successfully logged in!");
            // Start the bot
            RunAsync(client).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Keeps the connection of the Guilded client.
        /// </summary>
        /// <param name="client">Client to keep running</param>
        static async Task RunAsync(GuildedBotClient client)
        {
            // Connects to Guilded
            await client.ConnectAsync();
            // Makes it stop forever, so the bot wouldn't instantly shutdown after connecting
            await Task.Delay(-1);
        }
    }
}