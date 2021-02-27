namespace ProjectName

open System.Linq
open System.Collections.Generic

open Guilded.NET
open Guilded.NET.Objects.Chat
open Guilded.NET.Objects.Events
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
    static member Ping
        (
            client: BasicGuildedClient,
            messageCreated: MessageCreatedEvent,
            command: string,
            arguments: IList<string>
        ) =
        // Sends a message to channel where `ping`/`pong` command was used
        "Pong!"
        |> Message.Generate
        |> messageCreated.RespondAsync
        |> Async.AwaitTask
        |> ignore
    // <summary>
    /// Shows a list of all bot commands.
    /// </summary>
    /// <param name="client">Client to post message with</param>
    /// <param name="messageCreated">Message creation event</param>
    /// <param name="command">Name of the command used</param>
    /// <param name="arguments">Command arguments</param>
    [<Command("help",
              "commands",
              "commandlist",
              "command-list",
              Description = "Shows a list of commands.",
              Usage = "<command name>")>]
    static member Help
        (
            client: BasicGuildedClient,
            messageCreated: MessageCreatedEvent,
            command: string,
            arguments: IList<string>
        ) =
        // Gets all commands in the client
        let keys = client.CommandDictionary.Keys
        // If there are 0 arguments, then give all commands
        if arguments.Count = 0 then
            // Turn commands to string
            let com =
                seq {
                    for c in keys do
                        "**" + c.Name + "**: " + c.Description
                }
            // Generate embed for the command list
            let embed =
                Embed()
                    .SetTitle("Command list")
                    .SetDescription(com |> String.concat ", ")
                    .AddField("For more info", "Type `help command_name` to get more info about a command.")
            // Output the embed
            embed
            |> Message.Generate
            |> messageCreated.RespondAsync
            |> Async.AwaitTask
            |> ignore
        // If argument was given
        else
            // Command's name
            let commandName = arguments.[0].Trim()
            // Find that command
            let com =
                keys.FirstOrDefault
                    (fun x ->
                        x.Name = commandName
                        || x.Alias.Contains(commandName))
            // If that command was not found
            if isNull com then
                "Could not find that command."
                |> Message.Generate
                |> messageCreated.RespondAsync
                |> Async.AwaitTask
                |> ignore
            // If it was, then generate info for it
            else
                // Create an embed with command's info
                let embed =
                    Embed()
                        .SetTitle(com.Name)
                        .SetDescription(com.Description)
                        .AddField("Aliases", com.Alias |> String.concat ", ", true)
                        .AddField("Usage", com.Name + " " + com.Usage)
                // Output the embed
                embed
                |> Message.Generate
                |> messageCreated.RespondAsync
                |> Async.AwaitTask
                |> ignore
