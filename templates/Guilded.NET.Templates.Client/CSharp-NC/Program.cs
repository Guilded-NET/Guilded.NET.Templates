using System.IO;
using System.Threading.Tasks;

using Guilded.NET;

using Newtonsoft.Json.Linq;

using Serilog;
using Serilog.Core;

namespace ProjectName
{
    public class Program
    {
        internal static readonly Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
        static void Main()
        {
            JObject config = JObject.Parse(File.ReadAllText("./config/config.json"));
            string email = config["email"].Value<string>(),
                   password = config["password"].Value<string>(),
                   prefix = config["prefix"].Value<string>();
            logger.Verbose("Starting the bot with prefix '{prefix}'", prefix);
            GuildedClientConfig clientConfig = new(
                GuildedClientConfig.BasicPrefix(prefix),
                // TODO: Change null to GId.Parse("yourId")
                null
            );
            using GuildedUserClient client = new(email, password, new GuildedClientConfig(prefix));
            client.FetchCommands(
                typeof(CommandList)
            );
            client.Connected += (o, e) => logger.Debug("Connected");
            client.Prepared += (o, e) => logger.Information(
                "I successfully logged in! - ID: {Id}\n - Name: {Username}\n - Server count: {Teams}",
                client.Me.Id,
                client.Me.Username,
                client.Me.TeamCount);
            StartAsync(client).GetAwaiter().GetResult();
        }
        static async Task StartAsync(GuildedUserClient client)
        {
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}