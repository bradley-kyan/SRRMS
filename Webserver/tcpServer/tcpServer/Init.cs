using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Data;

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
            Thread.Sleep(200);
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
        public IList<string> DBUpdateTime { get; set; }
        public Prefernces()
        {
            DeviceIds = new List<string>();
            DeviceNote = new List<string>();
            DBUpdateTime = new List<string>();
        }
    }

    public class RawDataTable
    {
        public DataTable DT { get; set; } = new DataTable();
        public RawDataTable()
        {
            DT.Columns.Add("C_DeviceID", typeof(string));
            DT.Columns.Add("Card_ID", typeof(string));
            DT.Columns.Add("In_Time", typeof(DateTime));
        }
    }
}

