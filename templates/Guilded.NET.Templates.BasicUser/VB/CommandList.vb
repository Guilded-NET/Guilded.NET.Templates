Imports System.Collections.Generic

Imports Guilded.NET
Imports Guilded.NET.Objects.Chat
Imports Guilded.NET.Objects.Events

Namespace ProjectName
    ''' <summary>
    ''' List of user bot commands.
    ''' </summary>
    Public Module CommandList
        ' NOTE: This is how message-based command looks like

        ''' <summary>
        ''' Responds with `Pong!`.
        ''' </summary>
        ''' <param name="client">Client that generated a command info and invoked this command method</param>
        ''' <param name="info">Information about the command used, with arguments and other things</param>
        ''' <param name="messageCreated">Message creation event</param>
        <Command("ping", "pong", Description := "Responds with `Pong!`")>
        Public Async Sub Ping(client As BasicGuildedClient, info As CommandInfo, messageCreated As MessageCreatedEvent)
            ' Sends a message to channel where `ping`/`pong` command was used
            ' Generates a new message with content `Pong!`
            Await messageCreated.RespondAsync( Message.Generate("Pong!") )
        End Sub

        ' NOTE: This is how forum/comment-based command looks like

        ''' <summary>
        ''' Responds with `Pong!`.
        ''' </summary>
        ''' <param name="client">Client that generated a command info and invoked this command method</param>
        ''' <param name="info">Information about the command used, with arguments and other things</param>
        ''' <param name="replyCreated">Reply/comment creation event</param>
        <Command("ping", "pong", Description := "Responds with `Pong!`", Placement := CommandPlacement.Comments)>
        Public Async Sub PingReply(client As BasicGuildedClient, info As CommandInfo, replyCreated As ContentReplyCreatedEvent)
            ' Sends a reply to the post where the command was invoked
            ' Markdown is ignored here. Use nodes/leaves instead.
            ' Markdown-plain-text doesn't work as it's supposed to in comments/repliess
            Await replyCreated.ReplyAsync( MessageContent.GenerateText("Pong!") )
        End Sub
    End Module
End Namespace
