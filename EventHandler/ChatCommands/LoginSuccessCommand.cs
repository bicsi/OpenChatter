using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class LoginSuccessCommand : ChatCommandBase {
        public override string Code => "conn_success";
        public override Type ParserType => typeof (LoginSuccessParser);
    }

    internal class LoginSuccessParser : CommandParser<LoginSuccessCommand> {
        public override async Task WriteAsync(LoginSuccessCommand command, StreamWriter writer) { }

        public override async Task<LoginSuccessCommand> ReadAsync(StreamReader reader) {
            return new LoginSuccessCommand();
        }
    }
}
