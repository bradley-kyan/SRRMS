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
    /// Used for getting/updating data from the preferences file -> in use due to json serializer/deserializer limitations
    /// </summary>
    public class Preferences
    {
        public DateTime UpdateTime { get; set; }
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public IList<string> DeviceIds { get; set; }
        public IList<string> DeviceNote { get; set; }
        public string DBUpdateTime { get; set; }
        
        public Preferences()
        {
            DeviceIds = new List<string>();
            DeviceNote = new List<string>();
        }
    }
    /// <summary>
    /// Static version of preferences class for use when server starts
    /// </summary>
    public static class PreferencesStatic
    {
        public static string DbType;
        public static string ConnectionString;
        public static IList<string> DeviceIds;
        public static string DBUpdateTime;
        static PreferencesStatic()
        {
            DeviceIds = new List<string>();
        }
        /// <summary>
        /// Updates the static variables from the input data
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="dbUpdateTime"></param>
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
        /// <summary>
        /// Prints all currently stored device ID in the preferences file to the console
        /// </summary>
        public static void ListIds() 
        {
            foreach(string DeviceIds in DeviceIds)
            {
                Console.WriteLine(DeviceIds);
            }
        }
        /// <summary>
        /// Returns True or False if the current ID is included in the preferences file
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public static bool DeviceIdExists(string DeviceID)
        {
            if (DeviceIds.Contains(DeviceID))
                return true;
            else
                return false;
        }

    }
    /// <summary>
    /// Queue used for when new data is recieved from clients.
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
    /// Datatable used to stored 'cleaned' data stored in the queue -> <c>DataQueue</c>
    /// </summary>
    public static class RawDataTable
    {
        /// <summary>
        /// Datatable with values: 
        /// <list type="table">
        /// <item><term>C_DeviceID</term> Device ID of client</item>
        /// <item><term>Card_ID</term> UID of the scanned card</item>
        /// <item><term>In_Time</term> Time the card was scanned from client</item>
        /// </list>
        /// </summary>
        public static DataTable DT;
        static RawDataTable()
        {
            DT = new DataTable();
            DT.Columns.Add("C_DeviceID", typeof(string));
            DT.Columns.Add("Card_ID", typeof(string));
            DT.Columns.Add("In_Time", typeof(DateTime));
        }

        /// <summary>
        /// Lists all the currently stored data in the datatable to the console
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

