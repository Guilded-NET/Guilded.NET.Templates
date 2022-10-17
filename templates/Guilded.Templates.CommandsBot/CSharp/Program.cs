using System.Reactive.Linq;
using Guilded;
using Guilded.Commands;
using Newtonsoft.Json.Linq;
using ProjectName;

// Get the configuration values
JObject config = JObject.Parse(await File.ReadAllTextAsync("./config/config.json"));

string auth = config.Value<string>("auth")!,
       globalPrefix = config.Value<string>("prefix")!;

await using var client = new GuildedBotClient(auth).AddCommands(new BotCommands(), globalPrefix);

client.Prepared
      .Subscribe(me =>
          Console.WriteLine("The bot is prepared!\nLogged in as \"{0}\" with the ID \"{1}\"", me.Name, me.Id)
      );

// Wait for !ping messages
client.MessageCreated
    .Where(msgCreated => msgCreated.Content == globalPrefix + "ping")
    .Subscribe(async msgCreated =>
        await msgCreated.ReplyAsync("Pong!")
    );

await client.ConnectAsync();

// Don't close the program when the bot connects
await Task.Delay(-1);