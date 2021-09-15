using System;
using System.Collections.Generic;
using System.IO;
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
                string turn = String.Empty;
                bool turnP = true;
                StringBuilder stringBuilder = new StringBuilder();
                string[] nums;

                while (true)
                {

                    if(turnP==true)
                    {
                        Console.WriteLine("Your turn!");
                        turn  = Console.ReadLine();
                        socket.Send(Encoding.Unicode.GetBytes(turn));
                        turnP = false;


                    }
                    else
                    {
                        Console.WriteLine("Wait for other player to make turn!");
                        do
                        {
                            bytes = socket.Receive(data);
                            stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        } while (socket.Available > 0);

                        Console.Clear();

                        nums = stringBuilder.ToString().Split('.');

                        for (int i = 0; i < 9; i++)
                        {
                            Console.Write(nums[i]);
                            if (i % 3 == 0&&i!=0)
                                Console.WriteLine();
                        }

                        turnP = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
