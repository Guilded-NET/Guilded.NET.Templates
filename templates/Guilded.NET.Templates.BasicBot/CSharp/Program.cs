using System;
using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

namespace ProjectName
{
    public static class Program
    {
        private static void Main()
        {
            // To get auth and prefix properties from config file
            JObject config = JObject.Parse(File.ReadAllText("./config/config.json"));

            string auth = config["auth"].Value<string>(),
                   prefix = config["prefix"].Value<string>();

            // Creates new client
            using GuildedBotClient client = new(auth);

            client.Connected += (o, e) => Console.WriteLine("Connected");
            client.Prepared += (o, e) => Console.WriteLine("I successfully logged in!");

            RunAsync(client).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private static async Task RunAsync(GuildedBotClient client)
        {
            await client.ConnectAsync().ConfigureAwait(false);

            // Don't close the program when the bot connects
            await Task.Delay(-1).ConfigureAwait(false);
        }
    }
}