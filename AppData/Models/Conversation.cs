using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models {
    public class Conversation : ModelBase {
        private ObservableCollection<Message> _messages;
        private string _messageContent;

        public ObservableCollection<Message> Messages {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(nameof(Messages)); }
        }

        public User OtherUser { get; private set; }

        public static Conversation Current { get; set; } = new Conversation(new User {Name = "admin"});

        public string MessageContent {
            get { return _messageContent; }
            set { _messageContent = value; OnPropertyChanged(nameof(MessageContent));}
        }

        public Conversation(User other) {
            OtherUser = other;
            _messages = new ObservableCollection<Message>();
        }
    }
}
