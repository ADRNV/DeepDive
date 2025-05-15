var locker = new Lock();

void GetIncorrectValue()
{
    int theValue = 0;
    var threads = new Thread[100];

    for (int i = 0; i < threads.Length; ++i)
    {
        threads[i] = new Thread(() =>
        {
            for (int j = 0; j < 100_000; ++j)
            {
                lock (locker)
                {
                  ++theValue;
                }
            }           
        });
        threads[i].Start();
    }
    foreach (var current in threads)
    {
        current.Join();
    }
    Console.WriteLine(theValue);
}

GetIncorrectValue();