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
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;

namespace computing
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public double xaxis = 0;//x轴起点
        public double yaxis1 = -22;//最小温度
        public double yaxis2 = 0;//最小风速
        public double yaxis3 = 0;//最小可见度
        public double yaxis4 = 0;//最小气压
        public double yaxis5 = 0;//最小可见度
        public double yaxis6 = -30;//最小体感温度
        public int yaxis7 = 0;//最小风向角度
        public double group = 100;//可视间距
        public ObservableDataSource<Point> dataSource1 = new ObservableDataSource<Point>();//温度绿线
        public ObservableDataSource<Point> dataSource2 = new ObservableDataSource<Point>();//风速绿线
        public ObservableDataSource<Point> dataSource3 = new ObservableDataSource<Point>();//可见度绿线
        public ObservableDataSource<Point> dataSource4 = new ObservableDataSource<Point>();//气压绿线
        public ObservableDataSource<Point> dataSource5 = new ObservableDataSource<Point>();//湿度绿线
        public ObservableDataSource<Point> dataSource6 = new ObservableDataSource<Point>();//体感温度绿线
        public ObservableDataSource<Point> dataSource7 = new ObservableDataSource<Point>();//温度红线
        public ObservableDataSource<Point> dataSource8 = new ObservableDataSource<Point>();//风速红线
        public ObservableDataSource<Point> dataSource9 = new ObservableDataSource<Point>();//可见度红线
        public ObservableDataSource<Point> dataSource10 = new ObservableDataSource<Point>();//气压红线
        public ObservableDataSource<Point> dataSource11 = new ObservableDataSource<Point>();//湿度红线
        public ObservableDataSource<Point> dataSource12 = new ObservableDataSource<Point>();//体感温度红线
        public ObservableDataSource<Point> dataSource13 = new ObservableDataSource<Point>();//风向角度绿线
        public ObservableDataSource<Point> dataSource14 = new ObservableDataSource<Point>();//风向角度红线
        public LineGraph l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14;//预测与实时曲线
        public Thread thread, thread2, thread3;//端口监听线程
        public static Message message;
        public static DateTime datetime = new DateTime(2006, 4, 1, 0, 0, 0);
        public static ObservableCollection<Message> messageData = new ObservableCollection<Message>();//超实时信息表格修改
        public static List<Message> datalist = Dao.SelectData();
        public static int i = 0;//异常时刻
        public static int j = 0;
        public static DispatcherTimer timer = new DispatcherTimer();//超实时计算定时器
        public static int endhour;//计算时长
        public static int ratio;//计算倍率
        public static int onetime;//单位计算步长

        public MainWindow()
        {
            InitializeComponent();
            syssta.Text = "正常";
            changesta.Text = "未切换";
            resetsta.Text = "未复位";
            thread = new Thread(new ThreadStart(()=>TcpListen.RedSocketListen(this,thread)));
            thread.Start();
            thread2 = new Thread(new ThreadStart(()=>TcpListen.QuesSocketListen(this)));
            thread2.Start();
            thread3 = new Thread(new ThreadStart(()=>TcpListen.TimeSocketListen(this)));
            thread3.Start();
            l1 = plotter1.AddLineGraph(dataSource1, Colors.Green, 2, "预测曲线");//温度绿线
            l2 = plotter2.AddLineGraph(dataSource2, Colors.Green, 2, "预测曲线");//风速绿线
            l3 = plotter3.AddLineGraph(dataSource3, Colors.Green, 2, "预测曲线");//可见度绿线
            l4 = plotter4.AddLineGraph(dataSource4, Colors.Green, 2, "预测曲线");//气压绿线
            l5 = plotter5.AddLineGraph(dataSource5, Colors.Green, 2, "预测曲线");//湿度绿线
            l6 = plotter6.AddLineGraph(dataSource6, Colors.Green, 2, "预测曲线");//体感温度曲线
            l7 = plotter1.AddLineGraph(dataSource7, Colors.Red, 2, "实测曲线");//温度红线
            l8 = plotter2.AddLineGraph(dataSource8, Colors.Red, 2, "实测曲线");//风速红线
            l9 = plotter3.AddLineGraph(dataSource9, Colors.Red, 2, "实测曲线");//可见度红线
            l10 = plotter4.AddLineGraph(dataSource10, Colors.Red, 2, "实测曲线");//气压红线
            l11 = plotter5.AddLineGraph(dataSource11, Colors.Red, 2, "实测曲线");//湿度红线
            l12 = plotter6.AddLineGraph(dataSource12, Colors.Red, 2, "实测曲线");//湿度红线
            l13 = plotter7.AddLineGraph(dataSource13, Colors.Green, 2, "预测曲线");//风向绿线
            l14 = plotter7.AddLineGraph(dataSource14, Colors.Red, 2, "实测曲线");//风向红线

            plotter1.Viewport.Visible = new Rect(xaxis, yaxis1, group, 40 - yaxis1);
            plotter2.Viewport.Visible = new Rect(xaxis, yaxis2, group, 65 - yaxis2);
            plotter3.Viewport.Visible = new Rect(xaxis, yaxis3, group, 20 - yaxis3);
            plotter4.Viewport.Visible = new Rect(xaxis, yaxis4, group, 1050 - yaxis4);
            plotter5.Viewport.Visible = new Rect(xaxis, yaxis5, group, 1 - yaxis5);
            plotter6.Viewport.Visible = new Rect(xaxis, yaxis6, group, 40 - yaxis6);
            plotter7.Viewport.Visible = new Rect(xaxis, yaxis7, group, 370 - yaxis7);

            plotter1.LegendVisible = true;
            plotter2.LegendVisible = true;
            plotter3.LegendVisible = true;
            plotter4.LegendVisible = true;
            plotter5.LegendVisible = true;
            plotter6.LegendVisible = true;
            plotter7.LegendVisible = true;
        }

        public void DrawFirstPoint(MainWindow mainWindow,int messhour,double temp,double wind,double visible,double press,double humi,double apptemp,int windbear)
        {
            mainWindow.dataSource1.AppendAsync(base.Dispatcher, new Point(messhour, temp));
            mainWindow.dataSource2.AppendAsync(base.Dispatcher, new Point(messhour, wind));
            mainWindow.dataSource3.AppendAsync(base.Dispatcher, new Point(messhour, visible));
            mainWindow.dataSource4.AppendAsync(base.Dispatcher, new Point(messhour, press));
            mainWindow.dataSource5.AppendAsync(base.Dispatcher, new Point(messhour, humi));
            mainWindow.dataSource6.AppendAsync(base.Dispatcher, new Point(messhour, apptemp));
            mainWindow.dataSource13.AppendAsync(base.Dispatcher, new Point(messhour, windbear));
        }

        private void Exsimulate_Click(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds((int)(onetime / (ratio)));
            timer.Tick += ExShowPoint;
            timer.IsEnabled = true;
            timer.Start();
            exsimulate.IsEnabled = false;
        }

        private void ExShowPoint(object sender, EventArgs e)//定时器调用函数
        {
            if (j <= i + endhour * ratio)
            {
                AddMessage(datalist[j], Colors.Green);
                j++;
            }
            else
            {
                timer.Stop();
                timer.Tick -= ExShowPoint;
            }
        }

        private void Beyond_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (beyond.Text == endingtime.Text)
                generate.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private static double GetRandom(double min, double max)
        {
            Random random = new Random();
            return random.NextDouble() * (max - min) + min;
        }

        public void AddMessage(Message m, Color c)//显示曲线
        {
            if (c == Colors.Green)
            {
                Message message = SimulateCalculate.EveryHourCalculate(m);
                dataSource1.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Temp));
                dataSource2.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Wind));
                dataSource3.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Visible));
                dataSource4.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Press));
                dataSource5.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Humi));
                dataSource6.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Apptemp));
                dataSource13.AppendAsync(base.Dispatcher, new Point(message.Hour, message.Windbear));
                TimeSpan span = new TimeSpan(message.Hour / 24, message.Hour % 24, 0, 0);
                DateTime newDate = datetime + span;
                beyond.Text = newDate.ToString("yyyy-MM-dd HH:mm:ss");
                //加入数据库并显示历史记录
                Dao.InsertQuestionData(message);
                messageData.Add(message);
                messageData = new ObservableCollection<Message>(messageData.OrderBy(item =>item.Hour));
                dataGrid.DataContext = messageData;
            }
            else
            {
                dataSource7.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Temp));
                dataSource8.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Wind));
                dataSource9.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Visible));
                dataSource10.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Press));
                dataSource11.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Humi));
                dataSource12.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Apptemp));
                dataSource14.AppendAsync(base.Dispatcher, new Point(m.Hour, m.Windbear));
                TimeSpan span = new TimeSpan(m.Hour / 24, m.Hour % 24, 0, 0);
                DateTime newdate = datetime + span;
                now.Text = newdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void Now_TextChanged(object sender, TextChangedEventArgs e)//恢复正常运行
        {
            if (now.Text == foretime.Text && now.Text != "")
            {
                syssta.Text = "正常";
                syssta.Background = Brushes.White;
                syssta.Foreground = Brushes.Black;
                changesta.Text = "未切换";
                resetsta.Text = "未复位";
                question.Text = "";
                endingtime.Text = "";
                beyond.Text = "";
                foretime.Text = "";
                TcpSend.SendNormalSignal();
            }
        }

        private void Change_TextChanged(object sender, TextChangedEventArgs e)//换算函数
        {
            int number;
            bool success = int.TryParse(Change.Text, out number);
            if (success == false || number < 0)
            {
                return;
            }
            else
            {
                TimeSpan span = new TimeSpan(number / 24, number % 24, 0, 0);
                DateTime newdate = datetime + span;
                datedisplay.Text = newdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)//生成Excel文件
        {
            genestat.Foreground = Brushes.Black;
            genestat.Text = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files(*.xls)|All files(*.*)";
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "questiondata";
            saveFileDialog.RestoreDirectory = true;
            if(saveFileDialog.ShowDialog().Value == true)
            {
                string localfilepath = saveFileDialog.FileName.ToString();
                DataChangeExcel.DataSetToExcel(Dao.GetDataBaseTable(), localfilepath);
                genestat.Foreground = Brushes.Green;
                genestat.Text = "导出成功";
                generate.IsEnabled = false;
            }
            deletedata.IsEnabled = true;
        }

        private void Deletedata_Click(object sender, RoutedEventArgs e)
        {
            Dao.Delete();
            deletedata.IsEnabled = false;
        }
    }
}
