using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;    // кодирование - раскодирование
using System.Collections.Generic;   // LIST
using Microsoft.Data.Sqlite;

namespace ChatApp_Server
{
    internal class Program
    {
        static List<User> users = new List<User>();

        static public SqliteConnection connection = new SqliteConnection("Data Source=socketClient.db");

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            string ip_text = "127.0.0.1";
            int port = 7632;

            IPAddress iP = IPAddress.Parse(ip_text);

            Socket socketServer = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(iP, port);

            socketServer.Bind(endPoint);

            socketServer.Listen(10);  // сколько клиентов одновременно

            Console.WriteLine("[SERVER] Waiting clients...");
            while (true)
            {
                Socket socketClient = socketServer.Accept();  // lock thread
                User user = new User(socketClient);//socketClient
                //user.ClientSocket = socketClient;

                // упаковка в поток
                Thread managerThread = new Thread(WorkWithClient);
                managerThread.Start(user);

                Console.WriteLine("[SERVER] Client connected to server");
            }
        }

        public static void WorkWithClient(object objUser)
        {
            User user = (User)objUser;  // DownCast

            Thread threadHelperForReceive = new Thread(Receive);
            threadHelperForReceive.Start(user);
        }

        public static void Receive(object obj)
        {
            User user = (User)obj;   // DownCast

            try
            {
                // ЛОВИМ ИМЯ КЛИЕНТА
                byte[] bytes_name = new byte[256];
                int num_name = user.ClientSocket.Receive(bytes_name);
                string res = (Encoding.Unicode.GetString(bytes_name, 0, num_name));
                string name = res.Split("^")[1];
                int color = int.Parse(res.Split("^")[0]);
                user.Name = name;
                ConsoleColor consoleColor = ConsoleColor.White;
                if (color == 0)
                {
                    consoleColor = ConsoleColor.Red;
                }
                else if (color == 1)
                {
                    consoleColor = ConsoleColor.Green;
                }
                else if (color == 2)
                {
                    consoleColor = ConsoleColor.Blue;
                }
                else if (color == 3)
                {
                    consoleColor = ConsoleColor.Magenta;
                }
                else
                {
                    consoleColor = ConsoleColor.Yellow;
                }
                user.Color = consoleColor;
                users.Add(user);

                while (true)
                {
                    // Получаем данные от клиента
                    byte[] bytes = new byte[256];   // буфер для получения данных
                    int num = user.ClientSocket.Receive(bytes);    // lock for thread
                    string text = Encoding.Unicode.GetString(bytes, 0, num);
                    Console.ForegroundColor = user.Color;
                    Console.WriteLine("[SERVER] Message from [" + DateTime.Now.ToString("T") + "] "+name + ": " + text);
                    Console.ResetColor();

                    // Делаем рассылку всем!!!
                    SendMessageEveryone(text, user);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("EXEPTION: " + ex.Message);
                Console.WriteLine(user.Name + " отключился от чата!");
                users.Remove(user);    // удаляем юзера из листа
                Console.ResetColor();
            }

            
        }

        public static void SendMessageEveryone(string message, User userSender)
        {
            for (int i = 0; i < users.Count; i++)
            {
                User user = users[i];
                //(int)userSender.Color
                int colorN = 0;
                if (userSender.Color == ConsoleColor.Red)
                {
                    colorN = 0;
                }
                else if (userSender.Color == ConsoleColor.Green)
                {
                    colorN = 1;
                }
                else if (userSender.Color == ConsoleColor.Blue)
                {
                    colorN = 2;
                }
                else if (userSender.Color == ConsoleColor.Magenta)
                {
                    colorN = 3;
                }
                else
                {
                    colorN = 4;
                }
                string text = colorN + "^[" + DateTime.Now.ToString("T") + "] " +userSender.Name + ": " + message;
                byte[] bytes = Encoding.Unicode.GetBytes(text);
                user.ClientSocket.Send(bytes);
            }
        }
    }

    class User
    {
        private Socket clientSocket;
        private string name;
        private ConsoleColor color;
        private int id;

        public static int count = 0;

        // свойства
        public Socket ClientSocket { get { return clientSocket; } set { clientSocket = value; } }
        public string Name { 
            get { return name; } 
            set { name = value; }
        }
        public ConsoleColor Color { get { return color; } set { color = value; } }
       

        public User(Socket socket)
        {
            this.clientSocket = socket;

            id = count;
            count++;

            //this.color = (ConsoleColor)(count % 15 + 1);
        }
    }
}
