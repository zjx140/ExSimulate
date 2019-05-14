using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace TcpSendTest
{
    class TcpSend
    {
        public static void SendRedData(MainWindow mainWindow,Timer timer)//发送实时数据
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.NoDelay = true;
                tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 9999);
                NetworkStream ns = tcpClient.GetStream();
                if (MainWindow.i < MainWindow.datalist.Count && ns.CanWrite)
                {
                    Byte[] bytSend = Encoding.UTF8.GetBytes(MainWindow.datalist[MainWindow.i].ToString());
                    ns.Write(bytSend, 0, bytSend.Length);
                    ns.Flush();
                    if (!MakeSure.IsNormal(MainWindow.datalist[MainWindow.i], MainWindow.temp_minnum, MainWindow.temp_maxnum, MainWindow.wind_maxnum, MainWindow.humi_minnum, MainWindow.humi_maxnum, MainWindow.visible_minnum) && MainWindow.flag == 0)//第一次出现异常
                    {
                        MainWindow.player.Load();
                        MainWindow.player.PlayLooping();
                        MainWindow.queshour = MainWindow.i;
                        Application.Current.Dispatcher.Invoke(() => {
                            mainWindow.status.Text = "异常";
                            mainWindow.status.Background = Brushes.Red;
                            mainWindow.status.Foreground = Brushes.White;
                            mainWindow.stopnoise.IsEnabled = true;
                            mainWindow.extime.IsEnabled = true;
                            mainWindow.ratio.IsEnabled = true;
                            mainWindow.send.IsEnabled = true;
                        });
                        MainWindow.message = MainWindow.datalist[MainWindow.i];
                        MainWindow.flag++;
                    }
                    MainWindow.i++;
                }
                else
                {
                    ns.Close();
                    timer.Stop();
                    mainWindow.button.IsEnabled = true;
                    return;
                }
                ns.Close();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
                MessageBox.Show("IP地址设置错误或接收端异常", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                tcpClient.Close();
            }
        }

        public static void SendQuestionSignal(int hour, int duration, int rationum, Message message, int onetime, IPAddress iPAddress)//发送异常信号给诊断和仿真机
        {
            TcpClient tcpClient1 = new TcpClient();
            tcpClient1.NoDelay = true;
            tcpClient1.Connect(iPAddress, 10000);
            NetworkStream networkStream = tcpClient1.GetStream();
            if (networkStream.CanWrite)
            {
                Byte[] bytSend1 = Encoding.UTF8.GetBytes(hour + "|" + duration + "|" + rationum + "|" + message.ToString() + "|" + onetime);
                networkStream.Write(bytSend1, 0, bytSend1.Length);
                networkStream.Flush();
            }
            networkStream.Close();
            tcpClient1.Close();
        }

        public static void SendTimeInform(string catandper,IPAddress iPAddress)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.NoDelay = true;
            tcpClient.Connect(iPAddress, 9996);
            NetworkStream ns = tcpClient.GetStream();
            Byte[] bytSend = Encoding.UTF8.GetBytes(catandper);
            ns.Write(bytSend, 0, bytSend.Length);
            ns.Flush();
            ns.Close();
            tcpClient.Close();
        }
    }
}
