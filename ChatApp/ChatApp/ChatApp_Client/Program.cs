using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;              // for encoding

namespace ChatApp_Client
{
    internal class Program
    {
        static public string name = "";
        static void Main(string[] args)
        {
            Console.WriteLine("Push Enter To Start...");
            Console.ReadLine();

            string ip_text = "127.0.0.1";
            int port = 7632;

            IPAddress iP = IPAddress.Parse(ip_text);

            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(iP, port);

            Console.WriteLine("[CLIENT] Started...");

            // ОТПРАВКА ИМЕНИ
            Console.Write("Введите ваше имя для чата: ");
            name = Console.ReadLine();

            socket.Connect(endPoint);   // lock thread

            Console.WriteLine("[CLIENT] Connected to server!");

            socket.Send(Encoding.Unicode.GetBytes(name));
            Console.WriteLine(name + " добро пожаловать в чат!");

            Thread threadSend = new Thread(Send);
            threadSend.Start(socket);

            while (true)
            {
                // Получаем данные с сервера
                byte[] bytes = new byte[256];   // буфер для получения данных
                int num = socket.Receive(bytes);    // lock for thread
                string text = Encoding.Unicode.GetString(bytes, 0, num);

                string[] partsText = text.Split(new char[] { ',' },2);

                Console.ForegroundColor = (ConsoleColor)int.Parse(partsText[0]);
                Console.WriteLine(partsText[1]);
                Console.ResetColor();
            }

            Console.ReadLine();   // pause
        }

        public static void Send(object obj)
        {
            Socket socket = (Socket)obj;

            // МЫ В ЧАТЕ. ОТПРАВКА СООБЩЕНИЙ
            while (true)
            {
                string message = Console.ReadLine();

                string text = message;
                byte[] bytes = Encoding.Unicode.GetBytes(text);
                socket.Send(bytes);

                //Thread.Sleep(5000);
            }
        }
    }
}
