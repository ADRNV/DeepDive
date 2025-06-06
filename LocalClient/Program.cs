using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

int count = int.Parse(args[0]);
Console.WriteLine($"Running with {count} connections");
var tasks = new Task[count];
int failCount = 0;
var faileCountLock = new Lock();

Stopwatch sw = Stopwatch.StartNew();

for (int i = 0; i < count; ++i)
{
    tasks[i] = RunTest(i);
}
Task.WaitAll(tasks);
sw.Stop();

 lock(faileCountLock)
   if (failCount > 0) Console.WriteLine($"{failCount} failures");

Console.WriteLine($"time: {sw.ElapsedMilliseconds}ms");

Task RunTest(int currentTask) 
{
    return Task.Run(async () =>
    {
        var rng = new Random(currentTask);
        await Task.Delay(rng.Next(2 * count));
        using var clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        try
        {
            var hostName = Dns.GetHostName();

            IPHostEntry localhost = Dns.GetHostEntryAsync(hostName).Result;

            var adress = new IPEndPoint(localhost.AddressList[0], 7777);

            await clientSocket.ConnectAsync(adress);

            var buffer = new byte[1024 * 1024];
            while (clientSocket.Connected)
            {
                int read = await clientSocket.ReceiveAsync(
                      buffer, SocketFlags.None);
                if (read == 0) break;
            }
        }
        catch
        {
            lock (faileCountLock)
                ++failCount;
        }
    });
}