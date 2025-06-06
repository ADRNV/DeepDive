using System.Net;
using System.Net.Sockets;
using System.Text;

var hostName = Dns.GetHostName();

IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

var adress = new IPEndPoint(localhost.AddressList[0], 11_000);

using var listener = new Socket(adress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

listener.Bind(adress);
listener.Listen(10);

Console.WriteLine("Server start");

var handler = await listener.AcceptAsync();

while (true)
{
    var buffer = new byte[1024];

    var recived = await handler.ReceiveAsync(buffer, SocketFlags.None);

    var response = Encoding.UTF8.GetString(buffer, 0, recived);

    var eom = "<|EOM|>";
    if (response.IndexOf(eom) > -1 /* is end of message */)
    {
        Console.WriteLine(
            $"Socket server received message: \"{response.Replace(eom, "")}\"");

        var ackMessage = "<|RD|>";

        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
        await handler.SendAsync(echoBytes, 0);
        Console.WriteLine(
            $"Socket server sent acknowledgment: \"{ackMessage}\"");

        //break;
    }

}
