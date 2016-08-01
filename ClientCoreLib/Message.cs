using System;

namespace ClientCoreLib {
    public abstract class Message {
        public string Body;
        public DateTime DateSent { get; set; }
    }

    public class ReceivedMessage : Message {
        public string From;
    }

    public class SentMessage : Message {
        public string To;
    }
}