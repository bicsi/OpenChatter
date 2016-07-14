using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using EventHandler = System.EventHandler;


namespace ServerCoreLib {
    internal class ServerConnection : IDisposable {

        private TcpClient client;
        public string Name { get; private set; }
        private bool active = false;

        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        // Ctor
        public ServerConnection(TcpClient client) {
            // Instantiate the client and the streams
            this.client = client;
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
        }

        public async Task ReadNameAsync() {
            await writer.WriteLineAsync(ServerCommandType.SendNameRequest);
            await writer.FlushAsync();

            // Get response from client  
            string response = await reader.ReadLineAsync();

            // Parse the response to gather info
            string[] splitted = response.Split(' ');

            // TODO: Make this work
            if (splitted[0] != ClientCommandType.ReceiveNameRequest)
                throw new InvalidDataException("Client's name response did not match protocol");

            // Update client's name
            Name = splitted[1];
            active = true;
            ListenForCommands();
        }

        public override string ToString() {
            return $"{Name} [{client.Client.RemoteEndPoint}]";
        }

        

        public async Task SendConnAckAsync() {
            await writer.WriteLineAsync(ServerCommandType.SendConnectACK);
            await writer.FlushAsync();
        }

        private async void ListenForCommands() {
            while (true) {
                string response = await reader.ReadLineAsync();

                // If client has disconnected
                if (response == ClientCommandType.ReceiveDisconnect) {
                    Dispose();
                    return;
                }
                // Wait for a while
                Thread.Sleep(1000);
            }
        }

        public void Dispose() {
            active = false;

            Console.WriteLine($"{this} disconnected.");

            writer.Close();
            reader.Close();
            client.Close();
        }
    }
}