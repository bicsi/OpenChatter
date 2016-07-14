using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using EventHandler = System.EventHandler;


namespace ServerCoreLib {
    public class ServerConnection : IDisposable {

        private TcpClient client;
        public string Name { get; private set; }
        private bool active = false;
        private bool connected = false;

        private Action<ServerConnection, ChatCommand> OnReceivedCommand;
        private Action<ServerConnection> OnActivated;

        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private CommandParser parser;

        /// <summary>
        /// The constructor for the ServerConnection class
        /// </summary>
        /// <param name="client"> The TcpClient of the connection </param>
        public ServerConnection (
            TcpClient client, 
            Action<ServerConnection, ChatCommand> onReceivedCommand,
            Action<ServerConnection> onActivated
            ) {
            // Instantiate the client and the streams
            this.client = client;
            Name = "anonymous";
            OnReceivedCommand = onReceivedCommand;
            OnActivated = onActivated;
        }


        /// <summary>
        /// Starts the ServerConnection to listen for commands
        /// </summary>
        public void Start() {

            // Initialize streams
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            this.connected = true;

            ListenForCommandsAsync();
        }
        
        
        /// <summary>
        /// Listens asynchronously for commands
        /// </summary>
        private async void ListenForCommandsAsync() {
            while (connected) {
                //Fetch a command
                string response = await reader.ReadLineAsync();
                ChatCommand command = CommandParser.ParseClientCommand(response);

                // Invoke the event
                OnReceivedCommand(this, command);

                // Wait for a while
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Activates the connection with a given name
        /// Invokes the OnActivated event
        /// </summary>
        /// <param name="name"></param>
        public void Activate(string name) {
            active = true;
            Name = name;
            OnActivated(this);
        }
        
        
        /// <summary>
        /// Converts the ServerConnection to a string for
        /// easy output
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"{Name} [{client.Client.RemoteEndPoint}]";
        }

        /// <summary>
        /// Destroys the object and closes the streams
        /// </summary>
        public void Dispose() {
            active = false;
            connected = false;

            writer.Close();
            reader.Close();
            client.Close();

            //TODO: OnDispose();
        }

        
    }
}