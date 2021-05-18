using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace tcpServer
{
    public class DataHandler
    {
        public Queue<string> DataQueue { get; set; }

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

        public byte Handler(string data)
        {
            string lhs = DataClean(data, 1);
            string rhs = DataClean(data, 2);

            if (VerifySender(lhs) != true)
                return 0;
            else
            {

            }
        }

        private void ConnectionOpen(string procedureName)
        {
            Prefernces p = new Prefernces();
            try
            {
                var myStringList = new List<string>();
                using (SqlConnection sqlConnection1 = new SqlConnection(p.ConnectionString))
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = sqlConnection1;
                    sqlConnection1.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Data is accessible through the DataReader object here.
                        reader.Read();

                        var myObject = new object[100];

                        int colCount = reader.GetValues(myObject);
                        for (int i = 0; i < colCount; i++)
                        {
                            myStringList.Add((string)myObject[i]);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Sql Connection Error");
            };
            
        }

        //Clean data => verify sender => get user type (student vs teacher) => 
        public Exception ReturnVerifyError()
        {
            return null;
        }

        private static bool VerifySender(string input)
        {
            Prefernces p = new Prefernces();
            if (p.DeviceIds.Contains(input))
                return true;
            else
                return false;
        }

        private static async Task DBHandler()
        {
            return;
        }
    }
}
