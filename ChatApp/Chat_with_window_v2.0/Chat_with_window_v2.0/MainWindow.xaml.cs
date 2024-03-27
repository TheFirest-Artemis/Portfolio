using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Threading;
using System.Drawing;
using System.Runtime.Intrinsics.X86;

namespace Chat_with_window_v2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string colorN = "";
        static string text = "";
        Socket socket;
        static string[] chats = new string[128];
        public MainWindow()
        {
            InitializeComponent();
            send.IsEnabled = false;
            messageT.IsEnabled = false;

            StreamReader streamReader1 = new StreamReader("servers.txt");
            int len = streamReader1.ReadToEnd().Split("\n").Length;
            streamReader1.Close();
            StreamReader streamReader2 = new StreamReader("servers.txt");
            string nameV = "";
            for (int i = 0; i < len-1; i++)
            {
                string[] line = (streamReader2.ReadLine()).Split();
                if (line[2] == nameV || nameV == "")
                {
                    chatsNames.Items.Add(line[0]);
                    serverIp_Port.Text = line[1];
                    serverName.Text = line[0];
                    clientName.Text = line[2];
                    nameV = line[2];
                    colorN = line[3];
                    chats[i] = line[1];

                    serverIp_Port.IsEnabled = false;
                    serverName.IsEnabled = false;
                    clientName.IsEnabled = false;
                    BoxColors.IsEnabled = false;
                    //enter.IsEnabled = false;
                    send.IsEnabled = true;
                    messageT.IsEnabled = true;
                    messageT.Focus();
                }
            }
            streamReader2.Close();

            try
            {
                Ipandsocket(serverIp_Port.Text.Split(":")[0], int.Parse(serverIp_Port.Text.Split(":")[1]));
            }
            catch (Exception ex)
            {

            }
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (serverIp_Port.IsEnabled == true)
            {
                string ip_text = "";
                int port = 80;
                bool flag = false;
                try
                {
                    ip_text = serverIp_Port.Text.Split(":")[0];
                    try
                    {
                        port = int.Parse(serverIp_Port.Text.Split(":")[1]);
                    }
                    finally { }
                }
                catch (Exception)
                {
                    flag = true;
                }

                if (clientName.Text == "")
                {
                    MessageBox.Show("Введите ваше имя в поле Your Name");
                }
                else if (clientName.Text.Length < 3)
                {
                    MessageBox.Show("Ваше имя должно быть больше 2 символов");
                }
                else if (serverIp_Port.Text == "")
                {
                    MessageBox.Show("Введите адрес сервера в формате Ip:Port\nВ поле Server Ip:Port");
                }
                else if (flag)
                {
                    MessageBox.Show("Формат Ip:Port неверен");
                }
                else
                {
                    if (BoxColors.SelectedItem == null)
                    {
                        Random colorRandom = new Random();
                        int cR = colorRandom.Next(5);
                        colorN = cR.ToString();

                    }
                    else
                    {
                        colorN = BoxColors.SelectedIndex.ToString();
                    }

                    if (serverName.Text != "")
                    {
                        string server_name = serverName.Text;
                        int count_name = 0;
                        for (int i = 0; i < chatsNames.Items.Count - 2; i++)
                        {
                            if (server_name == chatsNames.Items[i].ToString())
                            {
                                count_name++;
                                server_name = serverName.Text + "(" + count_name + ")";
                            }
                        }
                        chatsNames.Items.Add(server_name);
                        StreamWriter iostream = new StreamWriter("servers.txt", true);
                        iostream.WriteLine(server_name + " " + serverIp_Port.Text + " " + clientName.Text + " " + colorN);
                        iostream.Close();
                    }

                    Ipandsocket(ip_text, port);

                    serverIp_Port.IsEnabled = false;
                    serverName.IsEnabled = false;
                    clientName.IsEnabled = false;
                    BoxColors.IsEnabled = false;
                    //enter.IsEnabled = false;
                    send.IsEnabled = true;
                    messageT.IsEnabled = true;
                    messageT.Focus();
                }
            }
            else
            {
                socket.Close();
                serverIp_Port.IsEnabled = true;
                serverName.IsEnabled = true;
                clientName.IsEnabled = true;
                BoxColors.IsEnabled = true;
                send.IsEnabled = false;
                messageT.IsEnabled = false;
                chat.Text = "";
            }
        }

        private void Ipandsocket(string ip_text, int port)
        {
            IPAddress iP = IPAddress.Parse(ip_text);

            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(iP, port);
            socket.Connect(endPoint);   // lock thread
                                        //MessageBox.Show("Подключились");
            socket.Send(Encoding.Unicode.GetBytes(colorN + "^" + clientName.Text));

            Thread threadSend = new Thread(Priem);
            threadSend.Start();
        }

        private void Priem()
        {
            while (true)
            {
                try
                {
                    // Получаем данные с сервера
                    byte[] bytes = new byte[256];   // буфер для получения данных
                    int num = socket.Receive(bytes);    // lock for thread
                    string res = Encoding.Unicode.GetString(bytes, 0, num);
                    text = res.Split("^")[1];
                    int color = int.Parse(res.Split("^")[0]);
                    SolidColorBrush consoleColor = Brushes.WhiteSmoke;
                    string[] partsText = res.Split('^');
                    if (color == 0)
                    {
                        consoleColor = Brushes.Red;
                    }
                    else if (color == 1)
                    {
                        consoleColor = Brushes.Green;
                    }
                    else if (color == 2)
                    {
                        consoleColor = Brushes.Blue;
                    }
                    else if (color == 3)
                    {
                        consoleColor = Brushes.Violet;
                    }
                    else
                    {
                        consoleColor = Brushes.DeepPink;
                    }

                    this.Dispatcher.Invoke((ThreadStart)delegate
                    {
                        Run run = new Run(text +
                        "\n"); run.Foreground = consoleColor; chat.Inlines.Add(run);
                    });
                }
                catch(Exception)
                { 

                }

            }
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            string text = messageT.Text;
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            socket.Send(bytes);
            messageT.Text = "";
            messageT.Focus();
        }

        private void chatsNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            socket.Close();
            chat.Text = "";
            int index = this.chatsNames.SelectedIndex;
            /*StreamReader streamReader2 = new StreamReader("servers.txt");
            for (int i = 0; i < index-1; i++)
            {
                streamReader2.ReadLine();
            }
            string[] line = streamReader2.ReadLine().Split();
            streamReader2.Close();*/

            serverIp_Port.Text = chats[index];
            serverName.Text = chatsNames.SelectedItem.ToString();
            //clientName.Text = line[2];
            //colorN = line[3];

            serverIp_Port.IsEnabled = false;
            serverName.IsEnabled = false;
            clientName.IsEnabled = false;
            BoxColors.IsEnabled = false;
            //enter.IsEnabled = false;
            send.IsEnabled = true;
            messageT.IsEnabled = true;
            messageT.Focus();

            try
            {
                Ipandsocket(serverIp_Port.Text.Split(":")[0], int.Parse(serverIp_Port.Text.Split(":")[1]));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
