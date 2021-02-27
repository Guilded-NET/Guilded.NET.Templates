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
        ''' <summary>
        ''' Shows a list of all bot commands.
        ''' </summary>
        ''' <param name="client">Client to post message with</param>
        ''' <param name="messageCreated">Message creation event</param>
        ''' <param name="command">Name of the command used</param>
        ''' <param name="arguments">Command arguments</param>
        <Command("help", "commands", "commandlist", "command-list", Description := "Shows a list of commands.", Usage = "<command name>")>
        Public Async Sub Help(client As BasicGuildedClient, messageCreated As MessageCreatedEvent, content As String, arguments As IList(Of String))
            ' Gets all commands in the client
            Dim keys = client.CommandDictionary.Keys
            ' If there are 0 arguments, then give all commands
            If arguments.Count = 0 Then
                ' Turn commands to string
                Dim com As IEnumerable(Of String) = keys.[Select](Function(x) "**" + x.Name + "**: " + x.Description)
                ' Generate embed for the command list
                Dim embed As Embed = New Embed().SetTitle("Command list").SetDescription(String.Join("\n", com)).AddField("For more info", "Type `help command_name` to get more info about a command.")
                ' Output the embed
                Await messageCreated.RespondAsync(Message.Generate(embed))
            ' If argument was given
            Else
                ' Command's name
                Dim commandname As String = arguments(0).Trim()
                ' Find that command
                Dim com As CommandAttribute = keys.FirstOrDefault(Function(x) x.Name = command Or x.Alias.Contains(commandname))
                ' If that command was not found
                If com = Nothing Then
                    Await messageCreated.RespondAsync(Message.Generate("Could not find that command."))
                    Return
                End If
                ' If it was, then generate embed for it
                Dim embed As Embed = New Embed().SetTitle(com.Name).SetDescription(com.Description).AddField("Aliases", string.Join(", ", com.[Alias]), true).AddField("Usage", com.Name + " " + com.Usage)
                ' Output the embed
                Await messageCreated.RespondAsync(Message.Generate(embed))
            End If    
        End Sub
    End Module
End Namespace
