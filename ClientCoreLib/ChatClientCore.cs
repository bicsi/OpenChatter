using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ClientCoreLib {
    public class ChatClientCore : IDisposable {

        /// <summary>
        /// Starts the whole process
        /// </summary>

        private Connection connection;
        private CommandLineListener listener;

        public IPAddress IP { get; private set; }
        public int Port { get; private set; }
        public bool Stopped { get; set; }

        public ChatClientCore(IPAddress ipAddress, int port) {
            IP = ipAddress;
            Port = port;
            Stopped = false;

            listener = new CommandLineListener(OnUserTypedCommand);
            connection = new Connection(listener.Start);
        }

        private void OnUserTypedCommand(ChatCommand obj) {
            connection.SendCommand(obj);
        }

        public void Start() {
            connection.Start(IP, Port);
        }

        public void Stop() {
            connection.Stop();
            Stopped = true;
        }

        public void Dispose() {
            Stop();
        }
    }
}
