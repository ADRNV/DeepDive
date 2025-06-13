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

while (true)
{
    var connection = await listener.AcceptAsync();

    var buffer = new byte[1024];

    var thread = Task.Run(async () =>
    {
        using var file = new FileStream(@"data.txt",
          FileMode.Open, FileAccess.Read, FileShare.Read);
        var buffer = new byte[1024 * 1024];

        while (true)
        {
            int read = await file.ReadAsync(buffer, 0, buffer.Length);

            if (read != 0)
            {
                await connection.SendAsync(new ArraySegment<byte>(buffer, 0, read), SocketFlags.None);
                Console.WriteLine("Server send");
            }
            else
            {
                connection.Shutdown(SocketShutdown.Both);
                connection.Dispose();
                Console.WriteLine("Connection closed");
                return;
            }
        }
    });
}
