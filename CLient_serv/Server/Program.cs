using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    class Program
    {
        

        static int port = 8000;

        public static byte[] StringAnalitics(StringBuilder stringBuilder, Dictionary<string, int> wordsCount, string[] words, string returnString)
        {
            // Get array of words
            words = stringBuilder.ToString().Split(' ');

            // Count amount of each word
            foreach (var item in words)
            {
                if (wordsCount.ContainsKey(item)==false)
                    wordsCount.Add(item, 1);
                else
                    wordsCount[item]++;
            }

            // Sort dictionary
            wordsCount = wordsCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            
            //  Get the statistics string
            for (int i = 0; i < wordsCount.Count; i++)
            {
                returnString += wordsCount.ElementAt(i).Key + ": " + wordsCount.ElementAt(i).Value + "\n";
            }

            return Encoding.Unicode.GetBytes(returnString);
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


                    data = StringAnalitics(stringBuilder,  wordsCount,  words,  returnString);
                    
                    socketClient.Send(data);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
