﻿using System;
using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

namespace ProjectName
{
    /// <summary>
    /// User bot client program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates a new user bot client.
        /// </summary>
        /// <param name="args">Program arguments</param>
        static void Main()
        {
            // Read JSON "config/config.json"
            JObject config = JObject.Parse(File.ReadAllText("./config/config.json"));
            // Get login info
            string email = config["email"].Value<string>(),
                   password = config["password"].Value<string>(),
                   prefix = config["prefix"].Value<string>();
            // Tells us that it's starting with specific prefix
            Console.WriteLine($"Starting the bot with prefix '{prefix}'");
            // Creates new client
            using GuildedUserClient client = new(email, password, new GuildedClientConfig(prefix));
            // Fetches all commands from specific type
            client.FetchCommands(
                typeof(CommandList)
            );
            // When client connects to Guilded
            client.Connected += (o, e) => Console.WriteLine("Connected");
            // When client is ready
            client.Prepared += (o, e) => Console.WriteLine($"I successfully logged in!\n - ID: {client.Me.Id}\n - Name: {client.Me.Username}");
            // Start the bot
            StartAsync(client).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Makes bot connect to Guilded and then stops it from shutting down.
        /// </summary>
        /// <param name="client">Client to connect</param>
        /// <returns>Async task</returns>
        static async Task StartAsync(GuildedUserClient client)
        {
            // Connects to Guilded
            await client.ConnectAsync();
            // Makes it stop forever, so the bot wouldn't instantly shutdown after connecting
            await Task.Delay(-1);
        }
    }
}