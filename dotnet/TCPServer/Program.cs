using System.Net;
using System.Net.Sockets;

public class Server
{
    TcpListener listener { get; set; }

    public Server()
    {
        listener = new(IPAddress.Any, 10000);
    }

    public void Start()
    {
        try
        {
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread t = new(new ParameterizedThreadStart(Handle!));
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException {e}");
        }
        finally
        {
            listener.Stop();
        }
    }

    private void Handle(object obj)
    {
        Byte[] bytes;
        var client = (TcpClient)obj;
        var ip = client.Client.RemoteEndPoint;
        Console.WriteLine($"connection from {ip}");

        var stream = client.GetStream();

        int i;

        while (true)
        {
            bytes = new Byte[1024];
            if ((i = stream.Read(bytes, 0, 1024)) > 0)
            {
                stream.Write(bytes, 0, i);
            }
            else
            {
                break;
            }
        }

        client.Dispose();
        Console.WriteLine($"eof {ip}");
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Server s = new();
        s.Start();
    }
}
