using System;
using System.Data;
using System.Data.SqlClient;

namespace tcpServer
{
    public class DataHandler
    {
        /// <summary>
        /// Lock used for methods that access datatable: '<c>DT</c>'
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// Cleans input string and returns the substring from <c>:</c> separator.
        /// <para />
        /// <term>rawData</term>
        /// <param name="rawData">Input Data</param>
        /// <para/>
        /// <term>option</term>
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
        /// Starts timer with an interval predefined from PreferencesStatic.DBUpdateTime. Updates SQL DB at defined time as defined in the prefernces file.
        /// </summary>
        public void DBTimerContext()
        {
            string update = PreferencesStatic.DBUpdateTime;

            System.Timers.Timer _t = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = TimeSpan.FromSeconds(ParseTimeTOSeconds(update)).TotalMilliseconds
            };
            _t.Elapsed += SendTableData;
        }

        /// <summary>
        /// Parses time as:  
        /// <list type="bullet"><item>seconds ( <c>s | S</c> )</item><item>minutes ( <c>m | M</c> )</item><item>hours ( <c>h | H</c> )</item></list>
        /// to seconds.
        /// </summary>
        /// <returns>Seconds as <c>long</c> datatype</returns>
        public double ParseTimeTOSeconds(string time)
        {
            double secR;
            if (time.Contains("s") || time.Contains("S"))
            {
                var sRem = time.Replace("s", "").Replace("S", "");
                return Convert.ToDouble(sRem);
            }
            else if (time.Contains("m") || time.Contains("m"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Convert.ToDouble(mRem) * 60;
                return secR;
            }
            else if (time.Contains("h") || time.Contains("H"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Math.Pow((Convert.ToDouble(mRem) * 60), 2);
                return secR;
            }
            else
            {
                return 0;
            }
        }
        public static int DBNum = 0;
        /// <summary>
        /// Sends data to the DB from the provided connection string in the preferences file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void SendTableData(object source, EventArgs e)
        {
            try
            {
                lock (_lock)
                {
                    using (SqlConnection con = new SqlConnection(PreferencesStatic.ConnectionString))
                    {
                        con.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                        {
                            foreach (DataColumn c in RawDataTable.DT.Columns)
                                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                            bulkCopy.DestinationTableName = "Data_Dump";
                            try
                            {
                                bulkCopy.WriteToServer(RawDataTable.DT);
                                RawDataTable.DT.Clear();
                                DBNum++;
                            }
                            catch (Exception ex) //Writes to console if an error occurs when writing to the db
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        con.Close();
                        int currentpos = Console.CursorTop;
                        Console.SetCursorPosition(0, 6);
                        Console.Write(new string(' ', Console.BufferWidth));
                        Console.Write($"Data sent to database at: {DateTime.Now} | Current DB requsts this session >> {DBNum}             ");
                        Console.SetCursorPosition(0, currentpos);
                    }
                }

            }
            catch (Exception ex)//Throws error if cannot connect to the DB
            {
                Console.WriteLine("Sql Connection Error");
                Console.WriteLine(ex);
                throw new Exception();
            };
        }
        /// <summary>
        /// Stars timer to run <c>QueueHandler()</c> with an interval of 1ms
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

        public void QueueHandler(object thisobj, EventArgs e)
        {
            lock (this)
                try
                {
                    if (DataQueue.Queue.Count != 0)
                    {
                        for (int i = 0; i < 2;)
                        {
                            try
                            {
                                AddToDataTable(DataQueue.Queue.Dequeue());
                                break;
                            }
                            catch
                            {
                                i++;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch
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
            lock (_lock)
            {
                string deviceCode = DataClean(RawData, 1);
                if (PreferencesStatic.DeviceIdExists(deviceCode) == true)
                {
                    RawDataTable.DT.Rows.Add(deviceCode, DataClean(RawData, 2), DataClean(RawData, 3));
                    return true;
                }
                else
                    return false;
            }

        }
    }
}
