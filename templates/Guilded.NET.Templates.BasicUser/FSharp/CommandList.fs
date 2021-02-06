namespace ProjectName
    open Guilded.NET
    open Guilded.NET.Objects.Chat
    open Guilded.NET.Objects.Events
    open System.Collections.Generic
    /// <summary>
    /// List of user bot commands.
    /// </summary>
    type public CommandList =
        /// <summary>
        /// Responds with `Pong!`
        /// </summary>
        /// <param name="client">Client to post message with</param>
        /// <param name="messageCreated">Message creation event</param>
        /// <param name="command">Name of the command used</param>
        /// <param name="arguments">Command arguments</param>
        [<Command("ping", "pong", Description = "Responds with `Pong!`")>]
        static member Ping(client: BasicGuildedClient, messageCreated: MessageCreatedEvent, command: string, arguments: IList<string>) =
            // Sends a message to channel where `ping`/`pong` command was use
            "Pong!"
                |> Message.Generate
                |> messageCreated.RespondAsync
                |> Async.AwaitTask