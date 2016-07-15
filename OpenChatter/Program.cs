using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientCoreLib;

namespace OpenChatterClient {
    class Program {
        public static void Main(string[] args) {
            

            IPAddress ipAddr = IPAddress.Parse(args[0]);
            int port = int.Parse(args[1]);

            ChatClientCore client = new ChatClientCore(ipAddr, port);

            client.Start();
            while(!client.Stopped)
                Thread.Sleep(1000);
        }
    }
}
