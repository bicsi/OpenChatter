using CommandHandler;

namespace ServerCoreLib {
    internal class CommandParser {

        /// <summary>
        /// Parses command from string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ChatCommand ParseClientCommand(string response) {

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
                    command.Content = args[2];
                    break;
            }

            return command;
        }
    }
}