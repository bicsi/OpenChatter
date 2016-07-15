using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace CommandHandler {
    public class CommandParser {

        /// <summary>
        /// Parses command from string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public ChatCommand ParseClientCommand(string response) {
            if (response == null)
                return null;

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
        /// Parses a ServerCommand given a string
        /// </summary>
        public ChatCommand ParseServerCommand(string response) {

            // Split the command
            string[] args = response.Split(' ');

            ChatCommand command = new ChatCommand();
            command.Type = args[0];


            // Parse the command
            switch (args[0]) {
                case ServerCommandType.SendConnectACK:
                    break;

                case ServerCommandType.ForwardMessage:
                    command.Sender = args[1];
                    command.Content = string.Join(" ", args.Skip(2).ToArray());
                    break;

                case ServerCommandType.SendWelcomeMessage:
                    command.Content = string.Join(" ", args.Skip(1).ToArray());
                    break;

                default:
                    throw new InvalidEnumArgumentException($"Unrecognized command: {args[0]}");
            }

            return command;
        }

        /// <summary>
        /// Transforms message into string
        /// </summary>
        public string StringifyMessage(string senderName, string content) {
            return $"{ServerCommandType.ForwardMessage} {senderName} {content}";
        }

        /// <summary>
        /// Transforms welcome message
        /// </summary>
        public string StringifyWelcome() {
            return ServerCommandType.SendWelcomeMessage + " " + "This is a sample welcome message. This should be taken away from here.";
        }


        public string StringifyCommand(ChatCommand command, string senderName) {
            return $"{command.Type} {senderName} {command.Content}";
        }
    }
}