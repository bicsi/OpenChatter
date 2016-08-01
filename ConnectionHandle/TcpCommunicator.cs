using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;

namespace ConnectionHandler
{
    
    public class TcpCommunicator : IDisposable {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private CommandParser parser;

        public event Action<ChatCommandBase> OnCommandReceived = delegate { };

        public bool Connected => client.Connected;

        public TcpCommunicator() {
            client = new TcpClient();
            parser = new CommandParser();
        }

        /// <summary>
        /// Connect to the given TcpClient
        /// </summary>
        public async Task ConnectAsync(IPAddress ip, int port) {
            while (!Connected) {
                try {
                    await client.ConnectAsync(ip, port);
                }
                catch(Exception e) {
                    Trace.WriteLine(e);
                    Thread.Sleep(1000);
                }
            }
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
        }

        public async void StartListening() {
            while (Connected) {
                var command = await parser.ReadNext(reader);
                OnCommandReceived(command);
            }
        }
        
        public async Task SendCommandAsync(ChatCommandBase command) {
            ///TODO: MAKE IT THREAD SAFE
            await parser.Write(command, writer);
        }

        public void Dispose() {
            client.Close();
            reader.Close();
            writer.Close();
        }
    }
}
