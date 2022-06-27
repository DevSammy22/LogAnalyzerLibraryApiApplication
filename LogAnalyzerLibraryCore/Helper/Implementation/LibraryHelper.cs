using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzerLibraryCore.Helper
{
    public class LibraryHelper : ILibraryHelper
    {
        public async Task<List<string>> DataReaderAsync(string path)
        {
            if (File.Exists(path))
            {
                using StreamReader streams = new StreamReader(path);
                List<string> myLog = new List<string>();
                string temp = await streams.ReadToEndAsync();
                while (temp != null)
                {
                    myLog.Add(temp);
                }

                List<string> logs = new List<string>();

                for (int i = 0; i < myLog.Count; i++)
                {
                    string log;
                    if (myLog[i].Length < 30) continue;

                    if (int.TryParse($"{myLog[i][0]}", out int z))
                    {
                        log = myLog[i].Substring(25).ToLower();
                    }
                    else
                    {
                        log = myLog[i].Substring(32).ToLower();
                    }

                    logs.Add(log);
                }

                return logs;
            }
            else
            {
                return new List<string>();
            }
        }

        public async Task<Dictionary<string, int>> GroupLogsAsync(string path)
        {
            var logs = await DataReaderAsync(path);
            var myDict = new Dictionary<string, int>();
            foreach (var log in logs)
            {
                if (myDict.ContainsKey(log))
                {
                    myDict[log]++;
                }
                else
                {
                    myDict[log] = 1;
                }
            }
            return myDict;
        }
    }
}
