using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerCoreLib {
    public class ConnectionListener {
        public IPAddress IP { get; private set; }
        public int Port { get; private set; }

        private TcpListener listener;
        public event Action<TcpClient> OnClientConnected;

        /// <summary>
        /// Constructor for the ConnectionListener class
        /// </summary>
        /// <param name="ip">The IP of the server</param>
        /// <param name="port">The port configured for listening</param>
        public ConnectionListener(IPAddress ip, int port, Action<TcpClient> onClientConnected) {
            IP = ip;
            Port = port;
            OnClientConnected = onClientConnected;
        }

        public void Start() {
            // Initialize a listener
            listener = new TcpListener(IP, Port);

            // Start listening
            listener.Start();
        }

        public async Task AcceptConnectionsAsync() {
            while (true) {
                // Got connection
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"Connected to: {client.Client.RemoteEndPoint}");

                // Invoke event
                OnClientConnected(client);
            }
        }
    }
}