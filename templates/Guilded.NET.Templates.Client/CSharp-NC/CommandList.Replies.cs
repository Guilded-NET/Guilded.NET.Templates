// This is a list of all comment/reply commands
// NOTE: Make sure you add `Placement = CommandPlacement.Comments` when you want to make a reply command

using Guilded.NET;
using Guilded.NET.Objects.Chat;
using Guilded.NET.Objects.Events;

namespace ProjectName
{
    public static partial class CommandList
    {
        [Command("ping", "pong", Description = "Responds with `Pong!`", Placement = CommandPlacement.Comments)]
        public static async void PingReply(BasicGuildedClient client, CommandInfo info, ContentReplyCreatedEvent replyCreated)
        {
            await replyCreated.ReplyAsync(
                MessageContent.GenerateText("Pong!")
            );
        }
    }
}