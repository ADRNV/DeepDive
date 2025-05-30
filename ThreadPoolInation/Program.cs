using ThreadPoolInation;

var example = new NetworkQueringWithTasks();
var threads = Enumerable.Range(0, 3).Select(i => example.RunFromThread());

var locker = new Lock();

foreach(var thread in threads)
{
    thread.Start();
}

lock (locker)
{
    Console.WriteLine("Finish");
    foreach (var item in example.Cache)
    {
        Console.WriteLine($"{item.Key}:{item.Value}");
    }
}

lock (locker) 
{ 
    Console.WriteLine("Hitory");
    foreach (var item in example.History)
    {
        Console.WriteLine($"{item.Key}:{item.Value?.Length}");
    }
}

while (true) 
{
};

