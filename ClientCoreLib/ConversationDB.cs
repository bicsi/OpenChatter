using System;
using System.Collections.Generic;

namespace ClientCoreLib {
    public interface IConversationDb {
        void AddConversation(string with);
        void AddMessage(Message msg);
        List<Message> GetConversation(string with);

        event Action<string> OnInitializedConversation;
    }

    internal class ConversationDb : IConversationDb {
        private Dictionary<string, List<Message>> database;

        public static ConversationDb Instance = new ConversationDb();

        public event Action<string> OnInitializedConversation = delegate { };

        private ConversationDb() {
            database = new Dictionary<string, List<Message>>();
        }

        public void AddConversation(string with) {
            database.Add(with, new List<Message>());
            OnInitializedConversation(with);
        }
        

        public void AddMessage(Message msg) {
            string with = (msg is SentMessage) ? ((SentMessage) msg).To : ((ReceivedMessage) msg).From;

            // If not added conversation, will initialize conversation
            if (!database.ContainsKey(with)) {
                AddConversation(with);
            }

            // Add message to conversation
            database[with].Add(msg);
        }

        public List<Message> GetConversation(string with) {

            if (!database.ContainsKey(with)) {
                AddConversation(with);
            }
            return database[with];
        }

    }
}
