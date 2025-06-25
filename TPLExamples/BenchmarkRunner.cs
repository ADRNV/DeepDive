using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Threading.Tasks;

namespace TPLExamples
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class BenchmarkRunner
    {
        private int _count = 1_000;

        [Benchmark]
        public void RunWorkInThread()
        {
            var threads = new Thread[_count];

            for(int i = 0;i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Work();
                });

                threads[i].Start();
            }

            foreach(var thread in threads)
            {
                thread.Join();
            }
        }

        [Benchmark]
        public void RunWorkInTask()
        {
            var tasks = new Task[_count];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                     {
                        Work();
                     });
            }

            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void RunWorkInParalel()
        {
            var tasks = Enumerable.Range(0, _count).ToArray();

            Parallel.ForEach(tasks, (i) =>
            {
                Work();
            });
        }

        [Benchmark]
        public void RunWorkInAsyncParalel()
        {
            var tasks = Enumerable.Range(0, _count).ToArray();

            var task = Parallel.ForEachAsync(tasks, async (i, c) =>
            {
                await WorkAsync();
            });

            Task.WaitAll(task);
        }

        [Benchmark]
        public void RunWorkInTaskAsync()
        {
            var tasks = new Task[_count];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    await WorkAsync();
                });
            }

            Task.WaitAll(tasks);
        }

        private bool Work()
        {
            Thread.Sleep(1_000);
            return true;
        }

        private async Task<bool> WorkAsync()
        {
           await  Task.Delay(1_000);

           return true;
        }
    }
}
