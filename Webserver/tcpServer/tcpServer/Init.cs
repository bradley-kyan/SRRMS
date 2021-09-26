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
    /// <summary>
    /// Base prefernces used during cli prefernces management
    /// </summary>
    public class Prefernces
    {
        public DateTime UpdateTime { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public IList<string> DeviceIds { get; set; }
        public IList<string> DeviceNote { get; set; }
        public string DBUpdateTime { get; set; }
        
        public Prefernces()
        {
            DeviceIds = new List<string>();
            DeviceNote = new List<string>();
        }
    }
    /// <summary>
    /// Static version of prefernces to be accesed during runtime
    /// </summary>
    public static class PreferncesStatic
    {
        public static string DbType;
        public static string ConnectionString;
        public static IList<string> DeviceIds;
        public static string DBUpdateTime;
        static PreferncesStatic()
        {
            DeviceIds = new List<string>();
        }

        public static void UpdateValues(string dbType, string connectionString, string dbUpdateTime)
        {
            DbType = dbType;
            ConnectionString = connectionString;
            DBUpdateTime = dbUpdateTime;
        }
        public static void DeviceAdd(string value)
        {
            // Record this value in the list.
            DeviceIds.Add(value);
        }
        public static void ListIds()
        {
            foreach(string DeviceIds in DeviceIds)
            {
                Console.WriteLine(DeviceIds);
            }
        }
        public static bool DeviceIdExists(string DeviceID)
        {
            if (DeviceIds.Contains(DeviceID))
                return true;
            else
                return false;
        }

    }
    /// <summary>
    /// Queue used to store scanned data awaiting to be pushed to datatable
    /// </summary>
    public static class DataQueue
    {
        public static Queue<string> Queue;//Require to add to queue since DataTable is not thread safe
        static DataQueue()
        {
            Queue = new Queue<string>();
        }
    }
    /// <summary>
    /// Data Table used to store scanned data from readers
    /// </summary>
    public static class RawDataTable
    {
        public static DataTable DT;
        static RawDataTable()
        {
            DT = new DataTable();
            DT.Columns.Add("C_DeviceID", typeof(string));
            DT.Columns.Add("Card_ID", typeof(string));
            DT.Columns.Add("In_Time", typeof(DateTime));
        }

        /// <summary>
        /// Returns and writes current data table data to cli
        /// </summary>
        public static void List()
        {
            foreach (DataRow dataRow in DT.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}

