namespace ProjectName

open Guilded.NET
open Guilded.NET.Objects.Chat
open Guilded.NET.Objects.Events
/// <summary>
/// List of user bot commands.
/// </summary>
type public CommandList =
    // NOTE: This is how message-based command looks like

    /// <summary>
    /// Responds with `Pong!`.
    /// </summary>
    /// <param name="client">Client that generated a command info and invoked this command method</param>
    /// <param name="info">Information about the command used, with arguments and other things</param>
    /// <param name="messageCreated">Message creation event</param>
    [<Command("ping", "pong", Description = "Responds with `Pong!`")>]
    static member Ping
        (
            client: BasicGuildedClient,
            info: CommandInfo,
            messageCreated: MessageCreatedEvent
        ) =
        // Sends a message to channel where `ping`/`pong` command was used
        "Pong!"
        |> Message.Generate
        |> messageCreated.RespondAsync
        |> Async.AwaitTask
        |> ignore

    // NOTE: This is how forum/comment-based command looks like

    /// <summary>
    /// Responds with `Pong!`.
    /// </summary>
    /// <param name="client">Client that generated a command info and invoked this command method</param>
    /// <param name="info">Information about the command used, with arguments and other things</param>
    /// <param name="replyCreated">Reply/comment creation event</param>
    [<Command("ping", "pong", Description = "Responds with `Pong!`", Placement = CommandPlacement.Comments)>]
    static member PingComment
        (
            client: BasicGuildedClient,
            info: CommandInfo,
            replyCreated: ContentReplyCreatedEvent
        ) =
        // Sends a message to channel where `ping`/`pong` command was used
        "Pong!"
        |> MessageContent.GenerateText // Markdown won't work because of this. Markdown-plain-text is not supported in comments/replies
        |> replyCreated.ReplyAsync
        |> Async.AwaitTask
        |> ignore
