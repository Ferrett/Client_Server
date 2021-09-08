using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    class Program
    {
        static int port = 8000;
        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(10);

                Console.WriteLine("Start server...");

                while (true)
                {
                    Socket socketClient = socket.Accept();
                    StringBuilder stringBuilder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = socketClient.Receive(data);
                        stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (socketClient.Available > 0);

                    int[] nums = { 0, 0 };

                    nums[0] = int.Parse(stringBuilder.ToString().Split(',')[0].Split('[')[1]);
                    nums[1] = int.Parse(stringBuilder.ToString().Split(',')[1].Split(']')[0]);

                    Console.WriteLine($"Result: {nums[0]}+{nums[1]}={nums[0]+nums[1]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
