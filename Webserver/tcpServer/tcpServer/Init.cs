using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing;

namespace tcpServer
{
    class Init
    {
        const string srrmsTitle = "  ______________\n |              |\n | SRRMS Server |\n |______________| \n";
        static string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void startup()
        {
            Console.Title = "SRRMS Server";

            Console.WriteLine(srrmsTitle);

            if(File.Exists(currentPath + "/preferences.json"))
            {
                Console.WriteLine("preferences.json found\n\nPress any key to continue...");
                Console.ReadKey();
                startServer();

            }
            else
            {
                Console.WriteLine("preferences.json not found\n");
                File.Create("preferences.json");
                Console.WriteLine("Created preferences.json\n\nPress any key to continue...");
                Console.ReadKey();
                startServer();
            }
        }

        static void startServer()
        {
            Console.Clear();
            Console.WriteLine(srrmsTitle);
            Console.WriteLine("Listening Server Started\n________________________\n\n");

        }
    }
}
