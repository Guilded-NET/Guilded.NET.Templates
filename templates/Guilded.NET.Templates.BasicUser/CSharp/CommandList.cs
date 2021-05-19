using Guilded.NET;
using Guilded.NET.Objects.Chat;
using Guilded.NET.Objects.Events;

namespace ProjectName {
    /// <summary>
    /// List of user bot commands.
    /// </summary>
    public static class CommandList {
        // NOTE: This is how message-based command looks like

        /// <summary>
        /// Responds with `Pong!`.
        /// </summary>
        /// <param name="client">Client that generated a command info and invoked this command method</param>
        /// <param name="info">Information about the command used, with arguments and other things</param>
        /// <param name="messageCreated">Message creation event</param>
        [Command("ping", "pong", Description = "Responds with `Pong!`")]
        public static async void Ping(BasicGuildedClient client, CommandInfo info, MessageCreatedEvent messageCreated) {
            // Sends a message to channel where `ping`/`pong` command was used
            await messageCreated.RespondAsync(
                // Generates a new message with content `Pong!`
                Message.Generate("Pong!")
            );
        }

        // NOTE: This is how forum/comment-based command looks like

        /// <summary>
        /// Responds with `Pong!`.
        /// </summary>
        /// <param name="client">Client that generated a command info and invoked this command method</param>
        /// <param name="info">Information about the command used, with arguments and other things</param>
        /// <param name="replyCreated">Reply/comment creation event</param>
        [Command("ping", "pong", Description = "Responds with `Pong!`", Placement = CommandPlacement.Comments)]
        public static async void PingReply(BasicGuildedClient client, CommandInfo info, ContentReplyCreatedEvent replyCreated) {
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