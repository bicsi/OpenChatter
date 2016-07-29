using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public interface ICommandParserBase {
        Task Send(ChatCommandBase command, StreamWriter writer);
        Task<ChatCommandBase> Receive(StreamReader reader);
    }

    internal abstract class CommandParser<T> : ICommandParserBase where T : ChatCommandBase {

        public abstract Task WriteAsync(T command, StreamWriter writer);
        public async Task Send(ChatCommandBase command, StreamWriter writer) {
            await WriteAsync((T) command, writer);
        }
        
        public abstract Task<T> ReadAsync(StreamReader reader);
        public async Task<ChatCommandBase> Receive(StreamReader reader) {
            return await ReadAsync(reader);
        }

    }
}
