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

        private Lock _locker = new(); 

        public NetworkQueringWithTasks()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://icanhazdadjoke.com");
        }

        public async Task<string> GetJoke(string query)
        {
            lock (Cache)
            {
                if (Cache.TryGetValue(query, out var value))
                {
                    return value;
                }
            }

            var result = (await _httpClient.GetAsync($"j/{query}"));
            
            await Task.Delay(1000);
            
            lock(Cache)
            {
                Cache[query] = result.StatusCode.ToString();
                History.Add(new KeyValuePair<string, string>(query, result.ToString()));
            }

            return result.ToString();
        }

        public Thread RunFromThread()
        {
            var thread = new Thread(async () =>
            {
                var queries = new List<string>() { "R7UfaahVfFd", "PZDAXL6pOCd", "MRZ0LJtHQCd", "usrcaMuszd", "R7UfaahVfFd", "usrcaMuszd" };

                foreach (var query in queries)
                {
                    var result = await GetJoke(query);
                }

            });
            thread.Name = "Network fetch";

            return thread;
        }

        public async Task<string> GetJokeNoLock(string query)
        {

            if (Cache.TryGetValue(query, out var value))
            {
                return value;
            }
            
            var result = (await _httpClient.GetAsync($"j/{query}"));

            await Task.Delay(1000);

            Cache[query] = result.StatusCode.ToString();
            History.Add(new KeyValuePair<string, string>(query, result.ToString()));
            
            return result.ToString();
        }

        public Thread RunFromThreadNoLock()
        {
            var thread = new Thread(async () =>
            {
                var queries = new List<string>() { "R7UfaahVfFd", "PZDAXL6pOCd", "MRZ0LJtHQCd", "usrcaMuszd", "R7UfaahVfFd", "usrcaMuszd" };

                foreach (var query in queries)
                {
                    var result = await GetJoke(query);
                }

            });
            thread.Name = "Network fetch";

            return thread;
        }

        [Benchmark]
        public void Run() => Enumerable.Range(0, 3).Select(i => this.RunFromThread());

        [Benchmark]
        public void RunNoLock() => Enumerable.Range(0, 3).Select(i => this.RunFromThreadNoLock());

    }
}
