using System.Collections.Generic;
using System.Linq;

using Guilded.NET;
using Guilded.NET.Objects.Chat;
using Guilded.NET.Objects.Events;

namespace ProjectName
{
    public static class CommandList
    {
        [Command("ping", "pong", Description = "Responds with `Pong!`")]
        public static async void Ping(BasicGuildedClient client, MessageCreatedEvent messageCreated, string command, IList<string> arguments)
        {
            await messageCreated.RespondAsync(
                Message.Generate("Pong!")
            );
        }
        [Command("help", "commands", "commandlist", "command-list", Description = "Shows a list of commands.", Usage = "<command name>")]
        public static async void Help(BasicGuildedClient client, MessageCreatedEvent messageCreated, string command, IList<string> arguments)
        {
            var keys = client.CommandDictionary.Keys;
            if (arguments.Count == 0)
            {
                IEnumerable<string> com = keys.Select(x => $"**{x.Name}**: {x.Description}");
                Embed embed = new Embed().
                    SetTitle("Command list").
                    SetDescription(string.Join("\n", com)).
                    AddField("For more info", "Type `help command_name` to get more info about a command.");
                await messageCreated.RespondAsync(Message.Generate(embed));
            }
            else
            {
                string commandname = arguments[0].Trim();
                CommandAttribute com = keys.FirstOrDefault(x => x.Name == commandname || x.Alias.Contains(commandname));
                if (com == null)
                {
                    await messageCreated.RespondAsync(Message.Generate("Could not find that command."));
                    return;
                }
                Embed embed = new Embed().
                    SetTitle(com.Name).
                    SetDescription(com.Description).
                    AddField("Aliases", string.Join(", ", com.Alias), true).
                    AddField("Usage", $"{com.Name} {com.Usage}");
                await messageCreated.RespondAsync(Message.Generate(embed));
            }
        }
    }
}