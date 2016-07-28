using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler {
    public class SendMessageCommand : ChatCommandBase {
        public string Sender { get; set; }
        public string Destination { get; set; }
        public string Body { get; set; }
        public override string Code => "send";
    }

    public class SendMessageParser : CommandParser<SendMessageCommand> {   
        static SendMessageParser() {
                CommandParser.AddParser(new SendMessageCommand().Code, new SendMessageParser());
        }   

        public override async Task Write(SendMessageCommand command, StreamWriter writer) {
            await writer.WriteLineAsync(command.Sender);
            await writer.WriteLineAsync(command.Destination);
            await writer.WriteLineAsync(command.Body);
        }

        public override async Task<SendMessageCommand> Read(StreamReader reader) {
            var result = new SendMessageCommand {
                Sender = await reader.ReadLineAsync(),
                Destination = await reader.ReadLineAsync(),
                Body = await reader.ReadLineAsync()
            };
            return result;
        }
    }
}