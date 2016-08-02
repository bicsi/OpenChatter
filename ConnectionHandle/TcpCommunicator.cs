using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using CommandHandler.ChatCommands;

namespace ConnectionHandler
{
    
    public class TcpCommunicator : IDisposable {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private CommandParser parser;
        private ConcurrentBag<ChatCommandBase> sendQueue; 

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
                    await Task.Delay(1000);
                }
            }
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            sendQueue = new ConcurrentBag<ChatCommandBase>();
        }

        private async void StartListening() {
            while (Connected) {
                var command = await parser.ReadNext(reader);
                OnCommandReceived(command);
            }
        }

        private async void StartSending() {
            while (Connected) {
                var command = await FetchCommandAsync();
                await parser.Write(command, writer);
            }
        }

        private async Task<ChatCommandBase> FetchCommandAsync() {
            ChatCommandBase ret = new NopCommand();
            while (Connected) {
                if (sendQueue.TryTake(out ret))
                    break;
                await Task.Delay(100);
            }
            return ret;
        }

        public void StartAsync() {
            StartListening();
            StartSending();
        }

        

        public async Task SendCommandAsync(ChatCommandBase command) {
            // Places the command in queue
            sendQueue.Add(command);
        }

        public void Dispose() {
            client.Close();
            reader.Close();
            writer.Close();
        }
    }
}
