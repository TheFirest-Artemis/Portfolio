using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;    // кодирование - раскодирование
using System.Collections.Generic;   // LIST
//using Microsoft.Data.Sqlite;

namespace UniversalServer_v1._0
{
    internal class Program
    {
        static List<User> users = new List<User>();
        static List<string> history_chats = new List<string>();

        //static public SqliteConnection connection = new SqliteConnection("Data Source=socketClient.db");

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Ip: ");
            string ip_text = Console.ReadLine();
            Console.WriteLine("Ip: ");
            int port = int.Parse(Console.ReadLine());

            //string ip_text = "127.0.0.1";
            //int port = 7632;

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

                Thread threadOne = new Thread(SendMessageOne);
                threadOne.Start(user);


                while (true)
                {
                    // Получаем данные от клиента
                    byte[] bytes = new byte[256];   // буфер для получения данных
                    int num = user.ClientSocket.Receive(bytes);    // lock for thread
                    string text = Encoding.Unicode.GetString(bytes, 0, num);
                    Console.ForegroundColor = user.Color;
                    Console.WriteLine("[SERVER] Message from [" + DateTime.Now.ToString("T") + "] " + name + ": " + text);
                    Console.ResetColor();

                    // Делаем рассылку всем!!!
                    history_chats.Add(user.Name + " " + user.Color + " " + text);
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
                string text = colorN + "^[" + DateTime.Now.ToString("T") + "] " + userSender.Name + ": " + message;
                byte[] bytes = Encoding.Unicode.GetBytes(text);
                user.ClientSocket.Send(bytes);
            }
        }


        public static void SendMessageOne(object obj)
        {
            User userSender = (User)obj;
            for (int i = 0; i < history_chats.Count; i++)
            {
                int colorN = 0;
                if (history_chats[i].Split()[1] == "Red")
                {
                    colorN = 0;
                }
                else if (history_chats[i].Split()[1] == "Green")
                {
                    colorN = 1;
                }
                else if (history_chats[i].Split()[1] == "Blue")
                {
                    colorN = 2;
                }
                else if (history_chats[i].Split()[1] == "Purple")
                {
                    colorN = 3;
                }
                else
                {
                    colorN = 4;
                }
                string text = colorN + "^[" + DateTime.Now.ToString("T") + "] " + history_chats[i].Split()[0] + ": " + history_chats[i].Split()[2];
                byte[] bytes = Encoding.Unicode.GetBytes(text);
                userSender.ClientSocket.Send(bytes);
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
        public string Name
        {
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
