using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlTypes;

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

        public async void Handler(string data)
        {
            await Task.Run(() =>
            {
                DataQueue.Enqueue(data);
            });
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
