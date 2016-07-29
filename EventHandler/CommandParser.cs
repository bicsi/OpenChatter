using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandHandler.ChatCommands;

namespace CommandHandler {
    public class CommandParser {

        static CommandParser() {
            var derived = typeof (ChatCommandBase);
            Assembly assembly = Assembly.GetAssembly(derived);

            // Load all the commands available through Reflection
            List<Type> derivedTypes = assembly
                .GetTypes()
                .Where(t =>
                    t != derived && derived.IsAssignableFrom(t))
                .ToList();

            foreach (Type T in derivedTypes) {
                ChatCommandBase inst = ((ChatCommandBase) Activator.CreateInstance(T));

                string code = inst.Code;
                ICommandParserBase parser = (ICommandParserBase) Activator.CreateInstance(inst.ParserType);

                commandMap.Add(code, parser);
            }
    }

        private static readonly Dictionary<string, ICommandParserBase> commandMap = new Dictionary<string, ICommandParserBase>();

        public async Task<ChatCommandBase> ReadNext(StreamReader reader) {
            var commandName = await reader.ReadLineAsync();
            if (!commandMap.ContainsKey(commandName)) {
                Console.WriteLine($"Command unrecognized: {commandName}. (did you implement command properly?)");
                return new NopCommand();
            }

            return await commandMap[commandName].Receive(reader);
        }

        public async Task Write<T>(T command, StreamWriter writer) where T : ChatCommandBase {
            await writer.WriteLineAsync(command.Code);
            await (commandMap[command.Code]).Send(command, writer);
            await writer.FlushAsync();
        }
    }
}