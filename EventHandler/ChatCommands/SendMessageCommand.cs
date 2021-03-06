using System;
using System.IO;
using System.Threading.Tasks;

namespace CommandHandler.ChatCommands {
    public class SendMessageCommand : ChatCommandBase {
        public string Sender { get; set; }
        public string Destination { get; set; }
        public string Body { get; set; }
        public override string Code => "send";
        public override Type ParserType => typeof(SendMessageParser);
        public DateTime DateSent { get; set; }
    }

    internal class SendMessageParser : CommandParser<SendMessageCommand> {   
           

        public override async Task WriteAsync(SendMessageCommand command, StreamWriter writer) {
            await writer.WriteLineAsync(command.Sender);
            await writer.WriteLineAsync(command.Destination);
            await writer.WriteLineAsync(command.Body);
            await writer.WriteLineAsync(command.DateSent.ToBinary().ToString());
        }

        public override async Task<SendMessageCommand> ReadAsync(StreamReader reader) {
            var result = new SendMessageCommand {
                Sender = await reader.ReadLineAsync(),
                Destination = await reader.ReadLineAsync(),
                Body = await reader.ReadLineAsync(),
                DateSent = DateTime.FromBinary(long.Parse(await reader.ReadLineAsync()))
            };
            return result;
        }
    }
}