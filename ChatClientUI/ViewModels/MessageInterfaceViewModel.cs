using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using ClientCoreLib;
using Utilities;

namespace ChatClientUI.ViewModels {
    class MessageInterfaceViewModel {
        public Conversation CurrentConversation => Conversation.Current;
        
        public RelayCommand SendMessageCommand { get; private set; }

        public MessageInterfaceViewModel() {
            SendMessageCommand = new RelayCommand(SendMessage);
            DIContainer.GetInstance<IChatClient>().MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(ReceivedMessage obj) {
            CurrentConversation.Messages.Add(new AppData.Models.TheirMessage {
                Body = obj.Body,
                From = new User { Name = obj.From },
                DateSent = obj.DateSent
            });
        }

/*
        public void ChangeUser(User newUser) {
            IConversationDb database = DIContainer.GetInstance<IConversationDb>();
            var list = database.GetConversation(newUser.Name);

            CurrentConversation = new Conversation(newUser) {
                Messages = list.Select(msg => {
                    AppData.Models.Message result;
                    if (msg is ClientCoreLib.SentMessage) {
                        result = new AppData.Models.MyMessage {
                            To = newUser,
                            Body = msg.Body
                        };
                    }
                    else {
                        result = new AppData.Models.TheirMessage {
                            From = newUser,
                            Body = msg.Body
                        };
                    }
                    return result;
                }).ToList()
            };
        }
*/

        private void SendMessage() {
            string user = CurrentConversation.OtherUser.Name;
            string content = CurrentConversation.MessageContent;

            DIContainer.GetInstance<IChatClient>().SendMessageAsync(user, content);

            CurrentConversation.Messages.Add(new AppData.Models.MyMessage {
                Body = content,
                DateSent = DateTime.Now,
                To = new User {Name = user}
            });
        }
    }
}
