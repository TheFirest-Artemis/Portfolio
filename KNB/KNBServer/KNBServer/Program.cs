using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KNBServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "KNB Server";

            string ip_ad = "127.0.0.1";
            int port = 8005;
            IPAddress ip = IPAddress.Parse(ip_ad);

            IPEndPoint endPoint = new IPEndPoint(ip, port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);

            socket.Listen(1);

            Console.WriteLine("[SERVER] start listening");

            Socket client_socket = socket.Accept();

            Console.WriteLine("[SERVER] к серверу подключился клиент\n");
            while (true)
            {
                try
                {
                    string message = ReceiveDataFromNet(client_socket);

                    //Console.WriteLine("[SERVER] Clien message: " + message);

                    Console.Write("Введите ваш предмет: ");

                    string text = Console.ReadLine();
                    string answer = "";

                    if (message == text)
                    {
                        answer = "Ничья";
                    }
                    else if ((message == "к" && text == "н") || (message == "н" && text == "б") || (message == "б" && text == "к"))
                    {
                        answer = "Игрок 1 выиграл";
                    }
                    else
                    {
                        answer = "Игрок 2 выиграл";
                    }

                    Console.WriteLine("Исход: " + answer + "\n");
                    SendDataToNet(client_socket, answer);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();

                    break;
                }
            }
        }

        private static string ReceiveDataFromNet(Socket socket)
        {
            byte[] data = new byte[256];
            int num_bites = socket.Receive(data);
            string message = Encoding.Unicode.GetString(data, 0, num_bites);

            return message;
        }

        private static void SendDataToNet(Socket socket, string text)
        {
            byte[] text_bites = Encoding.Unicode.GetBytes(text);
            socket.Send(text_bites);
        }

    }
}
