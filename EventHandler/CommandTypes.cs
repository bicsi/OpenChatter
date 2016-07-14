using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public class ServerCommandType {
        public const string SendNameRequest = "name?";
        public const string SendConnectACK = "conn_success";
        public const string SendConnectFail = "conn_fail";
    }

    public class ClientCommandType {
        public const string ReceiveNameRequest = "name";
        public const string ReceiveDisconnect = "killed";
    }

    
}
