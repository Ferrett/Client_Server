using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ClientServer;

namespace Clients
{
    class Program
    {
        static void Main(string[] args)
        {

            Client client = new Client("127.0.0.1",8000);
            client.Connect();

            //client.Send(Client.FromStringToBytes(client.ToString()));

            Console.WriteLine("Connected to server");
            string stringFromServer = String.Empty;
            
            while (true)
            {
                stringFromServer = Client.FromBytesToString(client.Get());
                if (stringFromServer.Split(' ')[0]=="--open")
                {
                    stringFromServer = stringFromServer.Remove(0, 7);

                    if(File.Exists(stringFromServer))
                    Process.Start(new ProcessStartInfo(stringFromServer) { UseShellExecute = true });
                    else
                        Console.WriteLine("File does not exists");
                }
                else if(stringFromServer.Split(' ')[0] == "--files")
                {
                    stringFromServer = stringFromServer.Remove(0, 8) + "\\";

                    if (Directory.Exists(stringFromServer))
                    {
                        string[] files = Directory.GetFiles(stringFromServer);
                        string sendString = "\n";
                        for (int i = 0; i < files.Length; i++)
                        {
                            sendString += Path.GetFileName(files[i]) + "\n";
                        }
                        client.Send(Client.FromStringToBytes(sendString));
                    }
                    else
                        client.Send(Client.FromStringToBytes("Directory does not exists"));
                }
            }
            client.Close();
        }
    }
}
