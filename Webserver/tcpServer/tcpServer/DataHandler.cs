using System;
using System.Collections.Generic;
using System.Text;

namespace tcpServer
{
    public class DataHandler
    {
        public Queue<string> CleanData { get; set; }
        public void Data(string requestData)
        {
            CleanData.Enqueue(requestData);
        }
        private void DataClean(string rawData)
        {
            string[] split = rawData.Split("\n");
            foreach (string s in split)
            {
                string raw = s.Substring(s.IndexOf(':') + 1);

            }
        }

        private bool VerifySender()
        {
            return false;
        }
        private void DBHandler()
        {

        }
    }
}
