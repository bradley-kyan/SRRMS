using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace tcpServer
{
    public class ServerInitializer : DevicePref
    {
        internal void Initializer()
        {
            Header(1);
            Thread.Sleep(1000);
            Console.WriteLine("Initalising...");
            Thread t1 = new Thread(new DataHandler().QueueTimerContext);
            Thread t2 = new Thread(AsynchronousSocketListener.StartListening);
            t1.Start();
            t2.Start();
        }
    }
}
