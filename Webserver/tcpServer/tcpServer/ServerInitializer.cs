using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace tcpServer
{
    public class ServerInitializer : DevicePref
    {
        /// <summary>
        /// Called to start the listening server and run data handling methods
        /// </summary>
        internal void Initializer()
        {
            Header(1);
            Thread.Sleep(1000);
            Console.WriteLine("Initalising...");
            Deserializer();
            Thread t1 = new Thread(new DataHandler().QueueTimerContext);
            Thread t3 = new Thread(new DataHandler().DBTimerContext);
            Thread t2 = new Thread(AsynchronousSocketListener.StartListening);
            t1.Start();
            t3.Start();
            t2.Start();
        }
    }
}
