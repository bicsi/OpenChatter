using System;
using System.Collections.Generic;
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

        event Action<SendMessageCommand> MessageReceived;
        event Action<List<string>> UserListUpdated;

        bool LoggedIn { get; }
        string Name { get; }
        Task RefreshAsync();
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
            Name = name;
            LoggedIn = true;
            await RefreshAsync();
        }

        public async Task LogoutAsync() {
            await communicator.SendCommandAsync(new ClientLogoffCommand());
            Name = null;
            LoggedIn = false;
        }

        public event Action<SendMessageCommand> MessageReceived = delegate { };
        public event Action<List<string>> UserListUpdated = delegate { };
        public bool LoggedIn { get; private set; }
        public string Name { get; private set; }


        /// <summary>
        /// Handles a server command
        /// </summary>
        private void HandleCommand(ChatCommandBase comm) {
            if (comm is ActiveUsersCommand) {
                UserListUpdated(((ActiveUsersCommand) comm).Users);
                return;
            }
        }
        

        public async Task StartAsync(IPAddress ip, int port) {
            DIContainer.AddInstance((IChatClient) Instance);
            await communicator.ConnectAsync(ip, port);
            communicator.StartListening();
            StartAutoRefreshAsync();
        }

        public async Task RefreshAsync() {
            if (LoggedIn)
                await communicator.SendCommandAsync(new ActiveUsersCommand());
        }

        private async void StartAutoRefreshAsync() {
            while (communicator.Connected) {
                await RefreshAsync();
                await Task.Delay(5000);
            }
        }
    }
}
