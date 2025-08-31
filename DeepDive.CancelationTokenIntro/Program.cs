using System.Net;
//Something old API doesnt support work Cancelation token
//In this case need 'pair' events from old api and cancelation CancelationToken.Register(callback)  
using var cts = new CancellationTokenSource();

var token = cts.Token;

using var client = new WebClient();

client.DownloadStringCompleted += Client_DownloadStringCompleted;

using var clientDisposeCallback =
    token.Register(() =>
    {
        client.CancelAsync();
        client.Dispose();
    });


var content = "";

while (true)
{
    try
    {
        token.ThrowIfCancellationRequested();
    }
    catch (OperationCanceledException oce)
    {
        Console.WriteLine(oce.Message);
    }
    
    var key = Console.ReadKey(intercept: true).Key;

    switch (key)
    {
        case ConsoleKey.Enter:
           
            Console.WriteLine("Start download");

            //On background. For example not pass cancelation token
            Task.Run(async () =>
            {
                content = await client.DownloadStringTaskAsync(new Uri("https://openweathermap.org/api/one-call-3#current"));
            }).ConfigureAwait(true);
        break;
        case ConsoleKey.C:
            await cts.CancelAsync();
        break;
    }
}

void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
{
    if (e.Cancelled)
    {
        Console.WriteLine("Download canceled");
    }
    else if (e.Error != null)
    {
        Console.WriteLine(e.Result);
        Console.WriteLine($"Error: {e.Error.Message}");
    }
    else
    {
        Console.WriteLine("Complete");
        Console.WriteLine(e.Result[..200]);
    }
}
