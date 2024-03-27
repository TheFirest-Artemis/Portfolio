using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KNBClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "KNB Client";

            int server_port = 8005;
            string server_ip_adress = "127.0.0.1";
            IPAddress server_ip = IPAddress.Parse(server_ip_adress);

            IPEndPoint endPoint = new IPEndPoint(server_ip, server_port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endPoint);

            Console.WriteLine("Подключение к серверу прошло успешно\n");

            while (true)
            {
                Console.Write("Введите ваш предмет: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string message = Console.ReadLine();
                Console.ResetColor();
                SendDataToNet(socket, message);

                Thread.Sleep(500);
                Console.WriteLine("Оппонент думает");
                Thread.Sleep(500);

                string server_message = ReceiveDataFromNet(socket);


                Console.Write("Исход: ");
                Console.WriteLine(server_message + "\n");
                Console.ResetColor();
            }

        }

        private static void SendDataToNet(Socket socket, string text)
        {
            byte[] text_bites = Encoding.Unicode.GetBytes(text);
            socket.Send(text_bites);
        }

        private static string ReceiveDataFromNet(Socket socket)
        {
            byte[] data = new byte[256];
            int num_bites = socket.Receive(data);
            string message = Encoding.Unicode.GetString(data, 0, num_bites);

            return message;
        }

    }
}
