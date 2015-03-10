using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindRoseCtrl
{
    public class WindData
    {
        DateTime timeStamp;
        double speed;
        double temp;
        double dir;

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public Double WindSpeed
        {
            get
            { return speed; }
            set
            { speed = value; }
        }

        public double WindDir
        {
            get { return dir; }
            set { dir = value; }
        }

        public double Temp
        {
            get { return temp;  }
            set { temp = value; }
        }

        public WindData(DateTime time, double temp, double dir, double speed)
        {
            this.TimeStamp = time;
            this.Temp = temp;
            this.WindDir = dir;
            this.WindSpeed = speed;
        }
    }
}
