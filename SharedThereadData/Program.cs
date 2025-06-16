using System.Diagnostics;

int inc = -1_000;
Lock incLock = new Lock();

int dec = 1_000;
Lock decLock = new Lock();

void Inc()
{
    while (inc != 1_000)
    {
        lock (decLock)
        {
            lock (incLock)
            {
                inc++;
            }
        }
    }   
}

void Dec()
{
    while (dec != 0) 
    {
        lock(incLock){

            lock (decLock)
            {
                dec--;
            }
        }
    }
}

var incThread = new Thread(Inc);
var decThread = new Thread(Dec);

incThread.Start();
decThread.Start();

while (true)
{
    Console.WriteLine($"Dec {dec}\nInc {inc}");
    Console.Clear();
}

