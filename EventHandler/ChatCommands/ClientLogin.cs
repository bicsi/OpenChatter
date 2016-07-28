using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public class ClientLogin : ChatCommandBase {
        public override string Code => "login";

        public string Name;
    }

    public class ClientLoginParser : CommandParser<ClientLogin> {
        static ClientLoginParser() {
            CommandParser.AddParser(new ClientLogin().Code, new ClientLoginParser());
        }
        public override async Task Write(ClientLogin command, StreamWriter writer) {
            await writer.WriteLineAsync(command.Name);
        }

        public override async Task<ClientLogin> Read(StreamReader reader) {
            return new ClientLogin {
                Name = await reader.ReadLineAsync()
            };
        }
    }
}
