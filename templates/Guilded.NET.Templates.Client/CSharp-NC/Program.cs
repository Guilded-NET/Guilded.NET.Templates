using System;
using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

namespace ProjectName
{
    public class Program
    {
        static void Main()
        {
            JObject config = JObject.Parse(File.ReadAllText("./config/config.json"));
            string email = config["email"].Value<string>(),
                   password = config["password"].Value<string>(),
                   prefix = config["prefix"].Value<string>();
            Console.WriteLine($"Starting the bot with prefix '{prefix}'");
            GuildedClientConfig clientConfig = new(
                GuildedClientConfig.BasicPrefix(prefix),
                null
            );
            using GuildedUserClient client = new(email, password, clientConfig);
            client.FetchCommands(
                typeof(CommandList)
            );
            client.Connected += (o, e) => Console.WriteLine("Connected");
            client.Prepared += (o, e) => Console.WriteLine($"I successfully logged in!\n - ID: {client.Me.Id}\n - Name: {client.Me.Username}");
            StartAsync(client).GetAwaiter().GetResult();
        }
        static async Task StartAsync(GuildedUserClient client)
        {
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}