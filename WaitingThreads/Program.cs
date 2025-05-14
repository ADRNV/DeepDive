var threads = new Thread[150];

for(int i = 0;i < threads.Length; i++)
{
    threads[i] = new Thread(SomeWork);
    threads[i].Start();
}

foreach(var thread in threads)
{
    var terminated = thread.Join(TimeSpan.FromSeconds(10));

    if (terminated)
    {
        Console.WriteLine($"[{thread.ManagedThreadId}] - Terminated");
    }
    else
    {
        Console.WriteLine($"[{thread.ManagedThreadId}] - Waited");
    }
    //thread.Join();
}

Console.WriteLine("Finish");

void SomeWork(){
    var randomTime = new Random().Next(3000, 15000);
    Thread.Sleep(randomTime);
    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Work at {randomTime}");
}