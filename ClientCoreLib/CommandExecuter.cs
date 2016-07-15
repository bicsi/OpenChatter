using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using CommandHandler;

namespace ClientCoreLib {
    internal class CommandExecuter {

        private BlockingCollection<ChatCommand> commandsQueue;
        private Connection connection;

        /// <summary>
        /// Constructs the object, given a connection
        /// </summary>
        public CommandExecuter(Connection conn) {
            connection = conn;
            commandsQueue = new BlockingCollection<ChatCommand>();
        }

        /// <summary>
        /// Starts a neverending loop of executing commands
        /// </summary>
        public void Start() {
            foreach (var command in commandsQueue.GetConsumingEnumerable()) {
                ExecuteCommand(command);
            }
        }

        /// <summary>
        /// Executes the currently extracted command
        /// </summary>
        private void ExecuteCommand(ChatCommand command) {
            switch (command.Type) {
                case ServerCommandType.SendWelcomeMessage:
                    connection.DisplayWelcome(command.Content);
                    break;

                case ServerCommandType.ForwardMessage:
                    connection.DisplayMessage(command.Sender, command.Content);
                    break;

                case ServerCommandType.SendConnectACK:
                    connection.DisplayConnected();
                    break;

                default:
                    throw new InvalidEnumArgumentException($"Invalid command: {command.Type}");
            }
        }

        /// <summary>
        /// Adds a given command to the processing queue
        /// </summary>
        public void AddCommand(ChatCommand command) {
            commandsQueue.Add(command);
        }

        public void Stop() {
            commandsQueue.CompleteAdding();
        }
    }
}