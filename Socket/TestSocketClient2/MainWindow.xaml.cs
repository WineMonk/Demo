using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestSocketClient2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Socket _clientSocket;

        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            //声明负责通信的套接字
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            WriteConsole("正在连接...");
            IPAddress IP = IPAddress.Parse(textBoxIP.Text);
            int Port = int.Parse(textBoxPort.Text);
            IPEndPoint iPEndPoint = new IPEndPoint(IP, Port);
            try
            {
                //用socket对象的Connect()方法以上面建立的IPEndPoint对象做为参数，向服务器发出连接请求
                _clientSocket.Connect(iPEndPoint);
                WriteConsole($"{textBoxIP.Text}连接成功");
                ChangButtonState(true);
                Task.Factory.StartNew(() =>
                {
                    while (_clientSocket.Connected)
                    {
                        try
                        {
                            //调用Receive()接收字节数据
                            byte[] receive = new byte[1024];
                            _clientSocket.Receive(receive);
                            if (receive.Length > 0)
                            {
                                WriteConsole($"接收 - {Encoding.UTF8.GetString(receive)}");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteConsole(ex.Message);
                        }
                    }
                    ChangButtonState(false);
                });
            }
            catch
            {
                MessageBox.Show("服务器未打开");
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            //关闭套接字
            _clientSocket.Close();
            MessageBox.Show("已关闭连接");
            WriteConsole("客户端已关闭");
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            byte[] send = Encoding.UTF8.GetBytes(textBoxInput.Text);
            //调用Send()函数发送数据
            _clientSocket.Send(send);
            WriteConsole($"发送 - {textBoxInput.Text}");
            textBoxInput.Clear();
        }

        private void ChangButtonState(bool connected)
        {
            this.Dispatcher.Invoke(() =>
            {
                buttonClose.IsEnabled = connected;
                buttonSend.IsEnabled = connected;
                buttonConnect.IsEnabled = !connected;
            });
        }

        private void WriteConsole(string msg)
        {
            this.Dispatcher.Invoke(() =>
            {
                textBoxConsole.Text += $"{DateTime.Now:yy-MM-dd hh:mm:ss}  {msg}\r\n";
            });
        }
    }
}