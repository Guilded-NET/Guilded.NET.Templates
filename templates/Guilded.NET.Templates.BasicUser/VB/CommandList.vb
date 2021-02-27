Imports System.Collections.Generic

Imports Guilded.NET
Imports Guilded.NET.Objects.Chat
Imports Guilded.NET.Objects.Events

Namespace ProjectName
    ''' <summary>
    ''' List of user bot commands.
    ''' </summary>
    Public Module CommandList
        ''' <summary>
        ''' Responds with `Pong!`
        ''' </summary>
        ''' <param name="client">Client to post message with</param>
        ''' <param name="messageCreated">Message creation event</param>
        ''' <param name="command">Name of the command used</param>
        ''' <param name="arguments">Command arguments</param>
        <Command("ping", "pong", Description := "Responds with `Pong!`")>
        Public Async Sub Ping(client As BasicGuildedClient, messageCreated As MessageCreatedEvent, content As String, arguments As IList(Of String))
            ' Sends a message to channel where `ping`/`pong` command was used
            ' Generates a new message with content `Pong!`
            Await messageCreated.RespondAsync( Message.Generate("Pong!") )
        End Sub        
    End Module
End Namespace
