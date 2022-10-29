open Guilded.Webhook

[<EntryPoint>]
let main argv =
    async {
        let webhookClient = new GuildedWebhookClient("...url here...")

        "Guilded.NET webhook client works!"
            |> webhookClient.CreateMessageAsync
            |> Async.AwaitTask
            |> ignore
    } |> Async.RunSynchronously

    0