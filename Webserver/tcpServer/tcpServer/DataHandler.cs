using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

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
        /// <param name="option"><c>1</c> :: Returns substring lhs, <c>2</c> :: Returns substring rhs</param>
        /// </summary>
        /// <returns>LHS or RHS input string as <c>string</c></returns>
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
                    throw new ArgumentException();
            }
        }

        private void SendTableData()
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
            int x = 1;
            Monitor.Enter(x);
            try
            {
                bool exit = AddToDataTable(DataQueue.Dequeue());
                if (exit == true)
                    Monitor.Exit(x);
            }
            finally
            {
                Monitor.Exit(x);
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
