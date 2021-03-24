using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace tcpServer
{
    class Program
    {
        public static void Main(String[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 10932);

            try
            {
                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(100);

                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();

                // Incoming data from the client.    
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("") > -1)
                    {
                        break;
                    }
                }

                //Check message => 'data'
                char[] sbArray = data.ToCharArray();

                char[] headerElem = new char[5];

                Array.Copy(sbArray, 0, headerElem, 0, 4);

                Console.WriteLine(headerElem[4]);



                //Post data handling

          
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
