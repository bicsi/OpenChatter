using System.Linq;
using CommandHandler;

namespace ServerCoreLib {
    internal class CommandParser {

        /// <summary>
        /// Parses command from string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public ChatCommand ParseClientCommand(string response) {

            // Split the command
            string[] args = response.Split(' ');

            ChatCommand command = new ChatCommand();
            command.Type = args[0];
            

            // Parse the command type
            switch (args[0]) {
                case ClientCommandType.SendDisconnect:
                    break;

                case ClientCommandType.SendName:
                    command.Content = args[1];
                    break;

                case ClientCommandType.SendMessage:
                    command.Recipient = args[1];
                    command.Content = string.Join(" ", args.Skip(2).ToArray());
                    break;
            }

            return command;
        }

        /// <summary>
        /// Transforms message into string
        /// </summary>
        public string StringifyMessage(string senderName, string content) {
            return $"{ServerCommandType.ForwardMessage} {senderName} {content}";
        }

        public string StringifyWelcome() {
            return ServerCommandType.SendWelcomeMessage + " " + "This is a sample welcome message. This should be taken away from here.";
        }
    }
}