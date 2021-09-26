using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Buffers.Text;

namespace tcpServer
{

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
   

    public class AsynchronousSocketListener
    {
        public string bound;

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener()
        {
        }
        public static void StartListening()
        {
            int portNum = 29882;
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = Array.Find(ipHostInfo.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNum);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            Retry:
            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                Console.Clear();
                new DevicePref().Header(1);
                Console.WriteLine($"Server started on {ipHostInfo.HostName}({ipAddress}):{localEndPoint.Port}\n\n\n");
                new AsynchronousSocketListener().bound = $"Server started on {ipHostInfo.HostName}({ipAddress}):{localEndPoint.Port}\n\n\n";

                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                new DevicePref().Header(1);
                Console.WriteLine(e.ToString() + $"\n\nTrying to bind port to port {portNum}");
                portNum++;
                Thread.Sleep(1500);
                localEndPoint = new IPEndPoint(ipAddress, portNum);
                goto Retry;
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead)); 
                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    DataQueue.Queue.Enqueue(content.Replace("<EOF>", ""));
                    Send(handler, $"HTTP/1.1 200 OK\nDate: {DateTime.Now}");//Authorized device, return OK
                    handler.Close();//close connection
                }
                else if (PreferncesStatic.DeviceIds.Contains(content.Split(':')[0]) == false)
                {
                    Console.Clear();

                    Console.WriteLine(new AsynchronousSocketListener().bound);
                    Console.WriteLine("auth.error >> " + handler.RemoteEndPoint);
                    Send(handler, $"HTTP/1.1 403 Forbidden\nDate: {DateTime.Now}");//Unauthorized device, tell device it is unauthorized
                    handler.Close();//close connection
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }
        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        public static int ReqNum = 0;
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                ReqNum++;
                Console.WriteLine("Total requests this session >> {0}                        ", ReqNum);
                Console.WriteLine("Sent {0} bytes to client >> {1}                           ", bytesSent, handler.RemoteEndPoint);
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                Thread.Sleep(50);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                handler.Dispose();
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
