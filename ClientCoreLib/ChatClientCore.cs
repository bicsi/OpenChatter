using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CommandHandler;
using CommandHandler.ChatCommands;
using ConnectionHandler;
using Utilities;

namespace ClientCoreLib {
    public interface IChatClient {
        Task StartAsync(IPAddress ip, int port);
        Task LoginAsync(string name);
        Task LogoutAsync();
    }

    public class ChatClientCore : IChatClient {

        private TcpCommunicator communicator;
        public static ChatClientCore Instance = new ChatClientCore();

        private ChatClientCore() {
            communicator = new TcpCommunicator();
            communicator.OnCommandReceived += HandleCommand;
        }

        public async Task LoginAsync(string name) {
            await communicator.SendCommandAsync(new ClientLoginCommand {
                Name = name
            });
        }

        public async Task LogoutAsync() {
            await communicator.SendCommandAsync(new ClientLogoffCommand());
        }

        /// <summary>
        /// Handles a server command
        /// </summary>
        private void HandleCommand(ChatCommandBase comm) {
            if (comm is SendMessageCommand) {
                
            }
        }
        

        public async Task StartAsync(IPAddress ip, int port) {
            DIContainer.AddInstance((IChatClient) Instance);
            await communicator.ConnectAsync(ip, port);
            communicator.StartListening();
        }
    }
}
