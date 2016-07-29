using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using CommandHandler.ChatCommands;

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
            Console.WriteLine();

            // Start the CommandExecuter
            executer.Start();


        }

        /// <summary>
        /// Some client has connected;
        /// ask for their name and start the connection
        /// </summary>
        /// <param name="client">the TcpClient returned by the listener</param>
        private void OnClientConnected(TcpClient client) {

            // Build a ServerConnection object
            ServerConnection conn = new ServerConnection (
                // TcpClient
                client,
                // OnReceivedCommand
                (sender, command) => {
                    if (command == null) {
                        sender.Dispose();
                        return;
                    }

                    if(command is NopCommand) {
                        Console.WriteLine("Command ignored.");
                        return;
                    }

                    if (command is ClientLogoffCommand) {
                        sender.Deactivate();
                        return;
                    }

                    if (command is ClientLoginCommand) {
                        sender.Activate(((ClientLoginCommand)command).Name);
                        return;
                    }
                     
                    executer.AddCommand(new ServerChatCommand {
                        SenderName = sender.Name,
                        Command = command
                    });
                },
                // OnActivated
                clientList.AddConnection,
                // OnDisposed
                clientList.RemoveConnection
            );

            // Start the ServerConnection object
            conn.Start();
        }
        
        
    }
}
