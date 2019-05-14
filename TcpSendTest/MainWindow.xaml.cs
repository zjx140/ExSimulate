using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Timer = System.Timers.Timer;

namespace TcpSendTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Message> datalist = Dao.SelectData();
        public static int i = 0;//红线
        public static int queshour = 0;//异常时刻
        public static Timer timer = new Timer();//红线定时器
        public static IPAddress iPAddress;//数据要发送的地址
        private static int exduration;//计算时长
        private static int rationum;//计算倍率
        public static double temp_maxnum, temp_minnum, wind_maxnum, visible_minnum, humi_maxnum, humi_minnum;//参数阈值
        public static int flag = 0;//异常信号发送标记值
        public static Message message;//异常详细信息
        private Thread thread;//恢复正常监听线程
        public static SoundPlayer player = new SoundPlayer(Resource1.attention);//警报声
        private static int onetime;//发送数据步长

        public MainWindow()
        {
            InitializeComponent();
            status.Text = "正常";
            //读取配置
            XmlDocument xml = new XmlDocument();
            xml.Load("config.xml");
            XmlNode root = xml.SelectSingleNode("config");
            XmlNode ipaddresstxt = root.SelectSingleNode("ip-address");
            XmlNode temp_max = root.SelectSingleNode("temp-max");
            XmlNode temp_min = root.SelectSingleNode("temp-min");
            XmlNode wind_max = root.SelectSingleNode("wind-max");
            XmlNode visible_min = root.SelectSingleNode("visible-min");
            XmlNode humi_max = root.SelectSingleNode("humi-max");
            XmlNode humi_min = root.SelectSingleNode("humi-min");
            XmlNode one_time = root.SelectSingleNode("onetime");
            iPAddress = IPAddress.Parse(ipaddresstxt.InnerText);
            temp_maxnum = double.Parse(temp_max.InnerText);
            temp_minnum = double.Parse(temp_min.InnerText);
            wind_maxnum = double.Parse(wind_max.InnerText);
            visible_minnum = double.Parse(visible_min.InnerText);
            humi_maxnum = double.Parse(humi_max.InnerText);
            humi_minnum = double.Parse(humi_min.InnerText);
            onetime = int.Parse(one_time.InnerText);
            onetime *= 1000;
            Console.WriteLine(iPAddress.ToString() + "|" + temp_maxnum + "|" + temp_minnum + "|" + wind_maxnum + "|" + visible_minnum + "|" + humi_maxnum + "|" + humi_minnum);
            thread = new Thread(new ThreadStart(()=>TcpListen.NormalSocketListen(this)));
            thread.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)//开启实时定时器
        {
            button.IsEnabled = false;
            button1.IsEnabled = true;
            timer.Elapsed += new ElapsedEventHandler(delegate { TcpSend.SendRedData(this, timer);});
            timer.Enabled = true;
            timer.Interval = onetime;
            timer.Start();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            timer.Enabled = !timer.Enabled;
            if(button1.Content.Equals("暂停发送"))
            {
                button1.Content = "继续发送";
            }
            else
            {
                button1.Content = "暂停发送";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            TcpSend.SendQuestionSignal(queshour, exduration,rationum,message,onetime,iPAddress);//发送异常信号给诊断和仿真机
            sendtime.IsEnabled = true;
        }

        private void Stopnoise_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            stopnoise.IsEnabled = false;
        }

        private void Extime_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool success = int.TryParse(extime.Text, out exduration);
            if(success == false)
            {
                return;
            }
        }

        private void Ratio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = ratio.SelectedItem as ComboBoxItem;
            string combotxt = comboBoxItem.Content.ToString();
            rationum = int.Parse(combotxt);
        }

        private void Sendtime_Click(object sender, RoutedEventArgs e)
        {
            string catandper = MakeSure.EnsureQues(message, temp_minnum, temp_maxnum, wind_maxnum, humi_minnum, humi_maxnum, visible_minnum);
            TcpSend.SendTimeInform(catandper,iPAddress);
            sendtime.IsEnabled = false;
        }
    }
}
