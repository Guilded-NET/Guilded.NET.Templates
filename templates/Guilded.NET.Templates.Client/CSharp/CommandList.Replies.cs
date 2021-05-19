// This is a list of all comment/reply commands
// NOTE: Make sure you add `Placement = CommandPlacement.Comments` when you want to make a reply command

using Guilded.NET;
using Guilded.NET.Objects.Chat;
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
        /// <param name="replyCreated">Reply/comment creation event</param>
        [Command("ping", "pong", Description = "Responds with `Pong!`", Placement = CommandPlacement.Comments)]
        public static async void PingReply(BasicGuildedClient client, CommandInfo info, ContentReplyCreatedEvent replyCreated)
        {
            // Sends a reply to the post where the command was invoked
            await replyCreated.ReplyAsync(
                // This ignores markdown.
                // `markdown-plain-text` only works with messages, not replies,
                // thus it's recommended to use nodes and leaves for content
                MessageContent.GenerateText("Pong!")
            );
        }
    }
}