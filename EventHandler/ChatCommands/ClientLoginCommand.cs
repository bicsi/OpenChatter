using System;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class ClientLoginCommand : ChatCommandBase {
        public override string Code => "login";
        public override Type ParserType => typeof(ClientLoginParser);

        public string Name;
        
    }

    internal class ClientLoginParser : CommandParser<ClientLoginCommand> {
        
        public override async Task WriteAsync(ClientLoginCommand command, StreamWriter writer) {
            await writer.WriteLineAsync(command.Name);
        }

        public override async Task<ClientLoginCommand> ReadAsync(StreamReader reader) {
            return new ClientLoginCommand {
                Name = await reader.ReadLineAsync()
            };
        }
    }
}
