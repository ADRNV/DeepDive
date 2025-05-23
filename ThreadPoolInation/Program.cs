using ThreadPoolInation;

var example = new NetworkQueringWithTasks();
var threads = Enumerable.Range(0, 35).Select(i => example.RunFromThread());

foreach(var thread in threads)
{
    thread.Start();
}

Thread.Sleep(10_000);
Console.WriteLine("Finish");
foreach (var item in example.Cache)
{
    Console.WriteLine($"{item.Key}:{item.Value.Length}");
}
Console.WriteLine("Hitory");
foreach (var item in example.History)
{
    Console.WriteLine($"{item.Key}:{item.Value?.Length}");
}

