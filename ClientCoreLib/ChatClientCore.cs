using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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

        event Action<ReceivedMessage> MessageReceived;
        event Action<List<string>> UserListUpdated;

        bool LoggedIn { get; }
        string Name { get; }
        Task RefreshAsync();
        Task SendMessageAsync(string user, string content);
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

        public event Action<ReceivedMessage> MessageReceived = delegate { };
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

            if (comm is SendMessageCommand) {

                var received = new ReceivedMessage { 
                    From = ((SendMessageCommand) comm).Sender,
                    Body = ((SendMessageCommand) comm).Body,
                    DateSent = ((SendMessageCommand) comm).DateSent
                };

                DIContainer.GetInstance<IConversationDb>().AddMessage(received);
                MessageReceived(received);

                return;
            }
        }
        

        public async Task StartAsync(IPAddress ip, int port) {
            DIContainer.AddInstance((IChatClient) Instance);
            DIContainer.AddInstance((IConversationDb) ConversationDb.Instance);

            await communicator.ConnectAsync(ip, port);
            communicator.StartListening();

            StartAutoRefreshAsync();
        }
        
        public async Task RefreshAsync() {
            if (LoggedIn) {
                await communicator.SendCommandAsync(new ActiveUsersCommand());
            }
        }

        public async Task SendMessageAsync(string user, string content) {
            DateTime sent = DateTime.Now;

            DIContainer.GetInstance<IConversationDb>().AddMessage(new SentMessage {
                To = Name,
                Body = content,
                DateSent = sent
            });

            await communicator.SendCommandAsync(new SendMessageCommand {
                Sender = Name,
                Destination = user,
                Body = content,
                DateSent = sent
            });
        }

        private async void StartAutoRefreshAsync() {
            while (communicator.Connected) {
                await RefreshAsync();
                await Task.Delay(5000);
            }
        }
    }
}
