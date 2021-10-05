using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ClientServer;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Clients
{
    class Program
    {
        static void Main(string[] args)
        {



            RegistryKey key = Registry.CurrentUser;

            string ip = string.Empty;

            if (key.GetSubKeyNames().Contains("ClientData"))
            {
                RegistryKey keyData = key.OpenSubKey("ClientData");
                Console.SetWindowSize(int.Parse(keyData.GetValue("Width").ToString()), int.Parse(keyData.GetValue("Heigth").ToString()));
                ip = keyData.GetValue("IP").ToString();
            }
            else
            {
                RegistryKey newKey = key.CreateSubKey("ClientData", true);

                newKey.SetValue("Width", 50);
                newKey.SetValue("Heigth", 50);
                newKey.SetValue("IP", "127.0.0.1");
                newKey.SetValue("LogPath", @"C:\Users\student\Desktop\Log.txt");
            }

            RegistryKey keyD = key.OpenSubKey("ClientData");
           

            Client client = new Client("127.0.0.1", 8000);
            client.Connect();

            Console.WriteLine("Connected to server");
            string stringFromServer = String.Empty;



            

            while (true)
            {
                try
                {
                    stringFromServer = Client.FromBytesToString(client.Get());

                    if (stringFromServer.Split(' ')[0] == "--open")
                    {
                        stringFromServer = stringFromServer.Remove(0, 7);

                        if (File.Exists(stringFromServer))
                            Process.Start(new ProcessStartInfo(stringFromServer) { UseShellExecute = true });
                        else
                            Console.WriteLine("File does not exists");
                    }
                    else if (stringFromServer.Split(' ')[0] == "--files")
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
                    else if (stringFromServer.Split(' ')[0] == "--res")
                    {

                        RegistryKey newKeyy = key.OpenSubKey("ClientData", true);

                        newKeyy.SetValue("Width", int.Parse(stringFromServer.Split(" ")[1].Split(",")[0]));
                        newKeyy.SetValue("Heigth", int.Parse(stringFromServer.Split(" ")[1].Split(",")[0]));


                        RegistryKey keyData = key.OpenSubKey("ClientData");
                        Console.SetWindowSize(int.Parse(keyData.GetValue("Width").ToString()), int.Parse(keyData.GetValue("Heigth").ToString()));
                    }
                    else if (stringFromServer.Split(' ')[0] == "--ip")
                    {

                        RegistryKey newKeyy = key.OpenSubKey("ClientData", true);

                        newKeyy.SetValue("IP", stringFromServer.Split(" ")[1]);



                        RegistryKey keyData = key.OpenSubKey("ClientData");
                        ip = keyData.GetValue("IP").ToString();
                    }
                }
                catch (Exception ex)
                {
                    
                    client.Close();
                }
            }


        }
    }
}
