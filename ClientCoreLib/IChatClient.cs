using System;
using System.Threading.Tasks;
using CommandHandler;

namespace ClientCoreLib {
    public interface IChatClient {
        Task Start(string myName, string ip, int serverPort);
        void SendMessage(SendMessageCommand msg);
        
        event Action<SendMessageCommand> MessageReceived;
        event Action<ActiveUsersCommand> UsersUpdated;

    }

    public class ChatClient : IChatClient {
        public async Task Start(string myName, string ip, int serverPort) {
            //
            //do not await this
            Task.Run(() => ListenMessages());
        }

        private void ListenMessages() {
            
        }

        public void SendMessage(SendMessageCommand msg) {
            //new CommandParser().Send(msg, )
        }

        public string[] GetActiveUsers() {
            throw new NotImplementedException();
        }

        public event Action<SendMessageCommand> MessageReceived;
        public event Action<ActiveUsersCommand> UsersUpdated;
    }
}