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
    public static partial class CommandList
    {
        [Command("ping", "pong", Description = "Responds with `Pong!`")]
        public static async void Ping(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            await messageCreated.RespondAsync(
                Message.Generate("Pong!")
            );
        }
        [Command("help", "commands", "commandlist", "command-list", Description = "Shows a list of commands.", Usage = "<command name>")]
        public static async void Help(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            var keys = client.MessageCommands.Keys;
            var arg = info.Arguments.FirstOrDefault();
            if (info.Arguments.Count == 0 || arg?.Object != MsgObject.Leaf)
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
                string commandname = ((Leaf)arg).Text.Trim();
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
        [Command("userinfo", "user-info", "user", "u", Description = "Provides an information about mentioned user", Usage = "<user mention>")]
        public static async void UserInfo(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated)
        {
            var arg = info.Arguments.FirstOrDefault();
            if(arg == default || arg?.Object != MsgObject.Inline)
            {
                await messageCreated.RespondAsync(Message.Generate("Please mention user you want to get information of."));
                return;
            }
            Node node = (Node)arg;
            if(node.Type != NodeType.Mention)
            {
                await messageCreated.RespondAsync(Message.Generate("First argument must be a mention of a user"));
                return;
            }
            Mention mention = (Mention)node;
            if(mention.MentionData.Type != "person")
            {
                await messageCreated.RespondAsync(Message.Generate("First argument must be a mention of a user."));
                return;
            }
            GId id = GId.Parse(mention.MentionData.Id);
            ProfileUser user = await client.GetProfileAsync(id);
            Uri icon = user.ProfilePicture ?? MediaUtil.DefaultAvatars[0];
            Embed embed = new Embed()
                .SetAuthor(user.Username, icon, new Uri($"https://guilded.gg/profile/{id}"))
                .SetThumbnail(icon)
                .SetDescription(user?.About?.Bio ?? "No bio found")
                .SetFooter(user?.About?.TagLine ?? "No tagline found")
                .AddField("Registered", user.JoinDate.ToString("`dd`/`MM`/`yyyy` - `HH`:`mm`:`ss`"), true)
                .AddField("Last online", user.LastOnline.ToString("`dd`/`MM`/`yyyy` - `HH`:`mm`:`ss`"), true)
                .AddField("Flairs", $"[{string.Join(", ", user.Flairs.Select(x => x.Flair))}]");
            if(user.ProfileBannerLarge != null)
                embed.SetImage(user.ProfileBannerLarge);
            await messageCreated.RespondAsync(
                Message.Generate(embed)
            );
        }
    }
}