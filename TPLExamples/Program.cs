using System.Collections.Concurrent;

var results = new ConcurrentBag<int>();
Parallel.For(0, 100, i =>
{
    // Simulate some work
    Task.Delay(100).Wait();
    results.Add(i);
});

Console.WriteLine($"Processed {results.Count} items in parallel.");
Console.Read();