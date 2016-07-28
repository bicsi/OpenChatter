using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;

namespace ServerCoreLib {
    public class ServerChatCommand {
        public ChatCommandBase Command { get; set; }
        public string SenderName { get; set; }
    }
    internal class CommandExecuter {
        private BlockingCollection<ServerChatCommand> commandsQueue;
        private ClientListTracker tracker;

        /// <summary>
        /// Constructor for the CommandExecuter class
        /// </summary>
        /// <param name="tracker"> The object that maps the names to connections </param>
        public CommandExecuter(ClientListTracker tracker) {
            this.tracker = tracker;
            commandsQueue = new BlockingCollection<ServerChatCommand>();
        }


        /// <summary>
        /// Adds a command to the processing queue
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(ServerChatCommand command) {
            commandsQueue.Add(command);
        }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="command"> The command to be executed </param>
        private void ExecuteCommand(ServerChatCommand command) {
            
            ServerConnection sender, recipient;

            if (command.Command is ClientLogin){
                sender = tracker.GetClientByName(command.SenderName);
                //sender.Activate(((ClientLogin)command.Command));
            }
            else if (command.Command is SendMessageCommand)
            {
                var cmd = ((SendMessageCommand)command.Command);
                try {
                    if (cmd.Sender != command.SenderName) {
                        Console.WriteLine($"Warning {command.SenderName} is cheating - saying his name is {cmd.Sender}.");
                    }
                    cmd.Sender = command.SenderName;
                    recipient = tracker.GetClientByName(cmd.Destination);
                    recipient.ReceiveMessageAsync(command.SenderName, cmd.Body);
                }
                catch (KeyNotFoundException e) {
                    Console.WriteLine($"One message was ignored. No user has that name ({cmd.Destination}).");
                }
            }
            else
                throw new Exception($"Unrecognized command: {command.Command.Code}.");
        }

    
        /// <summary>
        /// Start the loop that executes the commands in hte commandsQueue
        /// </summary>
        public void Start() {
            // Starts listening for commands
            foreach (var command in commandsQueue.GetConsumingEnumerable()) {
                ExecuteCommand(command);
            }
        }
    }
}
