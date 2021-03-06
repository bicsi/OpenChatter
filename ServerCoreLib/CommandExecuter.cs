﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using CommandHandler;
using CommandHandler.ChatCommands;

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
            sender = tracker.GetClientByName(command.SenderName);

            if (command.Command is ActiveUsersCommand) {
                sender.ReceiveActiveUsersAsync(tracker.GetList());
                return;
            }
            
            if (command.Command is SendMessageCommand) {
                var cmd = ((SendMessageCommand)command.Command);
                try {
                    if (cmd.Sender != command.SenderName) {
                        Console.WriteLine($"Warning: {sender} is cheating - saying his name is {cmd.Sender}.");
                        Console.WriteLine("Renaming...");
                    }
                    cmd.Sender = command.SenderName;
                    recipient = tracker.GetClientByName(cmd.Destination);
                    recipient.ReceiveMessageAsync(command.SenderName, cmd.Body, cmd.DateSent);

                    Console.WriteLine($"Send message command from {command.SenderName} to {recipient} : {cmd.Body}");
                }
                catch (KeyNotFoundException) {
                    Console.WriteLine($"One message was ignored. No user has that name ({cmd.Destination}).");
                }

                return;
            }

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
