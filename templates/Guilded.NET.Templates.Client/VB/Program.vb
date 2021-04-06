Imports System
Imports System.IO
Imports System.Threading.Tasks

Imports Guilded.NET

Imports Newtonsoft.Json.Linq

Namespace ProjectName
    ''' <summary>
    ''' User bot client program.
    ''' </summary>
    Module Program
        ''' <summary>
        ''' Guilded.NET userbot client
        ''' </summary>
        Dim bot As GuildedUserClient
        ''' <summary>
        ''' Creates a new user bot client.
        ''' </summary>
        ''' <param name="args">Program arguments</param>
        Sub Main(args As String())
            ' Read JSON "config/config.json"
            Dim config As JObject = JObject.Parse(File.ReadAllText("./config/config.json"))
            ' Get login info
            Dim email As String = config("email").Value(Of String)()
            Dim password As String = config("password").Value(Of String)()
            Dim prefix As String = config("prefix").Value(Of String)()
            ' Tells us that it's starting with specific prefix
            Console.WriteLine("Starting the both with prefix '{0}'", prefix)
            ' Creates config for the client
            ' TODO: Change null to GId.Parse("yourId")
            Dim clientConfig As GuildedClientConfig = New GuildedClientConfig(GuildedClientConfig.BasicPrefix(prefix), null)
            ' Creates new client
            Using client As New GuildedUserClient(email, password, clientConfig)
                ' Makes it accessable to handlers
                bot = client
                ' Fetches all commands from specific type
                client.FetchCommands(
                    GetType(CommandList)
                )
                ' When client connects to Guilded
                AddHandler client.Connected, AddressOf ConnectedEvent
                ' When client is ready
                AddHandler client.Prepared, AddressOf PreparedEvent
                ' Start the bot
                StartAsync(client).GetAwaiter().GetResult()
            End Using
        End Sub
        ''' <summary>
        ''' Makes bot connect to Guilded and then stops it from shutting down.
        ''' </summary>
        ''' <param name="client">Client to connect</param>
        ''' <returns>Async task</returns>
        Async Function StartAsync(client As GuildedUserClient) As Task
            ' Connects to Guilded
            Await client.ConnectAsync()
            ' Makes it stop forever, so the bot wouldn't instantly shutdown after connecting
            Await Task.Delay(-1)
        End Function
        ''' <summary>
        ''' When bot connects to Guilded.
        ''' </summary>
        ''' <param name="o">Caller(client)</param>
        ''' <param name="e">Arguments from the event; empty</param>
        Sub ConnectedEvent(o As Object, e As EventArgs)
            Console.WriteLine("Connected")
        End Sub
        ''' <summary>
        ''' When bot is prepared for use.
        ''' </summary>
        ''' <param name="o">Caller(client)</param>
        ''' <param name="e">Arguments from the event; empty</param>
        Sub PreparedEvent(o As Object, e As EventArgs)
            Console.WriteLine("I successfully logged in!\n - ID: {0}\n - Name: {1}", bot.[Me].Id, bot.[Me].Username)
        End Sub
    End Module
End Namespace