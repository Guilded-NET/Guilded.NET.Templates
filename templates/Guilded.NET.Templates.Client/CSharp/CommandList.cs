using Guilded.NET;
using Guilded.NET.Objects.Events;
using Guilded.NET.Objects.Chat;
using System.Collections.Generic;
using System.Linq;

namespace ProjectName {
    /// <summary>
    /// List of user bot commands.
    /// </summary>
    public static class CommandList {
        /// <summary>
        /// Responds with `Pong!`
        /// </summary>
        /// <param name="client">Client to post message with</param>
        /// <param name="messageCreated">Message creation event</param>
        /// <param name="command">Name of the command used</param>
        /// <param name="arguments">Command arguments</param>
        [Command("ping", "pong", Description = "Responds with `Pong!`")]
        public static async void Ping(BasicGuildedClient client, MessageCreatedEvent messageCreated, string command, IList<string> arguments) {
            // Sends a message to channel where `ping`/`pong` command was used
            await messageCreated.RespondAsync(
                // Generates a new message with content `Pong!`
                Message.Generate("Pong!")
            );
        }
        /// <summary>
        /// Shows a list of all bot commands.
        /// </summary>
        /// <param name="client">Client to post message with</param>
        /// <param name="messageCreated">Message creation event</param>
        /// <param name="command">Name of the command used</param>
        /// <param name="arguments">Command arguments</param>
        [Command("help", "commands", "commandlist", "command-list", Description = "Shows a list of commands.", Usage = "<command name>")]
        public static async void Help(BasicGuildedClient client, MessageCreatedEvent messageCreated, string command, IList<string> arguments) {
            // Gets all commands in the client
            var keys = client.CommandDictionary.Keys;
            // If there are 0 arguments, then give all commands
            if(arguments.Count == 0) {
                // Turn commands to string
                IEnumerable<string> com = keys.Select(x => $"**{x.Name}**: {x.Description}");
                // Generate embed for the command list
                Embed embed = new();
                embed.SetTitle("Command list");
                embed.SetDescription(string.Join("\n", com));
                embed.AddField("For more info", "Type `help command_name` to get more info about a command.");
                // Output the embed
                await messageCreated.RespondAsync(Message.Generate(embed));
            // If argument was given
            } else {
                // Command's name
                string commandname = arguments[0].Trim();
                // Find that command
                CommandAttribute com = keys.FirstOrDefault(x => x.Name == commandname || x.Alias.Contains(commandname));
                // If that command was not found
                if(com == null) {
                    await messageCreated.RespondAsync(Message.Generate("Could not find that command."));
                    return;
                }
                // If it was, then generate embed for it
                Embed embed = new();
                embed.SetTitle(com.Name);
                embed.SetDescription(com.Description);
                embed.AddField("Aliases", string.Join(", ", com.Alias), true);
                embed.AddField("Usage", $"{com.Name} {com.Usage}");
                // Output the embed
                await messageCreated.RespondAsync(Message.Generate(embed));
            }
        }
    }
}