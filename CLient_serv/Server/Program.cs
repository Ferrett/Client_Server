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
        static void DrawField()
        {
            Console.WriteLine($"{TicTacToe.Field[0, 0]}|{TicTacToe.Field[0, 1]}|{TicTacToe.Field[0, 2]}");
            Console.WriteLine($"—————");
            Console.WriteLine($"{TicTacToe.Field[1, 0]}|{TicTacToe.Field[1, 1]}|{TicTacToe.Field[1, 2]}");
            Console.WriteLine($"—————");
            Console.WriteLine($"{TicTacToe.Field[2, 0]}|{TicTacToe.Field[2, 1]}|{TicTacToe.Field[2, 2]}\n");
        }

        static int port = 8000;


        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            TicTacToe.Init();

            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(10);
                Console.WriteLine("\tWaiting for second player\n");


                while (true)
                {
                    Socket socketClient = socket.Accept();
                    StringBuilder stringBuilder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    string turnString = String.Empty;

                    while (true)
                    {
                        if (TicTacToe.Player2Turn == false)
                        {
                            Console.WriteLine("Wait for other player to make turn!");
                            do
                            {
                                bytes = socketClient.Receive(data);
                                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                            } while (socketClient.Available > 0);

      
                            TicTacToe.Field[int.Parse(stringBuilder.ToString().Split(',')[0])-1, int.Parse(stringBuilder.ToString().Split(',')[1])-1]= TicTacToe.Player1Symb;
                            stringBuilder = new StringBuilder();

                            if (TicTacToe.CheckWin())
                                break;
                            Console.Clear();
                            TicTacToe.Player2Turn = true; 
                        }
                        else
                        {
                            while (true)
                            {
                                DrawField();

                                Console.WriteLine("Your turn! Enter cords like this: 1,1");
                                turnString = Console.ReadLine();
                                if (TicTacToe.Field[int.Parse(turnString.Split(',')[0]) - 1, int.Parse(turnString.Split(',')[1]) - 1] == ' ')
                                    break;
                                Console.Clear();
                            }
                            TicTacToe.Field[int.Parse(turnString.Split(',')[0]) - 1, int.Parse(turnString.Split(',')[1]) - 1] = TicTacToe.Player2Symb;
                            if (TicTacToe.CheckWin())
                                break;
                            socketClient.Send(Encoding.Unicode.GetBytes(TicTacToe.FieldToString()));
                            Console.Clear();
                            TicTacToe.Player2Turn = false;
                        }
                    }
                    Console.Clear();
                    DrawField();
                    if(TicTacToe.Player2Turn==false)
                        Console.WriteLine("GG! Player 1 Won!");
                    else
                        Console.WriteLine("GG! Player 2 Won!");

                    socketClient.Send(Encoding.Unicode.GetBytes("GG"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
