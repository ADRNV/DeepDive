using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace StreamsAndAsync
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class StreamsAndAsyncExample
    {
        public async Task WriteSampleAsync(string text,string path = null)
        {
           if(path == null)
           {
             path = Environment.CurrentDirectory + "txt.txt";
           }

            if (File.Exists(path)) File.Delete(path);

            using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1);

            var textBytes = Encoding.UTF8.GetBytes(text);

            await fs.WriteAsync(textBytes, 0, textBytes.Length).ConfigureAwait(false);
        }

        public static string GenerateContent(int length = 10)
        {
           return Enumerable.Range(1, length).AsParallel()
               .Select(i => Guid.NewGuid().ToString())
               .Aggregate((a, b) => $"{a}\n{b}");
        }

        [Benchmark]
        public void Process10FilesThread()
        {
            var tasks = new Task[10];
            for (int i = 0; i < 10; ++i)
            {
                var icopy = i;
                tasks[i] = Task.Run(() =>
                {
                    File.ReadAllBytes(@$"C:\\tmp\{icopy}.txt");
                    //Console.WriteLine("Doing something with the file's content");
                });
            }
            Task.WaitAll(tasks);
        }

        [Benchmark]
        public Task Process10FilesAsync()
        {
            var tasks = new Task[10];
            for (int i = 0; i < 10; ++i)
            {
                var icopy = i;
                tasks[i] = Task.Run(async () =>
                {
                    await File.ReadAllBytesAsync(@$"C:\\tmp\{icopy}.txt");
                    //Console.WriteLine("Doing something with the file's content");
                });
            }
            return Task.WhenAll(tasks);
        }


    }
}
