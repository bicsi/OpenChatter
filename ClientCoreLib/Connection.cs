using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommandHandler;

namespace ClientCoreLib {
    internal class Connection : IDisposable {
        private TcpClient client;

        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        private CommandParser parser;
        private CommandExecuter executer;
        private Action OnActivated;

        public string Name { get; private set; }

        public Connection(Action onActivated) {
            client = new TcpClient();
            parser = new CommandParser();
            executer = new CommandExecuter(this);
            OnActivated = onActivated;
        }

        /// <summary>
        /// Connects to the TcpClient
        /// </summary>
        public void Start(IPAddress ipAddress, int port) {

            Console.WriteLine("Connecting to client...");

            Task.Run(new Action(() => client.Connect(ipAddress, port)))
                .ContinueWith((t) => {
                    Console.WriteLine($"Successfully connected to {client.Client.LocalEndPoint}!");

                    // Initialize streams
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);

                    Task.Run(new Action(executer.Start));

                    // Listen for commands asynchronously
                    ListenForCommandsAsync();
                });
        }

        /// <summary>
        /// Listents for received commands from server / command line
        /// </summary>
        public async void ListenForCommandsAsync() {
            while (client.Connected) {
                string response = await reader.ReadLineAsync();
                ChatCommand command = parser.ParseServerCommand(response);
                executer.AddCommand(command);
            }
        }

        public void Activate() {
            Console.WriteLine("You are now active on the server with the name: " + Name);
            Console.WriteLine();
            Console.WriteLine();

            OnActivated();
        }

        public void DisplayWelcome(string content) {
            Console.WriteLine(content);
            AskForName();
        }

        private void AskForName() {
            Console.Write("Please input your name: ");
            string name = Console.ReadLine();


            Name = name;
            // Create command for name
            SendCommand(new ChatCommand { Type = ClientCommandType.SendName });

        }

        public void DisplayMessage(string sender, string content) {
            Console.WriteLine($"{sender} : {content}");
        }

        public void Stop() {

            // Send stop signal
            SendCommand(new ChatCommand { Type = ClientCommandType.SendDisconnect });

            executer.Stop();
        }

        public void Dispose() {
            Stop();
            reader.Close();
            writer.Close();
            stream.Close();
        }

        public void SendCommand(ChatCommand chatCommand) {
            chatCommand.Sender = Name;
            writer.WriteLine(parser.StringifyCommand(chatCommand));
            writer.Flush();
        }
    }

}