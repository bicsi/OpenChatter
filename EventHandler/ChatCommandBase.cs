using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public abstract class ChatCommandBase {
        public abstract string Code { get; }
        public abstract Type ParserType { get; }
    }
}
