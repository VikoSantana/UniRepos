using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                string hostName = Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
                IPAddress ipAddres = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddres, port: 11000);

                Socket listener = new Socket(ipAddres.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    byte[] buffer = new byte[1024];
                    listener.Bind(localEndPoint);
                    listener.Listen(backlog: 100);
                    Socket handle = listener.Accept();

                    while (true)
                    {
                        string message = "";
                        while (true)
                        {

                            int messageSize = handle.Receive(buffer);
                            message += Encoding.ASCII.GetString(buffer, index: 0, count: messageSize);
                            if (message.Contains("<EOF>"))
                            {
                                message = message.Replace(oldValue: "<EOF>", newValue: "");
                                break;
                            }
                        }
                        Console.WriteLine(">" + message);
                        if (message == "exit")
                        {
                            handle.Shutdown(how: SocketShutdown.Both);
                            handle.Close();
                            break;
                        }
                    }
                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Clear();
                Console.WriteLine("Goodbye");
                Console.ReadKey(intercept: true);
            }
        }

    }
}