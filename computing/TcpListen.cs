using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace computing
{
    class TcpListen
    {
        public static void RedSocketListen(MainWindow mainWindow,Thread thread)//实时曲线监听函数
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));

            while (true)
            {
                listener.Listen(0);
                Socket socket = listener.Accept();
                NetworkStream networkStream = new NetworkStream(socket);
                StreamReader streamReader = new StreamReader(networkStream);
                string result = streamReader.ReadToEnd();
                Console.WriteLine(result);
                //处理数据
                try
                {
                    string[] results = result.Split('|');
                    int hours = int.Parse(results[0]);
                    double temp = double.Parse(results[1]);
                    double wind = double.Parse(results[2]);
                    double visible = double.Parse(results[3]);
                    double press = double.Parse(results[4]);
                    double humi = double.Parse(results[5]);
                    double apptemp = double.Parse(results[6]);
                    int windbear = int.Parse(results[7]);
                    MainWindow.message = new Message(hours, temp, apptemp, wind, visible, press, humi, windbear);
                    Application.Current.Dispatcher.Invoke(() => mainWindow.AddMessage(MainWindow.message, Colors.Red));
                    streamReader.Close();
                    networkStream.Close();
                    socket.Close();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                    streamReader.Close();
                    networkStream.Close();
                    socket.Close();
                    thread.Abort();
                    MessageBox.Show("数据发送完毕", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
            }
            //不关闭listener,关掉页面时由Exit函数回收
        }

        public static void QuesSocketListen(MainWindow mainWindow) //异常监听函数
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000));
            while (true)
            {
                listener.Listen(0);
                Socket socket = listener.Accept();
                NetworkStream networkStream = new NetworkStream(socket);
                StreamReader streamReader = new StreamReader(networkStream);
                string quesstr = streamReader.ReadToEnd();
                if (quesstr != "" && quesstr != null)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                    mainWindow.syssta.Text = "异常";
                    mainWindow.syssta.Background = Brushes.Red;
                    mainWindow.syssta.Foreground = Brushes.White;
                    mainWindow.changesta.Text = "已切换";
                    mainWindow.resetsta.Text = "已复位";
                    string[] results = quesstr.Split('|');
                    int queshour = int.Parse(results[0]);
                    MainWindow.endhour = int.Parse(results[1]);
                    MainWindow.ratio = int.Parse(results[2]);
                    int messhour = int.Parse(results[3]);
                    double temp = double.Parse(results[4]);
                    double wind = double.Parse(results[5]);
                    double visible = double.Parse(results[6]);
                    double press = double.Parse(results[7]);
                    double humi = double.Parse(results[8]);
                    double apptemp = double.Parse(results[9]);
                    int windbear = int.Parse(results[10]);
                    MainWindow.onetime = int.Parse(results[11]);
                    MainWindow.i = queshour;
                    TimeSpan span1 = new TimeSpan(queshour / 24, queshour % 24, 0, 0);
                    DateTime newDate1 = MainWindow.datetime + span1;
                    mainWindow.question.Text = newDate1.ToString("yyyy-MM-dd HH:mm:ss");
                    TimeSpan span2 = new TimeSpan((MainWindow.endhour * MainWindow.ratio) / 24, (MainWindow.endhour * MainWindow.ratio) % 24, 0, 0);
                    DateTime newDate2 = newDate1 + span2;
                    mainWindow.endingtime.Text = newDate2.ToString("yyyy-MM-dd HH:mm:ss");
                    mainWindow.DrawFirstPoint(mainWindow, messhour, temp, wind, visible, press, humi, apptemp, windbear);
                    MainWindow.j = MainWindow.i + 1;
                    mainWindow.exsimulate.IsEnabled = true;
                    });
                }
                streamReader.Close();
                networkStream.Close();
                socket.Close();
            }
        }

        public static void TimeSocketListen(MainWindow mainWindow)//停堆预测时间监听函数
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9996));
            while (true)
            {
                listener.Listen(0);
                Socket socket = listener.Accept();
                NetworkStream networkStream = new NetworkStream(socket);
                StreamReader streamReader = new StreamReader(networkStream);
                string catandper = streamReader.ReadToEnd();
                if (catandper != "" && catandper != null)
                {
                    string timestr = TimeCalculate.StopTimeCalculate(catandper, MainWindow.i, MainWindow.endhour, MainWindow.ratio);
                    Application.Current.Dispatcher.Invoke(() => { mainWindow.foretime.Text = timestr; });
                }
                streamReader.Close();
                networkStream.Close();
                socket.Close();
            }
        }
    }
}
