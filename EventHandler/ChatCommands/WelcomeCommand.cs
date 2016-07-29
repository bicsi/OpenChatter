using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class WelcomeCommand : ChatCommandBase {
        public string Body;
        public override string Code => "welcome";
        public override Type ParserType => typeof (WelcomeParser);
    }

    internal class WelcomeParser : CommandParser<WelcomeCommand> {
        
        public override async Task WriteAsync(WelcomeCommand command, StreamWriter writer) {
            await writer.WriteLineAsync(command.Body);
        }

        public override async Task<WelcomeCommand> ReadAsync(StreamReader reader) {
            return new WelcomeCommand {
                Body = await reader.ReadLineAsync()
            };
        }
    }
}
