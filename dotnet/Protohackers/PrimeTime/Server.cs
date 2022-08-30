using System.Text;
using System.Text.Json;
using System.Net;
using System.Net.Sockets;

namespace PrimeTime.Server;

public struct RequestMessage
{
    public string method { get; set; }
    public int number { get; set; }

    public bool IsValid()
    {
        return method.Equals("isPrime");
    }
}

public struct ResponseMessage
{
    public string method { get; init; }
    public bool prime { get; init; }

    public ResponseMessage(int n)
    {
        method = "isPrime";
        prime = false;
        prime = this.IsPrime(n);
    }

    private bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        var bound = (int)Math.Floor(Math.Sqrt(n));

        for (int i = 3; i <= bound; i += 2)
        {
            if (n % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}

public class Server
{

    TcpListener listener { get; set; }

    public Server(IPAddress ip, int port)
    {
        listener = new(ip, port);
    }

    public void Start()
    {
        try
        {
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread t = new(new ParameterizedThreadStart(Handle));
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
                string data = Encoding.ASCII.GetString(bytes, 0, i).Trim();
                string[] requests = data.Split(System.Environment.NewLine);

                List<string> response = new();
                string r = string.Empty;
                foreach (var s in requests)
                {
                    r = ParseMessage(s);

                    if (r.Equals(s))
                    {
                        Console.WriteLine($"BAD => {s} {r}");
                        stream.Write(bytes, 0, bytes.Length);
                        break;
                    }
                    response.Add(r);
                }

                if (response.Count.Equals(0))
                {
                    break;
                }


                string outData = string.Join("\n", response.ToArray()) + "\n";
                Console.WriteLine($"IN => {data}");
                Console.WriteLine($"OUT => {outData}");
                Byte[] send = Encoding.ASCII.GetBytes(outData, 0, outData.Length);

                stream.Write(send, 0, send.Length);
            }
            else
            {
                break;
            }
        }

        client.Dispose();
        Console.WriteLine($"eof {ip}");
    }

    private string ParseMessage(string msg)
    {
        try
        {
            var request = JsonSerializer.Deserialize<RequestMessage>(msg);

            if (!request.IsValid())
                return msg;

            var response = new ResponseMessage(request.number);
            string r = JsonSerializer.Serialize(response);
            return r;
        }
        catch (JsonException)
        {
            return msg;
        }
    }
}
