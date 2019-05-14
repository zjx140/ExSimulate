using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computing
{
    class TimeCalculate
    {
        public static string StopTimeCalculate(string catandper,int i,int endhour,int ratio)//显示停堆预测时间
        {
            Random random = new Random();
            DateTime datetime = new DateTime(2006, 4, 1, 0, 0, 0);
            TimeSpan span;
            string result = null;
            string cat = (catandper.Split('|'))[0];
            double percent = double.Parse((catandper.Split('|'))[1]);
            if(cat.Equals("温度") || cat.Equals("湿度"))
            {
                if (percent >= 0 && percent <= 0.1)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(3, 6), 0, 0);
                }
                else if (percent > 0.1 && percent <= 0.3)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(5, 8), 0, 0);
                }
                else if (percent > 0.3 && percent <= 0.5)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(8, 15), 0, 0);
                }
                else
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(15, 49), 0, 0);
                }
            }
            else if(cat.Equals("风速"))
            {
                if(percent >= 0 && percent <= 0.1)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(1, 4), 0, 0);
                }
                else if(percent > 0.1 && percent <= 0.3)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(3, 6), 0, 0);
                }
                else if(percent > 0.3 && percent <= 0.5)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(6, 10), 0, 0);
                }
                else
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(10, 20), 0, 0);
                }
            }
            else
            {
                if(percent >= 0 && percent <= 0.1)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(2, 4), 0, 0);
                }
                else if(percent > 0.1 && percent <= 0.3)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(3, 8), 0, 0);
                }
                else if(percent > 0.3 && percent <= 0.5)
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(8, 16), 0, 0);
                }
                else
                {
                    span = new TimeSpan(i + endhour * ratio + random.Next(16, 31), 0, 0);
                }
            }
            DateTime newdate = datetime + span;
            result = newdate.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
    }
}
