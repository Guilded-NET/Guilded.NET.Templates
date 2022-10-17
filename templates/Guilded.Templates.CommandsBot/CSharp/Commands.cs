using Guilded.Commands;

namespace ProjectName;

public partial class BotCommands : CommandModule
{
    [Command(Aliases = new string[] { "p" })]
    public static async Task Ping(CommandEvent invokation) =>
        await invokation.ReplyAsync("Pong!");

    [Command(Aliases = new string[] { "commands", "h" })]
    public async Task Help(CommandEvent invokation)
    {
        var commandNames = CommandLookup.Select(group => group.Key);

        await invokation.ReplyAsync($"Here are available commands: `{string.Join("`, `", commandNames)}`");
    }
}