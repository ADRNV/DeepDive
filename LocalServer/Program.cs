using System.Net;
using System.Net.Sockets;
using System.Text;

var hostName = Dns.GetHostName();

IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

var adress = new IPEndPoint(localhost.AddressList[0], 7777);

using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);

listener.Bind(adress);
listener.Listen(50);

Console.WriteLine("Server start");

var handler = await listener.AcceptAsync();

while (true)
{
    var buffer = new byte[1024];

    var connection = listener.Accept();

    var thread = new Thread(() =>
    {
        using var file = new FileStream(@"data.txt",
          FileMode.Open, FileAccess.Read, FileShare.Read);
        var buffer = new byte[1024 * 1024];

        while (true)
        {
            int read = file.Read(buffer, 0, buffer.Length);

            if (read != 0)
            {
                connection.Send(new ArraySegment<byte>(buffer, 0, read), SocketFlags.None);
            }
            else
            {
                Console.WriteLine("Server stopped");
                connection.Shutdown(SocketShutdown.Both);
                connection.Dispose();
                return;
            }
        }
    });

    thread.Start();
}
