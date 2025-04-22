using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace EnumerableInterfaces
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class EnumerableFileReading
    {
        public string Serch = "bb2db5c9-df3b-465d-8bc8-1b83c9617806";

        [Benchmark]
        public void FileAsEnumerable()
        {
            var sre = new StreamReaderAsyncEnumerable(@"C:\tmp\Test.txt");
            var found = false;

            foreach (var item in sre)
            {
                found = item.Contains(Serch);

                if (found)
                {
                    return;
                }
            }
        }

        [Benchmark]
        public async Task FileAsAsyncEnumerable()
        {
            var sre = new StreamReaderAsyncEnumerable(@"C:\tmp\Test.txt");
            var found = false;

            await foreach(var item in sre.WithCancellation(new CancellationToken()))
            {
                found = item.Contains(Serch);

                if (found)
                {
                    return;
                }
            }
        }


        [Benchmark(Baseline = true)]
        public void FileAsNonEnumerable()
        {
            StreamReader sr;

            sr = File.OpenText(@"c:\tmp\Test.txt");
            
            List<string> fileContents = new List<string>();
            
            while (!sr.EndOfStream)
            {
                fileContents.Add(sr.ReadLine());
            }

            // Check for the string.
            var stringsFound =
                from line in fileContents
                where line.Contains(Serch)
                select line;

            if (stringsFound.Count() > 0)
            {
                return;
            }
        }
    }
}
