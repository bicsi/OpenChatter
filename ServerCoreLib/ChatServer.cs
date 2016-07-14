using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCoreLib {
    public class ChatServer {

        // Public properties for the server connection
        public int Port { get; private set; }
        public IPAddress IP { get; private set; }

        
        private TcpListener listener;
        private ConcurrentBag<ServerConnection> activeConnections;
        private Queue<ServerConnection> availabilityQueue;    


        // Ctor
        public ChatServer(int port) {
            Port = port;
            IP = IPAddress.Parse("0.0.0.0");
            activeConnections = new ConcurrentBag<ServerConnection>();
            availabilityQueue = new Queue<ServerConnection>();
        }

        // Start starts the whole process
        public void Start() {

            // Log the details
            Console.WriteLine($"Current IP is: {IP}");
            Console.WriteLine($"Started listening at port: {Port}");

            // Initialize a listener
            listener = new TcpListener(IP, Port);

            // Start listening
            listener.Start();
            
            // Initialize thread for connections

            AcceptConnectionsAsync();

        }
        

        private void RemoveConnection(ServerConnection conn) {
            // Try to remove it until it's all ok
            while(activeConnections.TryTake(out conn) == false)
                Thread.Sleep(100);
        }
        private void AddConnection(ServerConnection conn) {
            // Adds a connection to the ChatServer list

            activeConnections.Add(conn);
            availabilityQueue.Enqueue(conn);
        }

        private async void AcceptConnectionsAsync() {

            while (true) {
                // Got connection
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"Connected to: {client.Client.RemoteEndPoint}");
                
                // Initialize a connection and ask for its name
                Console.WriteLine($"Asking {client.Client.RemoteEndPoint} for name...");
                ServerConnection conn = new ServerConnection(client);

                // TODO: Change ugly method
#pragma warning disable 4014
                conn.ReadNameAsync()
                    .ContinueWith(async t => {
#pragma warning restore 4014
                    // Throw exceptions TODO Add Exception Handling
                    if (t.IsFaulted)
                        return;

                    // Send connection ok to client
                    await conn.SendConnAckAsync();

                    AddConnection(conn);
                    Console.WriteLine($"{conn} successfully connected!");
                });
                
            }
        }

    }
}
