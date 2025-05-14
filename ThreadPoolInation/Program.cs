Console.WriteLine("Main thread");
for(var i = 0;i < 10000; i++)
{
    ThreadPool.QueueUserWorkItem(BackgroundWork);
}
Thread.Sleep(1000);
Console.WriteLine("End of main");

void BackgroundWork(object parameter)
{
    Console.WriteLine("Background thread");
}