using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;

namespace ClientCoreLib {
    class CommandLineListener {

        private CommandParser parser;
        private Action<ChatCommand> OnUserTypedCommand;

        public CommandLineListener(Action<ChatCommand> onUserTypedCommand) { 
            parser = new CommandParser();
            OnUserTypedCommand = onUserTypedCommand;
        }

        public void Start() {
            ListenForUserCommands();
        }

        private void ListenForUserCommands() {
            while (true) {
                string response = Console.ReadLine();
                ChatCommand command = parser.ParseClientCommand(response);

                OnUserTypedCommand(command);
                Thread.Sleep(100);
            }
        }
    }
}
