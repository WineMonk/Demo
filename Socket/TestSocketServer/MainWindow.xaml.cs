using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace TestSocketServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket _serverSocket;
        private List<Socket> _acceptSockets = new List<Socket>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            //创建用于监听连接的套接字
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
            IPAddress ipAddress = IPAddress.Parse(textBoxIP.Text);
            int port = int.Parse(textBoxPort.Text);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            //绑定端口
            _serverSocket.Bind(ipEndPoint);
            _serverSocket.Listen(10);
            ChangButtonState(true);
            Task.Factory.StartNew(() =>
            {
                //开始监听客户端连接
                while (true)
                {
                    try
                    {
                        //接收客户端连接
                        Socket acceptSocket = _serverSocket.Accept();
                        //记录客户端连接套接字
                        _acceptSockets.Add(acceptSocket);
                        IPEndPoint? iPEndPoint = acceptSocket.RemoteEndPoint as IPEndPoint;
                        string? clientAddress = null;
                        if (iPEndPoint != null)
                        {
                            clientAddress = $"{iPEndPoint.Address}:{ iPEndPoint.Port}";
                        }
                        WriteConsole($"客户端连接成功 - {clientAddress}");
                        Task.Factory.StartNew(() =>
                        {
                            //开始监听客户端消息
                            while (acceptSocket.Connected)
                            {
                                try
                                {
                                    byte[] recieve = new byte[1024];
                                    acceptSocket.Receive(recieve);
                                    if (recieve.Length > 0)
                                    {
                                        //打印接收数据
                                        WriteConsole($"接收 - {Encoding.UTF8.GetString(recieve)}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteConsole(ex.Message);
                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        WriteConsole(ex.Message);
                        break;
                    }
                }
            });
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            _serverSocket.Close();
            _acceptSockets.ForEach(acceptSocket => { acceptSocket.Close(); });
            _acceptSockets.Clear();
            MessageBox.Show("服务器已关闭");
            WriteConsole("服务器已关闭");
            ChangButtonState(false);
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            
            byte[] send = Encoding.UTF8.GetBytes(textBoxInput.Text);
            List<Socket> rmsk = new List<Socket>();
            _acceptSockets.ForEach(socket =>
            {
                if (socket.Connected)
                {
                    //调用Send()向客户端发送数据
                    socket.Send(send);
                }
                else
                {
                    rmsk.Add(socket);
                }
            }); 
            rmsk.ForEach(socket =>
            {
                _acceptSockets.Remove(socket);
            });
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