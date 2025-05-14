using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Immutable;
using System.Text;

namespace TaskVsValueTask
{

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetAsValueTask
    {

        public static Dictionary<Guid, string> Storage { get; set; } = new Dictionary<Guid, string>();

        public static List<string> ToSearch = new List<string>();

        private static string _path = $"{Environment.CurrentDirectory}valueTask.txt";

        public GetAsValueTask()
        {
            var guids = Utils.GetGuids(300_000);

            var text = guids.Select(g => g.ToString()).Aggregate((a, b) => $"{a}\n{b}");

            Utils.WriteSampleAsync(text, _path);

            foreach(var i in guids)
            {
                if(Storage.Count <= 500 && !Storage.ContainsKey(i))
                {
                    var rnd = new Random();
             
                    Storage.TryAdd(guids.ToArray()[rnd.Next(0, guids.Count() - 1)], i.ToString());
                }
                if (ToSearch.Count < 500)
                {
                    var rnd = new Random();

                    ToSearch.Add(guids.ToArray()[rnd.Next(0, guids.Count() - 1)].ToString());
                }
                if (Storage.Count >= 500)
                {
                    break;
                }
            }
        }

        public static async Task<bool> GetValue(string searchValue)
        {
            using var file = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            
            byte[] buffer = new byte[0x100];
            
            int numRead;

            string result = null;

            while ((numRead = await file.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.UTF8.GetString(buffer, 0, numRead);

                if (searchValue == text)
                {
                    result = text;
                    break;
                }
            }

            return !string.IsNullOrWhiteSpace(result); 
        }

        [Benchmark]
        public async Task<bool> Main()
        {
            Task<bool> result = null;

            foreach(var i in ToSearch)
            {
                if(Storage.TryGetValue(Guid.Parse(i), out var value))
                {
                    result = Task.FromResult(true);
                }
                else
                {
                    result =  GetValue(i);
                }  
            }

            return await result;
        }

        [Benchmark]
        public async ValueTask<bool> MainValueTask()
        {
            Task<bool> result = Task.FromResult(true);
            bool valueResult = false;

            foreach (var i in ToSearch)
            {
                if (Storage.TryGetValue(Guid.Parse(i), out var value))
                {
                    valueResult = true;
                }
                else
                {
                    result = GetValue(i);
                }
            }

            return valueResult ? valueResult : await result;
        }
    }
}
