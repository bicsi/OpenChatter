using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class NopCommand : ChatCommandBase {
        public override string Code => "nop";
        public override Type ParserType => typeof (NopParser);
    }

    internal class NopParser : CommandParser<NopCommand> {
        public override async Task WriteAsync(NopCommand command, StreamWriter writer) { }

        public override async Task<NopCommand> ReadAsync(StreamReader reader) {
            return new NopCommand();
        }
    }
}
