using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;

namespace tcpServer
{
    class Init
    {
        const string srrmsTitle = "  ______________\n |              |\n | SRRMS Server |\n |______________| \n";
        const string options = "  __________________\n |   |              |\n | 1 | Setup        |\n | 2 | Start server |\n |___|______________|";
        static string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void startup()
        {
            Console.Title = "SRRMS Server";
            Console.WriteLine(srrmsTitle);
            if (File.Exists(currentPath + "/preferences.json"))
            {
                Console.WriteLine("preferences.json found\n\n");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("preferences.json not found\n");
                File.Create("preferences.json");
                Console.WriteLine("Created preferences.json\n\n");

            }
        }
    }

    class Setup : Init
    {


    }

}
