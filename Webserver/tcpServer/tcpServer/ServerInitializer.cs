using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tcpServer
{
    internal class ServerInitializer : DevicePref
    {
        internal void Initializer()
        {
            Header(1);
            Console.WriteLine("Initalising...");
        }
    }
}
