using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;

namespace ServerCoreLib {
    public class ChatServerCore {
        
        public int Port { get; private set; }
        public IPAddress IP { get; private set; }

        
        private ClientListTracker clientList;

        private CommandExecuter executer;
        private ConnectionListener listener;


        /// <summary>
        /// Constructor for the ChatServerCore class
        /// </summary>
        /// <param name="port"> the port of the connection </param>
        public ChatServerCore(int port) {
            Port = port;
            IP = IPAddress.Parse("0.0.0.0"); // TODO: Use the real IP here

            clientList = new ClientListTracker();
            executer = new CommandExecuter(clientList);
            listener = new ConnectionListener(IP, Port, OnClientConnected);
        }
        

        /// <summary>
        /// Starts the whole process
        /// </summary>
        public void Start() {

            // Log the details
            Console.WriteLine($"Current IP is: {IP}");

            // Start the ConnectionListener and start accepting connections
            listener.Start();
            listener.AcceptConnectionsAsync();

            Console.WriteLine($"Started listening at port: {Port}");

            // Start the CommandExecuter
            executer.Start();


        }

        /// <summary>
        /// Some client has connected
        /// Ask for their name
        /// </summary>
        /// <param name="client">the TcpClient returned by the listener</param>
        private void OnClientConnected(TcpClient client) {

            // Build a ServerConnection object
            ServerConnection conn = new ServerConnection(
                client,
                (sender, command) => {
                    if (command.Type == ClientCommandType.SendName) {
                        sender.Activate(command.Content);
                    }
                    else {
                        command.Sender = sender.Name;
                        executer.AddCommand(command);
                    }
                    
                },
                clientList.AddConnection
            );

            // Start the ServerConnection object
            conn.Start();
        }
        
        
    }
}
