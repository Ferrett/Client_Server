using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ClientServer;

namespace BroadcastMessengerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1",8000);
            server.Start();
            Task task = new Task(()=>server.ConnectionUpdate());
            task.Start();
            
            while (true)
            {
               
                if (server.handler.Count != 0)
                {
                    Console.WriteLine("\n[1] - Open app");
                    Console.WriteLine("[2] - Get files");
                    int menu = int.Parse(Console.ReadLine());
                    switch (menu)
                    {
                        case 1:
                            {
                                server.ShowAllUsers(server);

                                Console.WriteLine("Enter index of user:");

                                int index = int.Parse(Console.ReadLine())-1;

                                Console.WriteLine("Enter \"--open\" and app path");
                                string str = Console.ReadLine();
                                server.Send(Server.FromStringToBytes(str), index);
                                    
                                break;
                            }
                        case 2:
                            {
                                server.ShowAllUsers(server);

                                int index = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine("Enter \"--files\" and directory path");
                                string str = Console.ReadLine();
                                server.Send(Server.FromStringToBytes(str), index);

                                str = Server.FromBytesToString(server.Get(index));
                                Console.WriteLine(str);
                                break;
                            }
                        default:
                            break;
                    }
                   
                }
            }

            server.Close();
        }
    }
}
