﻿using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerCoreLib;

namespace OpenChatterServer {
    class Program {
        static void Main(string[] args) {
            // Write welcome message
            Console.WriteLine("Welcome to OpenChatterServer!");

            // Initialize the "core" ChatServerCore object
            int port = 3333;
            ChatServerCore server = new ChatServerCore(port);

            // Start the server
            server.Start();

            Console.ReadLine();
        }
    }
}
