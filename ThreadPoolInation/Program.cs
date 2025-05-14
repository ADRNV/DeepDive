Console.WriteLine("Main thread");
Console.WriteLine(ThreadPool.ThreadCount);
var tasks = new List<Task>();

for(var i = 0;i <= 100; i++)
{
    var copy = i;
    var task = Task.Run(() => BackgroundWork(copy));
    tasks.Add(task);
}
Thread.Sleep(5_000);

Task.WaitAll(tasks);

Console.WriteLine($"End of [{Thread.CurrentThread.Name}] [{Thread.CurrentThread.ManagedThreadId}]");
Console.WriteLine(ThreadPool.ThreadCount);

async Task<int> BackgroundWork(int arg)
{
    var miliseconds = new Random().Next(1_000, 3_000);

    Console.WriteLine($"Passed {arg}: Execution at {miliseconds} on {Thread.CurrentThread.ManagedThreadId}[{Thread.CurrentThread.Name}]");
    
    await Task.Delay(miliseconds);

    return await Task.FromResult(0);
}