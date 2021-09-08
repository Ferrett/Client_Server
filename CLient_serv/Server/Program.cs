using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;


namespace Server
{
    class Program
    {
        

        static int port = 8000;

        public static byte[] Test(StringBuilder stringBuilder, Dictionary<string, int> wordsCount, string[] words, string returnString, byte[] data)
        {
            words = stringBuilder.ToString().Split(' ');

            foreach (var item in words)
            {
                if (wordsCount.ContainsKey(item)==false)
                    wordsCount.Add(item, 1);
                else
                    wordsCount[item]++;
            }
            wordsCount = wordsCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            

            for (int i = 0; i < wordsCount.Count; i++)
            {
                returnString += wordsCount.ElementAt(i).Key + ": " + wordsCount.ElementAt(i).Value + "\n";
            }

            data = Encoding.Unicode.GetBytes(returnString);

            return data;
        }

        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            string[] words = null;
            string returnString = string.Empty;

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


                    data = Test(stringBuilder,  wordsCount,  words,  returnString, data);
                    
                    socketClient.Send(data);
                    
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
