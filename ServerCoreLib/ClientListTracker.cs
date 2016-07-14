using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoreLib {
    internal class ClientListTracker {

        private ConcurrentDictionary<string, ServerConnection> clientDictionary;

        /// <summary>
        /// Constructor for the ClientListTracker class
        /// </summary>
        public ClientListTracker() {

            clientDictionary = new ConcurrentDictionary<string, ServerConnection>();
        }

        /// <summary>
        /// Adds a connection (client) to the dictionary for fast access / tracing
        /// </summary>
        /// <param name="conn"></param>
        public void AddConnection(ServerConnection conn) {
            string name = conn.Name;

            clientDictionary[name] = conn;

            Console.WriteLine($"Added connection {conn}.");
        }

        /// <summary>
        /// Removes a (disconnected) connection (client) from the dictionary
        /// </summary>
        /// <param name="conn"></param>
        public void RemoveConnection(ServerConnection conn) {
            string name = conn.Name;

            while (clientDictionary.ContainsKey(name))
                clientDictionary.TryRemove(name, out conn);

            Console.WriteLine($"Removed connection {conn}");
        }

        /// <summary>
        /// Gets a connection (client) by username
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ServerConnection GetClientByName(string name) {

            if (!clientDictionary.ContainsKey(name))
                throw new KeyNotFoundException("The user has been removed");

            ServerConnection ret;
            if (!clientDictionary.TryGetValue(name, out ret))
                throw new KeyNotFoundException("The user could not be accessed");

            // TODO: Check if ret is active

            return ret;
        }

    }
}
