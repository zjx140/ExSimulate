using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpSendTest
{
    class MakeSure
    {
        public static bool IsNormal(Message m,double temp_minnum,double temp_maxnum,double wind_maxnum,double humi_minnum,double humi_maxnum,double visible_minnum)
        {
            return m.Temp >= temp_minnum && m.Temp <= temp_maxnum && m.Wind >= 0 && m.Wind <= wind_maxnum && m.Humi >= humi_minnum && m.Humi <= humi_maxnum && m.Visible >= visible_minnum;
        }

        private static bool IsTempNormal(double temp, double temp_minnum,double temp_maxnum)
        {
            return temp >= temp_minnum && temp <= temp_maxnum;
        }

        private static bool IsWindNormal(double wind,double wind_maxnum)
        {
            return wind >= 0 && wind <= wind_maxnum;
        }

        private static bool IsHumiNormal(double humi,double humi_minnum,double humi_maxnum)
        {
            return humi >= humi_minnum && humi <= humi_maxnum;
        }

        private static bool IsVisibleNormal(double visible,double visible_minnum)
        {
            return visible >= visible_minnum;
        }

        public static string EnsureQues(Message message, double temp_minnum, double temp_maxnum, double wind_maxnum, double humi_minnum, double humi_maxnum, double visible_minnum)
        {
            string category = null;
            double percent = 0;
            if (!IsTempNormal(message.Temp, temp_minnum, temp_maxnum))
            {
                category = "温度";
                if (message.Temp > temp_maxnum)
                    percent = CaculatePercent(message.Temp, temp_maxnum);
                else
                    percent = CaculatePercent(message.Temp, temp_minnum);
            }
            else if (!IsVisibleNormal(message.Visible, visible_minnum))
            {
                category = "可见度";
                percent = CaculatePercent(message.Visible, visible_minnum);
            }
            else if (!IsWindNormal(message.Wind,wind_maxnum))
            {
                category = "风速";
                percent = CaculatePercent(message.Wind, wind_maxnum);
            }
            else
            {
                category = "湿度";
                if (message.Humi > humi_maxnum)
                    percent = CaculatePercent(message.Humi, humi_maxnum);
                else
                    percent = CaculatePercent(message.Humi, humi_minnum);
            }
            return category + "|" + percent;
        }

        private static double CaculatePercent(double temp, double tempnum)
        {
            return Math.Abs(temp - tempnum) / tempnum;
        }
    }
}
