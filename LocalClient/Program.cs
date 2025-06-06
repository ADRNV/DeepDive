using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;


var hostName = Dns.GetHostName();

IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

var adress = new IPEndPoint(localhost.AddressList[0], 11_000);

using var sender = new Socket(adress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

Thread.Sleep(2_000);

await sender.ConnectAsync(adress);

var count = 0;

var st = new Stopwatch();

st.Start();

while (sender.Connected)
{
    var message = $"Form client {count}";

    message += "<|EOM|>";
    
    Console.WriteLine("Client: "+ message);

    var encoded = Encoding.UTF8.GetBytes(message);

    _ = await sender.SendAsync(encoded, SocketFlags.None);

    var buffer = new byte[1_024];
    var received = await sender.ReceiveAsync(buffer, SocketFlags.None);
    var response = Encoding.UTF8.GetString(buffer, 0, received);
    if (response == "<|RD|>")
    {
        Console.WriteLine(
            $"Socket client received acknowledgment: \"{response}\"");
       // break;
    }

    if (count >= 10_000) break;
    
    count++;
}

st.Stop();

Console.WriteLine("Sended from " + st.ElapsedMilliseconds);
