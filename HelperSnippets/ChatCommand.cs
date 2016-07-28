using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public interface IParseData {
        void Read(StreamReader stream);
        void Write(StreamWriter stream);
    }

    public abstract class ChatCommandBase {
        public string CommandName {
            get {
                var type = typeof(CommandType);
                var memInfo = type.GetMember(Type.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                    false);
                return ((DescriptionAttribute)attributes[0]).Description;
            }
        }

        public CommandType Type {
            get { return CommandAssociationManager.GetCmdType(this.GetType()); }

        }

        public class AnonymousParser {
            public ChatCommandBase Read(StreamReader reader) {
                var tp = reader.ReadLine();
                CommandType cmdType;
                if (!Enum.TryParse(tp, out cmdType))
                    throw new Exception("Invalid command type " + cmdType);
                var msgType = CommandAssociationManager.MessageTypes[cmdType];
                return (ChatCommandBase)CommandAssociationManager.Parsers[cmdType].Read(reader);
            }

            public void Write(StreamWriter str, ChatCommandBase command) {
                CommandAssociationManager.Parsers[command.Type].Write(str);
            }
        }
        
        public enum CommandType {

            [Description("HelloYou")]
            Hello,
            [Description("SendMessage")]
            Send,
            History,
            CreateRoom,
            Rename
        }

        public class CreateRoomMessageParser : ParserBase<CreateRoomMessage> {
            public override void Write(StreamWriter str, CreateRoomMessage obj) {
                
            }

            public override CreateRoomMessage Read(StreamReader str) {
                throw new NotImplementedException();
            }
        }

        public class MessageContainer {
            
        }


        public interface IParser {
            object Read(StreamReader str);
            void Write(StreamWriter str, object obj);
        }

        public abstract class ParserBase<T> : IParser  where T : ChatCommandBase {
            public abstract T Read(StreamReader str);
            public void Write(StreamWriter str, object obj) {
                Write(str, (T) obj);
            }

            public abstract void Write(StreamWriter str, T obj);
            object IParser.Read(StreamReader str) {
                return Read(str);
            }
        }

      

        [CommandTypeAssociation(CommandType.Send)]
        public class ChatCommand : ChatCommandBase {
            public string Sender { get; set; }
            public string Recipient { get; set; }
            public string Content { get; set; }
        }

        public class GenericParser<T> : IParser where T : ChatCommandBase {
            public T Read(StreamReader str) {
                throw new NotImplementedException();
            }

            public void Write(StreamWriter str, T obj) {
                throw new NotImplementedException();
            }
        }


        [CommandTypeAssociation(CommandType.Send)]
        public class ChatCommandParser : ParserBase<ChatCommand> {
            public override ChatCommand Read(StreamReader str) {
                throw new NotImplementedException();
            }


            public override void Write(StreamWriter str, ChatCommand obj) {
                throw new NotImplementedException();
            }

        }



        public class CommandTypeAssociationAttribute : Attribute {
            public CommandType Type { get; private set; }

            public CommandTypeAssociationAttribute(CommandType type) {
                Type = type;
            }
        }

        [CommandTypeAssociation(CommandType.Hello)]
        public class ChatHelloUserName {
            public string Name { get; set; }
        }

        public class CommandAssociationManager {
            public static Dictionary<CommandType, Type> MessageTypes = new Dictionary<CommandType, Type>();
            public static Dictionary<CommandType, IParser> Parsers = new Dictionary<CommandType, IParser>();

            public static CommandType GetCmdType(Type type) {
                var attr = type.GetCustomAttributes(typeof (CommandTypeAssociationAttribute), true);
                return ((CommandTypeAssociationAttribute) attr[0]).Type;
            }

            public static void InitTypes() {
                var baseType = typeof (ChatCommandBase);
                MessageTypes =
                    baseType.Assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(baseType))
                        .ToDictionary(t => GetCmdType(t), t => t);

                var parserInterface = typeof(ParserBase<>);
/*
                Parsers =
                    parserInterface.Assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(baseType))
                        .ToDictionary(t => GetCmdType(t), t => t);
*/

            }

            public ChatCommandBase GetMessaageInstance(CommandType type) {
                return (ChatCommandBase)Activator.CreateInstance(MessageTypes[type]);
            }

            public static ChatCommandBase Test(StreamReader sr) {
                
            }
        }

        public abstract void Read(StreamReader stream);
        public abstract void Write(StreamWriter stream);
    }

    public class CreateRoomMessage : ChatCommandBase {
    }
}
