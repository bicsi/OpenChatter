using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
   public class ActiveUsersCommand : ChatCommandBase {
        public List<string> Users { get; set; }

        public override string Code => "users";
        public override Type ParserType => typeof (ActiveUsersParser);
   }
    
    internal class ActiveUsersParser : CommandParser<ActiveUsersCommand> {
        
        public override async Task WriteAsync(ActiveUsersCommand command, StreamWriter writer) {
            if(command.Users != null)
                await writer.WriteLineAsync(string.Join(" ", command.Users));            
        }

        public override async Task<ActiveUsersCommand> ReadAsync(StreamReader reader) {
            var result = new ActiveUsersCommand();
            var usr = await reader.ReadLineAsync();
            result.Users = new List<string>(usr.Split(' '));
            return result;
        }
    }
}