using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler {
    public class ActiveUsersCommand : ChatCommandBase {
        public List<string> Users { get; set; }
        public override string Code => "users";
    }
    public class ActiveUsersParser : CommandParser<ActiveUsersCommand> {
        static ActiveUsersParser() {
            CommandParser.AddParser(new ActiveUsersCommand().Code, new SendMessageParser());
        }

        public override async Task Write(ActiveUsersCommand command, StreamWriter writer) {
            await writer.WriteLineAsync(string.Join(" ", command.Users));            
        }

        public override async Task<ActiveUsersCommand> Read(StreamReader reader) {
            var result = new ActiveUsersCommand();
            var usr = await reader.ReadLineAsync();
            result.Users = new List<string>(usr.Split(' '));
            return result;
        }
    }
}