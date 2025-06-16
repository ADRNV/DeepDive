using LocalClient.Models;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace LocalClient
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class BenchhmarkRunner(string[] args)
    {

        private int _count = 100;

        private readonly Task[] _tasks = new Task[int.Parse(args[0])];

        private int _failCount = 0;

        private Lock _faileCountLock = new Lock();

        [Benchmark]
        public RunReport MainThreadBasedVersion()
        {
            var hostName = Dns.GetHostName();

            #region Wait server start
            IPHostEntry localhost = Dns.GetHostEntry(hostName);

            var adress = new IPEndPoint(localhost.AddressList[0], 7777);

            using var clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            while (!clientSocket.Connected)
            {
                clientSocket.Connect(adress);
            }
            #endregion

            int count = int.Parse(args[0]);
            var tasks = new Task[count];
            int failCount = 0;
            var faileCountLock = new Lock();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < count; ++i)
            {
                tasks[i] = RunTest(i);//Run on background
            }
            Task.WaitAll(tasks);
            sw.Stop();

            return new RunReport(count, failCount, sw.ElapsedMilliseconds);
        }

        [Benchmark]
        public RunReport MainTaskBasedVersion()
        {
            var hostName = Dns.GetHostName();

            #region Wait server start
            IPHostEntry localhost = Dns.GetHostEntry(hostName);

            var adress = new IPEndPoint(localhost.AddressList[0], 7777);

            using var clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            while (!clientSocket.Connected)
            {
                clientSocket.Connect(adress);
            }
            #endregion

            int count = int.Parse(args[0]);
            var tasks = new Task[count];
            int failCount = 0;
            var faileCountLock = new Lock();

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < count; ++i)
            {
                tasks[i] = RunTest(i);//Run on background
            }
            Task.WaitAll(tasks);
            sw.Stop();

            return new RunReport(count, failCount, sw.ElapsedMilliseconds);
        }


        Task RunTest(int currentTask)
        {
            return Task.Run(async () =>
            {
                var rng = new Random(currentTask);
                await Task.Delay(rng.Next(2 * _count));
                using var clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    var hostName = Dns.GetHostName();

                    IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

                    var adress = new IPEndPoint(localhost.AddressList[0], 7777);

                    await clientSocket.ConnectAsync(adress);

                    var buffer = new byte[1024 * 1024];

                    while (clientSocket.Connected && clientSocket.Available != 0)
                    {
                        int read = await clientSocket.ReceiveAsync(
                              buffer, SocketFlags.None);
                        if (read == 0) break;
                    }
                }
                catch
                {
                    lock (_faileCountLock)
                        ++_failCount;
                }
            });
        }
    }
}
