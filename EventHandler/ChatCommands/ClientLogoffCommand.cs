using System;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class ClientLogoffCommand : ChatCommandBase {
        public override string Code => "logoff";
        public override Type ParserType => typeof (ClientLogoffParser);
    }

    internal class ClientLogoffParser : CommandParser<ClientLogoffCommand> {
        public override async Task WriteAsync(ClientLogoffCommand command, StreamWriter writer) { }

        public override async Task<ClientLogoffCommand> ReadAsync(StreamReader reader) {
            return new ClientLogoffCommand();
        }
    }
}
