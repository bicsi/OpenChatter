using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models {
    public abstract class Message : ModelBase {
        public string Body { get; set; }
        public DateTime DateSent { get; set; }

        public override abstract string ToString();
    }

    public class MyMessage : Message {
        public User To { get; set; }

        public override string ToString() {
            return $"[{DateSent}] You: {Body}";
        }
    }

    public class TheirMessage : Message {
        public User From { get; set; }

        public override string ToString() {
            return $"[{DateSent}] {From}: {Body}";
        }
    }
}
