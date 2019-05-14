using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpSendTest
{
    public class Message
    {
        //要改的
        public int Hour { get; set; }
        public double Temp { get; set; }
        public double Apptemp { get; set; }
        public double Wind { get; set; }
        public double Visible { get; set; }
        public double Press { get; set; }
        public double Humi { get; set; }
        public int Windbear { get; set; }

        public Message(int hour, double temp, double apptemp, double wind, double visible, double press, double humi, int windbear)
        {
            Hour = hour;
            Temp = temp;
            Apptemp = apptemp;
            Wind = wind;
            Visible = visible;
            Press = press;
            Humi = humi;
            Windbear = windbear;
        }

        public Message()
        {

        }

        public override string ToString()
        {
            return Hour + "|" + Temp + "|" + Wind + "|" + Visible + "|" + Press + "|" + Humi + "|" + Apptemp + "|" + Windbear;
        }
    }
}
