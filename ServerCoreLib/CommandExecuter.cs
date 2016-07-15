﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;

namespace ServerCoreLib {
    internal class CommandExecuter {
        private BlockingCollection<ChatCommand> commandsQueue;
        private ClientListTracker tracker;

        /// <summary>
        /// Constructor for the CommandExecuter class
        /// </summary>
        /// <param name="tracker"> The object that maps the names to connections </param>
        public CommandExecuter(ClientListTracker tracker) {
            this.tracker = tracker;
            commandsQueue = new BlockingCollection<ChatCommand>();
        }


        /// <summary>
        /// Adds a command to the processing queue
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(ChatCommand command) {
            commandsQueue.Add(command);
        }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="command"> The command to be executed </param>
        private void ExecuteCommand(ChatCommand command) {

            string serialized;
            ServerConnection sender, recipient;

            switch (command.Type) {

                // Client command types

                case ClientCommandType.SendName:
                    sender = tracker.GetClientByName(command.Sender);
                    sender.Activate(command.Content);
                    break;

                case ClientCommandType.SendDisconnect:
                    sender = tracker.GetClientByName(command.Sender);
                    sender.Dispose();
                    break;

            }
            
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
