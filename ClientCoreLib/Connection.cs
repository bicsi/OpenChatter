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

        public string Name { get; private set; }

        public Connection() {
            client = new TcpClient();
            parser = new CommandParser();
            executer = new CommandExecuter(this);
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


        public void DisplayWelcome(string content) {
            Console.WriteLine(content);
            AskForName();
        }

        private void AskForName() {
            Console.Write("Please input your name: ");
            string name = Console.ReadLine();

            // Create command for name
            ChatCommand command = new ChatCommand {
                Type = ClientCommandType.SendName,
                Content = name
            };

            Name = name;

            writer.WriteLine(parser.StringifyCommand(command, name));
            writer.Flush();
        }

        public void DisplayMessage(string sender, string content) {
            Console.WriteLine($"{sender} : {content}");
        }

        public void DisplayConnected() {
            Console.WriteLine("You have connected to the server");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void Stop() {

            // Send stop signal
            ChatCommand command = new ChatCommand {Type = ClientCommandType.SendDisconnect};
            writer.WriteLine(parser.StringifyCommand(command, Name));
            writer.Flush();
            
            executer.Stop();
        }

        public void Dispose() {
            Stop();
            reader.Close();
            writer.Close();
            stream.Close();
        }
    }
}