// This is a list of all chat/messages commands

using System;
using System.Linq;
using System.Collections.Generic;

using Guilded.NET;
using Guilded.NET.Util;
using Guilded.NET.Objects;
using Guilded.NET.Objects.Chat;
using Guilded.NET.Objects.Users;
using Guilded.NET.Objects.Events;

namespace ProjectName
{
    /// <summary>
    /// List of user bot commands.
    /// </summary>
    public static partial class CommandList
    {
        /// <summary>
        /// Responds with `Pong!`.
        /// </summary>
        /// <param name="client">Client that generated a command info and invoked this command method</param>
        /// <param name="info">Information about the command used, with arguments and other things</param>
        /// <param name="messageCreated">Message creation event</param>
        [Command("ping", "pong", Description = "Responds with `Pong!`")]
        public static async void Ping(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            // Sends a message to channel where `ping`/`pong` command was used
            await messageCreated.RespondAsync(
                // Generates a new message with content `Pong!`
                Message.Generate("Pong!")
            );
        }
        /// <summary>
        /// Shows a list of commands.
        /// </summary>
        /// <param name="client">Client that generated a command info and invoked this command method</param>
        /// <param name="info">Information about the command used, with arguments and other things</param>
        /// <param name="messageCreated">Message creation event</param>
        [Command("help", "commands", "commandlist", "command-list", Description = "Shows a list of commands.", Usage = "<command name>")]
        public static async void Help(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            // Gets all commands in the client
            var keys = client.MessageCommands.Keys;
            // Gets first argument or default
            var arg = info.Arguments.FirstOrDefault();
            // If there are 0 arguments or argument isn't text/leaf argument, then give all commands
            if (info.Arguments.Count == 0 || arg?.Object != MsgObject.Leaf)
            {
                // Turn commands to string
                IEnumerable<string> com = keys.Select(x => $"**{x.Name}**: {x.Description}");
                // Generate embed for the command list
                Embed embed = new Embed().
                    SetTitle("Command list").
                    SetDescription(string.Join("\n", com)).
                    AddField("For more info", "Type `help command_name` to get more info about a command.");
                // Output the embed
                await messageCreated.RespondAsync(Message.Generate(embed));
            }
            // If the argument was given and is a leaf
            else
            {
                // Command's name
                string commandname = ((Leaf)arg).Text.Trim();
                // Find that command
                CommandAttribute com = keys.FirstOrDefault(x => x.Name == commandname || x.Alias.Contains(commandname));
                // If that command was not found
                if (com == null)
                {
                    await messageCreated.RespondAsync(Message.Generate("Could not find that command."));
                    return;
                }
                // If it was, then generate embed for it
                Embed embed = new Embed().
                    SetTitle(com.Name).
                    SetDescription(com.Description).
                    AddField("Aliases", string.Join(", ", com.Alias), true).
                    AddField("Usage", $"{com.Name} {com.Usage}");
                // Output the embed
                await messageCreated.RespondAsync(Message.Generate(embed));
            }
        }
        /// <summary>
        /// Gets information about user.
        /// </summary>
        /// <param name="client">Client that generated a command info and invoked this command method</param>
        /// <param name="info">Information about the command used, with arguments and other things</param>
        /// <param name="messageCreated">Message creation event</param>
        [Command("userinfo", "user-info", "user", "u", Description = "Provides an information about mentioned user", Usage = "<user mention>")]
        public static async void UserInfo(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            // Gets first argument
            var arg = info.Arguments.FirstOrDefault();
            // If it's empty or isn't an inline, error
            if(arg == default || arg?.Object != MsgObject.Inline)
            {
                await messageCreated.RespondAsync(Message.Generate("Please mention user you want to get information of."));
                return;
            }
            // Gets it as node
            Node node = (Node)arg;
            // Checks if it's a mention node
            if(node.Type != NodeType.Mention)
            {
                await messageCreated.RespondAsync(Message.Generate("First argument must be a mention of a user"));
                return;
            }
            // Gets it as a mention
            Mention mention = (Mention)node;
            // Checks if it's user mention
            if(mention.MentionData.Type != "person")
            {
                await messageCreated.RespondAsync(Message.Generate("First argument must be a mention of a user."));
                return;
            }
            // Gets ID of the user
            GId id = GId.Parse(mention.MentionData.Id);
            // Gets user's profile by that ID
            ProfileUser user = await client.GetProfileAsync(id);
            // Gets icon of the user
            Uri icon = user.ProfilePicture ?? MediaUtil.DefaultAvatars[0];
            // Generates embed for it
            Embed embed = new Embed()
                .SetAuthor(user.Username, icon, new Uri($"https://guilded.gg/profile/{id}"))
                .SetThumbnail(icon)
                .SetDescription(user?.About?.Bio ?? "No bio found")
                .SetFooter(user?.About?.TagLine ?? "No tagline found")
                .AddField("Registered", user.JoinDate.ToString("`dd`/`MM`/`yyyy` - `HH`:`mm`:`ss`"), true)
                .AddField("Last online", user.LastOnline.ToString("`dd`/`MM`/`yyyy` - `HH`:`mm`:`ss`"), true)
                .AddField("Flairs", $"[{string.Join(", ", user.Flairs.Select(x => x.Flair))}]");
            // Checks if user has a banner and adds it as image
            if(user.ProfileBannerLarge != null)
                embed.SetImage(user.ProfileBannerLarge);
            // Sends the embed
            await messageCreated.RespondAsync(
                Message.Generate(embed)
            );
        }
    }
}