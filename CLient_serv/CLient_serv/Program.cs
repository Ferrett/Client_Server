using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CLient_serv
{
    class Program
    {
       

        static string ipAddr = "127.0.0.1";
        static int port = 8000;
        static void Main(string[] args)
        {
           
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(iPEndPoint);

                int bytes = 0;
                byte[] data = new byte[256];
                StringBuilder stringBuilder = new StringBuilder();

                Console.WriteLine("Enter string:");
                string sms = Console.ReadLine();

                data = Encoding.Unicode.GetBytes(sms);


               


                socket.Send(data);

                Console.WriteLine($"String send success!");


                do
                {
                    bytes = socket.Receive(data);
                    stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);
                Console.WriteLine(stringBuilder.ToString());
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
