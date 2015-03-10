using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindRose
{
    class WindIndex
    {
        List<double> wIndex = new List<double>();
        int wDir;
        List<DateTimePoint> wDateTimePoint = new List<DateTimePoint>(); // input 
        List<DateTimePoint> indexNorth = new List<DateTimePoint>();     // output for North

        public int WindDirection
        {
            get { return wDir; }
            set { wDir = value; }
        }

        void CalculateWindIndex()
        { 
            // formula
            // out = cos(2*pi()*North-WindDirection)
        
        
        
        }

    }

    internal class DateTimePoint
    {
        DateTime dt;
        double Y;

        public DateTime Datetime
        {
            get { return dt; }
            set { dt = value; }
        }

        public double level
        {
            get { return Y; }
            set { Y = value; }
        }
        

    }
}
