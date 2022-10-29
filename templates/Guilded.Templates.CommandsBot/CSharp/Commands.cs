// Check out this guide if you want to get started with commands:
// https://guilded-net.github.io/docs/commands
using Guilded.Commands;

namespace ProjectName;

public partial class BotCommands : CommandModule
{
    [Command(Aliases = new string[] { "p" })]
    public static Task Ping(CommandEvent invokation) =>
        invokation.ReplyAsync("Pong!");

    [Command(Aliases = new string[] { "commands", "h" })]
    public Task Help(CommandEvent invokation)
    {
        var commandNames = CommandLookup.Select(group => group.Key);

        return invokation.ReplyAsync($"Here are available commands: `{string.Join("`, `", commandNames)}`");
    }

    [Description("This does stuff.")]
    [Command(Aliases = new string[] { "ex", "e" } )]
    [Example("10"), Example("ex", "50")]
    public async Task Example(CommandEvent invokation, [CommandParam("number to say")] int number)
    {
        await invokation.ReplyAsync($"Someone secretly said number `{number}`");

        // Delete the command message ("/example 10")
        await invokation.DeleteAsync();
    }
}