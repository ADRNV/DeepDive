
using var customCtx = new SingleThreadSynchronizationContext("customCtx");

//Thread.Sleep(1_000);

var work = Task.Run(async () =>
{
    Console.WriteLine($"Task on thread {Environment.CurrentManagedThreadId}\r\n");
    Console.WriteLine($"Task on thread {Environment.CurrentManagedThreadId}\r\n");
    return Task.FromResult(42);
});

foreach (var i in Enumerable.Range(1, 1_000)) 
{
    work.ContinueWith((t) =>
    {
        // Task in Single thread
        customCtx.Post(_ =>
        {
            Console.WriteLine($"Task {i} on thread {Environment.CurrentManagedThreadId}\r\n");
        }, null);
    });

    //await work.ContinueWith((t, s) => Console.WriteLine($"Task {i} on thread {Environment.CurrentManagedThreadId}\r\n"), TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);
}

Console.ReadLine();
