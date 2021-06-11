﻿using System;
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
        public Queue<string> DataQueue { get; set; }//Require to add to queue since DataTable is not thread safe

        /// <summary>
        /// Cleans input string and returns the substring from <c>:</c> separator.
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
            string[] array = rawData.Split(':');
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
        /// 
        /// </summary>
        private void DataTimer()
        {
            var p = new Prefernces();
            string update = p.DBUpdateTime;

            System.Timers.Timer _timer = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = TimeSpan.FromSeconds(ParseTimeTOSeconds(update)).TotalMilliseconds
            };
            _timer.Elapsed += SendTableData;

        }

        /// <summary>
        /// Parses time as:  
        /// <list type="bullet"><item>seconds (s)</item><item>minutes (m)</item><item>hours (h)</item></list>
        /// to seconds.
        /// </summary>
        /// <returns>Seconds as <c>long</c> datatype</returns>
        public long ParseTimeTOSeconds(string time)
        {
            long secR;
            if (time.Contains("s") || time.Contains("S"))
            {
                var sRem = time.Replace("s", "").Replace("S","");
                return Convert.ToInt64(sRem);
            }
            else if(time.Contains("m") || time.Contains("m"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Convert.ToInt64(mRem) * 60;
                return secR;
            }   
            else if(time.Contains("h") || time.Contains("H"))
            {
                var mRem = time.Replace("m", "").Replace("M", "");
                secR = Convert.ToInt64(mRem) * 60^2;
                return secR;
            }
            else
            {
                return 0;
            }
        }

        private void SendTableData(object source, EventArgs e)
        {
            Prefernces p = new Prefernces();
            RawDataTable r = new RawDataTable();
            try
            {//Data Source=tpisql01.avcol.school.nz;Initial Catalog=SRRMS_DB;Integrated Security=True
                using (SqlConnection con = new SqlConnection(p.ConnectionString))
                {
                    con.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        foreach (DataColumn c in r.DT.Columns)
                            bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);

                        bulkCopy.DestinationTableName = "Data_Dump";
                        try
                        {
                            bulkCopy.WriteToServer(r.DT);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    con.Close();
                    Console.WriteLine($"Suceesfully sent {r.DT} to database at: {DateTime.Now}");
                }
            }
            catch
            {
                Console.WriteLine("Sql Connection Error");

            };
        }

        public void QueueHandler()
        {
            var obj = 1;
            Monitor.Enter(obj);
            try
            {
                for (int i = 0; i < 2;)
                {
                    bool exit = AddToDataTable(DataQueue.Peek());
                    if (exit == true)
                    {
                        DataQueue.Dequeue();
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }

        /// <summary>
        /// Adds input data to DataTable.
        /// </summary>
        /// <returns><c>True</c> if successful execution. <c>Fasle</c> if unsuccessful</returns>
        public bool AddToDataTable(string RawData)
        {
            var p = new Prefernces();
            RawDataTable r = new RawDataTable();
            string deviceCode = DataClean(RawData, 1);
            if (VerifySender(deviceCode) is true)
            {
                r.DT.Rows.Add(deviceCode, DataClean(RawData, 2), DataClean(RawData, 3));
                return true;
            }
            else
                return false;
        }

        private static bool VerifySender(string input)
        {
            Prefernces p = new Prefernces();
            if (p.DeviceIds.Contains(input))
                return true;
            else
                return false;
        }
    }
}
