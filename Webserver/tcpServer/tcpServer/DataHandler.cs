using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        private static Task<bool> VerifySender(string input)
        {
            Prefernces p = new Prefernces();
            if (p.DeviceIds.Contains(input))
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
        private static async Task ConsumeTasks(CancellationToken cancel)
        {
            foreach (var task in ProduceForever(cancel))
            {
                await task;
            }
        }
        private static IEnumerable<Task> ProduceForever(CancellationToken cancel)
        {
            do
            {
                yield return DBHandler();
            } while (!cancel.IsCancellationRequested);
        }
        private static async Task DBHandler()
        {
            return;
        }
    }
}
