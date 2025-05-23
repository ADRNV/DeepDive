using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace ThreadPoolInation
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class NetworkQueringWithTasks
    {
        private HttpClient _httpClient;

        public Dictionary<string, string> Cache = new Dictionary<string, string>();

        public List<KeyValuePair<string, string>> History = new();

        public NetworkQueringWithTasks()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://icanhazdadjoke.com");
        }

        public async Task<string> GetJoke(string query)
        {
            if(Cache.TryGetValue(query, out var value))
            {
                History.Add(new KeyValuePair<string, string>(query, value));
                return value;
            }

            var result = (await _httpClient.GetAsync($"j/{query}"));

            Cache[query] = result.ToString();

            History.Add(new KeyValuePair<string, string>(query, result.ToString()));
            
            return result.ToString();
        }

        public Thread RunFromThread()
        {
            var thread = new Thread(async () =>
            {
                var result = await GetJoke(new Random().Next(1, 5).ToString());
              
                Out(result);

            });
            thread.Name = "Network fetch";

            return thread;
        }

        private void Out(string toOut)
        {
            Console.WriteLine(toOut);
        }
    }
}
