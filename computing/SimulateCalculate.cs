using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computing
{
    class SimulateCalculate
    {
        public static Message EveryHourCalculate(Message m)
        {
            Message message = new Message();
            message.Hour = m.Hour;
            message.Temp = m.Temp + Math.Round(GetRandom(-1, 1), 9);
            message.Wind = m.Wind + Math.Round(GetRandom(-1, 1), 4);
            message.Visible = m.Visible + Math.Round(GetRandom(-0.5, 0.5), 4);
            message.Press = m.Press + Math.Round(GetRandom(-5, 5), 2);
            if (1 - m.Humi < 0.01)
            {
                message.Humi = m.Humi + Math.Round(GetRandom(-0.01, 0), 2);
            }
            else
            {
                message.Humi = m.Humi + Math.Round(GetRandom(0, 0.01), 2);
            }
            message.Apptemp = m.Apptemp + Math.Round(GetRandom(-1, 1), 9);
            if (360 - m.Windbear < 3)
            {
                message.Windbear = m.Windbear + (int)(GetRandom(-3, 0));
            }
            else
            {
                message.Windbear = m.Windbear + (int)GetRandom(0, 3);
            }
            return message;
        }

        private static double GetRandom(double min, double max)
        {
            Random random = new Random();
            return random.NextDouble() * (max - min) + min;
        }
    }
}
