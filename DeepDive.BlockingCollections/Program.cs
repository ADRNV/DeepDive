using System.Collections.Concurrent;
using System.Runtime.InteropServices;

var resources = new BlockingCollection<int>(10);

var consume = Task.Run(async () =>
{
    int i = -1;

    while (!resources.IsCompleted)
    {
        try
        {
            await Task.Delay(3_000);
            i = resources.Take();
            Console.WriteLine($"Take {i}");

        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Adding completed");
        }
        Console.WriteLine();
    }
});

bool moreItemsToAdd = true;
Lock locker = new Lock();


var cancel = new CancellationTokenSource();
cancel.CancelAfter(TimeSpan.FromSeconds(8));
cancel.Token.Register(() =>
{
    moreItemsToAdd = false;
});

var produce = Task.Run(async () =>
{
    while (moreItemsToAdd)
    {
        int data = new Random().Next();

        Console.WriteLine($"Produce {data}");

        resources.Add(data);
    } 
    // Let consumer know we are done.
    resources.CompleteAdding();
}, cancel.Token);

Task.WaitAll(consume, produce);
