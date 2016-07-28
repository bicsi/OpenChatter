using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public abstract class MessageBase {
        public abstract ChatCommandBase.CommandType MessageType { get; }
    }

    public class SendMessage : MessageBase {
        public override ChatCommandBase.CommandType MessageType { get { return ChatCommandBase.CommandType.Send;} }
        public string Destination { get; set; }
        public string Body { get; set; }
    }

    public interface IParse {
        MessageBase Read(StreamReader sr);
        void Write(StreamWriter sr, MessageBase msg);
    }
    public abstract class ParserBase<T> : IParse where T : MessageBase {
        public abstract T Read(StreamReader sr);
        public void Write(StreamWriter sr, MessageBase msg) {
            Write(sr, (T)msg);
        }

        public abstract void Write(StreamWriter sr, T msg);

        MessageBase IParse.Read(StreamReader sr) {
            return Read(sr);
        }
    }
    public class SendMessageParser : ParserBase<SendMessage> {
        public override SendMessage Read(StreamReader sr) {
            throw new NotImplementedException();
        }

        public override void Write(StreamWriter sr, SendMessage msg) {
            throw new NotImplementedException();
        }
    }


    public class ParserFactory {
        public static ParserFactory Instance = new ParserFactory();

        private ParserFactory() {
            
        }

        private Dictionary<ChatCommandBase.CommandType, IParse> parsers = new Dictionary<ChatCommandBase.CommandType, IParse>();
        public IParse GetParser(ChatCommandBase.CommandType type) {
            return parsers[type];
        }

        public void AddParser(ChatCommandBase.CommandType type, IParse parser) {
            parsers.Add(type, parser);
        }
    }

    public class ParentParser : IParse {
        public MessageBase Read(StreamReader reader) {
            var tp = reader.ReadLine();
            ChatCommandBase.CommandType cmdType;
            if (!Enum.TryParse(tp, out cmdType))
                throw new Exception("Invalid command type " + cmdType);
            return ParserFactory.Instance.GetParser(cmdType).Read(reader);
        }

        public void Write(StreamWriter sr, MessageBase msg) {
            sr.WriteLine(msg.MessageType.ToString());
            ParserFactory.Instance.GetParser(msg.MessageType).Write(sr, msg);
        }
    }   
    public static class Prog {
        public static void Main() {
            ParserFactory.Instance.AddParser(ChatCommandBase.CommandType.Send, new SendMessageParser());
            ParserFactory.Instance.AddParser(ChatCommandBase.CommandType.Rename, new RenameMessageParser());
        }
    }

    public class RenameMessageParser : ParserBase<RenameMessage> {
        public override void Write(StreamWriter sr, RenameMessage msg) {
            sr.WriteLine(msg.Name);
        }

        public override RenameMessage Read(StreamReader sr) {
            return new RenameMessage {
                Name = sr.ReadLine()
            };

        }
    }

    public class RenameMessage : MessageBase {
        public override ChatCommandBase.CommandType MessageType { get {return ChatCommandBase.CommandType.Rename;} }
        public string Name { get; set; }
    }
}
