using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;
using System.Text.Json;
using System.Collections;
using System.Linq;

namespace tcpServer
{
    public class Init
    {
        public const string options = " 1: Setup\n 2: Devices\n 3: Start server\n_____________________________\n\n F12: Exit";
        public static string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string pLocate = currentPath + "/preferences.json";

        public static void startup()
        {
            DevicePref s = new DevicePref();

            s.Header(1);

            if (File.Exists(pLocate))
            {
                Console.WriteLine(" preferences.json found\n\n");
            }
            else
            {
                Console.WriteLine(" preferences.json not found\n");
                s.Serializer();
                Console.WriteLine(" Created preferences.json\n\n");
            }
            Thread.Sleep(1000);
            s.OptionSelect();

        }
    }

    public class Prefernces
    {
        public DateTime UpdateTime { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public IList<string> DeviceIds { get; set; }
        public IList<string> DeviceNote { get; set; }
        public Prefernces()
        {
            DeviceIds = new List<string>();
            DeviceNote = new List<string>();
        }
    }
}
