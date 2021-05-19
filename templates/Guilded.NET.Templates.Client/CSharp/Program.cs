using System;
using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

using Serilog;
using Serilog.Core;

namespace ProjectName
{
    /// <summary>
    /// User bot client program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Logger for this bot.
        /// </summary>
        internal static readonly Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
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
            logger.Verbose("Starting the bot with prefix '{prefix}'", prefix);
            // Creates config for the client
            GuildedClientConfig clientConfig = new(
                // That always returns given prefix
                // Literally `=> prefix`
                // Change this if you want:
                // server-specific prefixes, group-specific prefixes or any other way to get a prefix from it
                GuildedClientConfig.BasicPrefix(prefix),
                // TODO: Change null to GId.Parse("yourId")
                null
            );
            // Creates new client
            using GuildedUserClient client = new(email, password, new GuildedClientConfig(prefix));
            // Fetches all commands from specific type
            client.FetchCommands(
                typeof(CommandList)
            );
            // When client connects to Guilded
            client.Connected += (o, e) => logger.Debug("Connected");
            // When client is ready
            client.Prepared += (o, e) => logger.Information(
                "I successfully logged in! - ID: {Id}\n - Name: {Username}\n - Server count: {Teams}",
                client.Me.Id,
                client.Me.Username,
                client.Me.TeamCount);
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