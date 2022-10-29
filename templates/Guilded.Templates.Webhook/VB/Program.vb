Imports System
Imports Guilded.Webhook

Module Program
    Sub Main(args As String())
        MainAsync().GetAwaiter().GetResult()
    End Sub

    Async Function MainAsync() As Task
        Using webhookClient As New GuildedWebhookClient("...url here...")
            Await (webhookClient.CreateMessageAsync("Guilded.NET webhook client works!"))
        End Using
    End Function
End Module
