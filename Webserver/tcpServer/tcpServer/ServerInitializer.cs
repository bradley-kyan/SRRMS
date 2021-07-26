using System;
using System.Threading;
namespace tcpServer
{
    public class ServerInitializer : DevicePref
    {
        /// <summary>
        /// Starts the TCP server with critical processes run on its own thread for simultaneous execution
        /// </summary>
        internal void Initializer()
        {
            Header(1);
            Thread.Sleep(1000);
            Console.WriteLine("Initalising...");
            Deserializer();
            Thread t1 = new Thread(new DataHandler().QueueTimerContext);
            Thread t2 = new Thread(new DataHandler().DBTimerContext);
            Thread t3 = new Thread(AsynchronousSocketListener.StartListening);
            t1.Start();
            t2.Start();
            t3.Start();
        }
    }
}
