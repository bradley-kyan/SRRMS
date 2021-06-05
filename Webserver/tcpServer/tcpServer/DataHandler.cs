using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            if (option == 1)
                return rawData[rawData.IndexOf(':')..];
            else if (option == 2)
                return rawData[(rawData.IndexOf(':') + 1)..];
            else
                throw new ArgumentException();
        }

        private void ConnectionOpen(string procedureName)
        {
            Prefernces p = new Prefernces();
            try
            {
                using (SqlConnection sqlConnection1 = new SqlConnection(p.ConnectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = sqlConnection1;
                    sqlConnection1.Open();
                }
            }
            catch
            {
                Console.WriteLine("Sql Connection Error");
            };
        }

        /// <summary>
        /// Adds input data to DataTable.
        /// </summary>
        /// <returns><c>0</c> if successful input. <c>1</c> if string is invalid</returns>
        public byte AddToDataTable(string RawData)
        {
            var p = new Prefernces();
            RawDataTable r = new RawDataTable();
            string deviceCode = DataClean(RawData, 1);
            if (VerifySender(deviceCode) is true)
            {
                r.DT.Rows.Add(deviceCode, DataClean(RawData, 2), DateTime.Now);
                return 0;
            }
            else
                return 1;
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
