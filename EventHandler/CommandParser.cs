using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler {
    public class CommandParser {

        public static void AddParser(string commandName, ICommandParserBase parser) {
            commandMap.Add(commandName, parser);
        }

        private static Dictionary<string, ICommandParserBase> commandMap = new Dictionary<string, ICommandParserBase>();

        public async Task<ChatCommandBase> ReadNext(StreamReader reader) {
            var commandName = await reader.ReadLineAsync();
            return await commandMap[commandName].Receive(reader);
        }

        public async Task Write<T>(T command, StreamWriter writer) where T : ChatCommandBase {
            await writer.WriteLineAsync(command.Code);
            await (commandMap[command.Code]).Send(command, writer);
            await writer.FlushAsync();
        }
    }
}