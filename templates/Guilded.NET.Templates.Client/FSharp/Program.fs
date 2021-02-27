namespace ProjectName

open System.IO
open System.Threading.Tasks

open Guilded.NET

open Newtonsoft.Json.Linq
/// <summary>
/// User bot client program.
/// </summary>
module Program =
    /// <summary>
    /// Makes bot connect to Guilded and then stops it from shutting down.
    /// </summary>
    /// <param name="client">Client to connect</param>
    let startAsync (client: GuildedUserClient) =
        // Connects to Guilded
        client.ConnectAsync() |> Async.AwaitTask |> Async.Ignore |> ignore
        // Makes it stop forever
        -1 |> Task.Delay |> Async.AwaitTask |> Async.Ignore
    /// <summary>
    /// Creates a new user bot client.
    /// </summary>
    /// <param name="_">Program arguments</param>
    [<EntryPoint>]
    let main _ =
        // Read JSON "config/config.json"
        let config =
            JObject.Parse(File.ReadAllText "./config/config.json")
        // Get login info
        let email = config.["email"].Value<string>()
        let password = config.["password"].Value<string>()
        let prefix = config.["prefix"].Value<string>()
        // Tells us that it's starting with specific prefix
        printfn "Starting the bot with prefix %s" prefix
        // Creates config for the client
        let clientConfig =
            GuildedClientConfig(
                // Always returns the given prefix
                // Literally `=> prefix`
                // Change this if you want:
                // server-specific prefixes, group-specific prefixes or any other way to get a prefix from it
                prefix |> GuildedClientConfig.BasicPrefix,
                null
            )
        // Creates new client
        use client =
            new GuildedUserClient(email, password, clientConfig)
        // Fetches all commands from specific type
        client.FetchCommands typeof<ProjectName.CommandList>
        // When client connects to Guilded
        client.Connected.Add(fun arg -> printfn "Connected")
        // When client is prepared
        client.Prepared.Add
            (fun arg -> printfn "I successfully logged in!\n - ID: %O\n - Name: %s" client.Me.Id client.Me.Username)
        // Start the bot
        client |> startAsync |> Async.RunSynchronously
        // End
        0
