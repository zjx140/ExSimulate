using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TcpSendTest
{
    class TcpListen
    {
        public static void NormalSocketListen(MainWindow mainWindow)
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(MainWindow.iPAddress, 9998));
            while (true)
            {
                listener.Listen(0);
                Socket socket = listener.Accept();
                NetworkStream network = new NetworkStream(socket);
                StreamReader reader = new StreamReader(network);
                string result = reader.ReadToEnd();
                if (result != "" && result != null)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        mainWindow.extime.Text = "";
                        mainWindow.extime.IsEnabled = false;
                        mainWindow.ratio.IsEnabled = false;
                        mainWindow.send.IsEnabled = false;
                        mainWindow.status.Text = "正常";
                        mainWindow.status.Background = Brushes.White;
                        mainWindow.status.Foreground = Brushes.Black;
                    });
                    MainWindow.flag--;
                }
            }
        }
    }
}
