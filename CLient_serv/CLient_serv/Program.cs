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
        static void DrawField(string[,] nums)
        {
            Console.WriteLine($"{nums[0, 0]}|{nums[0, 1]}|{nums[0, 2]}");
            Console.WriteLine($"—————");
            Console.WriteLine($"{nums[1, 0]}|{nums[1, 1]}|{nums[1, 2]}");
            Console.WriteLine($"—————");
            Console.WriteLine($"{nums[2, 0]}|{nums[2, 1]}|{nums[2, 2]}\n");
        }

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(iPEndPoint);
                int bytes = 0;
                byte[] data = new byte[256];
                string turnString = String.Empty;
                bool YourTurn = true;
                StringBuilder stringBuilder = new StringBuilder();
                string[,] field = new string[3,3]{ { " ", " ", " " }, { " ", " ", " " },{ " ", " ", " " } };
     
                while (true)
                {
                    if(YourTurn==true)
                    {
                        while (true)
                        {
                            DrawField(field);

                            Console.WriteLine("Your turn! Enter cords like this: 1,1");
                            turnString = Console.ReadLine();
                            if (field[int.Parse(turnString.Split(',')[0])-1, int.Parse(turnString.Split(',')[1])-1] == " ")
                                break;
                            Console.Clear();
                        }
                        socket.Send(Encoding.Unicode.GetBytes(turnString));
                        Console.Clear();
                        YourTurn = false;
                    }
                    else
                    {
                        Console.WriteLine("Wait for other player to make turn!");
                        do
                        {
                            bytes = socket.Receive(data);
                            stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        } while (socket.Available > 0);

                        if (stringBuilder.ToString() == "GG")
                            break;

                        field[0, 0] = stringBuilder.ToString().Split('.')[0];
                        field[0, 1] = stringBuilder.ToString().Split('.')[1];
                        field[0, 2] = stringBuilder.ToString().Split('.')[2];
                        field[1, 0] = stringBuilder.ToString().Split('.')[3];
                        field[1, 1] = stringBuilder.ToString().Split('.')[4];
                        field[1, 2] = stringBuilder.ToString().Split('.')[5];
                        field[2, 0] = stringBuilder.ToString().Split('.')[6];
                        field[2, 1] = stringBuilder.ToString().Split('.')[7];
                        field[2, 2] = stringBuilder.ToString().Split('.')[8];


                        stringBuilder = new StringBuilder();
                        Console.Clear();
                        YourTurn = true;
                    }
                }
                Console.Clear();
                DrawField(field);
                if (YourTurn == false)
                    Console.WriteLine("GG! Player 1 Won!");
                else
                    Console.WriteLine("GG! Player 2 Won!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
