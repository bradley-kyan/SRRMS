using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Timers;

namespace tcpServer
{
    public class DataHandler
    {
        private static object _lock = new object();

        /// <summary>
        /// Cleans input string and returns the substring from <c>?</c> separator.
        /// <para />
        /// <c>rawData</c> =>
        /// <param name="rawData">Input Data</param>
        /// <para/>
        /// <c>option</c> =>
        /// <param name="option"><c>1</c> :: Returns lhs substring, <c>2</c> :: Returns middle substring, <c>3</c> :: Returns rhs substring</param>
        /// </summary>
        /// <returns>LHS, MID or RHS input string as <c>string</c></returns>
        private string DataClean(string rawData, byte option)
        {
            string[] array = rawData.Split('?');
            switch (option)
            {
                case 1:
                    return array[0];
                case 2:
                    return array[1];
                case 3:
                    return array[2];
                default:
                    return null;
            }
        }
        /// <summary>
        /// Parses time as:  
        /// <list type="bullet"><item>seconds (<c>s</c>)</item><item>minutes (<c>m</c>)</item><item>hours (<c>h</c>)</item></list>
        /// to seconds.
        /// </summary>
        /// <returns>Seconds as <c>long</c> datatype</returns>
        public double ParseTimeTOSeconds(string time) //All it does is convert the input to seconds
        {
            double secR;
            if (time.Contains("s") || time.Contains("S"))
            {
                var sRem = time.Replace("s", "").Replace("S", "");
                return Convert.ToDouble(sRem); //Already in seconds format, do nothing
            }
            else if (time.Contains("m") || time.Contains("m"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Convert.ToDouble(mRem) * 60;//Convert Minutes to seconds
                return secR;
            }
            else if (time.Contains("h") || time.Contains("H"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Convert.ToDouble(mRem) * 3600;//convert hours to seconds
                return secR;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Starts timer with an interval predefined from PreferncesStatic.DBUpdateTime. Updates SQL DB at defined time.
        /// </summary>
        public void DBTimerContext()
        {
            string update = PreferncesStatic.DBUpdateTime;

            System.Timers.Timer _t = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = TimeSpan.FromSeconds(ParseTimeTOSeconds(update)).TotalMilliseconds
            };
            _t.Elapsed += SendTableData;
        }
        public static int DBNum = 0;
        /// <summary>
        /// Sends data from the datatable to the SQL Server database through the connection string defined in the prefernces file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendTableData(object source, EventArgs e)
        {
            try
            {
                lock (_lock)//locks method to keep thread safe
                {

                    using (SqlConnection con = new SqlConnection(PreferncesStatic.ConnectionString))//Connects to DB through prefereneces connection string
                    {
                        con.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))//dumps the data in the datatable into the Data_Dump table located in the SQL DB
                        {
                            foreach (DataColumn c in RawDataTable.DT.Columns)
                                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                            bulkCopy.DestinationTableName = "Data_Dump";
                            try
                            {
                                bulkCopy.WriteToServer(RawDataTable.DT);
                                RawDataTable.DT.Clear(); //clears the datatable
                                DBNum++;//used for statistics to show amount of DB access per session
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        con.Close();
                        int currentpos = Console.CursorTop;
                        Console.SetCursorPosition(0, 6);
                        Console.Write(new string(' ', Console.BufferWidth));
                        Console.Write($"Data sent to database at: {DateTime.Now} | Current DB requsts this session >> {DBNum}             ");//Writes statistics to console
                        Console.SetCursorPosition(0, currentpos);

                    }
                }

            }
            catch
            {
                Console.WriteLine("Sql Connection Error");
            };
        }
        /// <summary>
        /// Stars timer to run QueueHandler with an interval of 1ms
        /// </summary>
        public void QueueTimerContext()
        {
            System.Timers.Timer _t = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1
            };
            _t.Elapsed += QueueHandler;
        }
        /// <summary>
        /// Method run by QueueTimerContext()
        /// </summary>
        public void QueueHandler(object thisobj, EventArgs e)
        {
            lock (this)//locks method to prevent multiple executions if exection time is slower than QueueTimerFContext() method call
                try
                {
                    if (DataQueue.Queue.Count != 0)//checks if Queue is empty
                    {
                        for (int i = 0; i < 2;)//Tries twice to dequeue to prevent spaghetti
                        {
                            try
                            {
                                AddToDataTable(DataQueue.Queue.Dequeue());
                                break;
                            }
                            catch
                            {
                                i++;//if error occurs try again
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch //runs if method cannot be locked
                {
                    return;
                }
        }
        /// <summary>
        /// Adds input data to DataTable.
        /// </summary>
        /// <returns><c>True</c> if successful execution. <c>Fasle</c> if unsuccessful</returns>
        public bool AddToDataTable(string RawData)
        {
            lock (_lock) //locks method to prevent multiple executions to keep thread safe
            {
                string deviceCode = DataClean(RawData, 1);
                if (PreferncesStatic.DeviceIdExists(deviceCode) == true)//check if the device code of the sender is valid
                {
                    RawDataTable.DT.Rows.Add(deviceCode, DataClean(RawData, 2), DataClean(RawData, 3));//Adds data to datatable
                    return true;
                }
                else
                    return false;
            }

        }
    }
}
