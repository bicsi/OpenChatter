using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler {
    public class ChatCommand {
        public string Type { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }
    }
}
