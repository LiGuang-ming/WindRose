using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindRoseCtrl
{
    public class RoseDistribute
    {
        int[,] roseDist = new int[6, 16];
        //public string[] dirname = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        public string[] dirname = { "N",  "NE",  "E",   "SE",   "S",   "SW",   "W",   "NW",   };

        double[] _avgSpeed;
        double[] _dirEvent;
        double[] _speedEvent;
        double[] _sumSpeedEvent;

        public List<windDistList> wndList = new List<windDistList>();

        public double sumSpeedEvent(int speedGrade)
        {
            int sumEvent = 0;
            for (int j = 0; j < 8; j++)
            {
                sumEvent += roseDist[speedGrade, j];
            }
            return sumEvent;
        }

        
        public void Clear()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    roseDist[i, j] = 0;
                }
            }

        }
        public RoseDistribute()
        {
            _avgSpeed = new double[16];
            _dirEvent = new double[16];
            _speedEvent = new double[16];
            _sumSpeedEvent = new double[6];
        }


        public double[] AvgSpeed
        {
            get
            {
                _avgSpeed[0] = getAveSpeed(0);
                return _avgSpeed;
            }
        }

        private double getAveSpeed(int p)
        {
            double sum = 0.0;
            double count;
            sum += roseDist[0, p];
            sum += roseDist[1, p] * 2.0;
            sum += roseDist[2, p] * 3.0;
            sum += roseDist[3, p] * 4.0;
            sum += roseDist[4, p] * 5.0;
            sum += roseDist[5, p] * 6.0;

            if ((count = dirEvent(0)) != 0.0)
                return sum / count;
            else
                return 0.0;
        }

        private double dirEvent(int p)
        {
            double sum = 0.0;
            sum += roseDist[0, p];
            sum += roseDist[1, p];
            sum += roseDist[2, p];
            sum += roseDist[3, p];
            sum += roseDist[4, p];
            sum += roseDist[5, p];
            return sum;
        }
        public double dirEvent(int speedGrade, int dir)
        {
            return roseDist[speedGrade, dir];
        }

        public void Add(double speed, double dir)
        {
            for (int index = 0; index < 8; index++)
            {
                if (CkeckSpeed(speed) == 0 && CheckDir(dir) == dirname[index]) roseDist[0, index]++;
                if (CkeckSpeed(speed) == 1 && CheckDir(dir) == dirname[index]) roseDist[1, index]++;
                if (CkeckSpeed(speed) == 2 && CheckDir(dir) == dirname[index]) roseDist[2, index]++;
                if (CkeckSpeed(speed) == 3 && CheckDir(dir) == dirname[index]) roseDist[3, index]++;
                if (CkeckSpeed(speed) == 4 && CheckDir(dir) == dirname[index]) roseDist[4, index]++;
                if (CkeckSpeed(speed) == 5 && CheckDir(dir) == dirname[index]) roseDist[5, index]++;
            }
        }

        public void showDist()
        {
            wndList.Clear();
            for (int index = 0; index < 8; index++)
            {
                //Console.WriteLine(dirname[index] + "= " + roseDist[0, index] +
                //    "    " + roseDist[1, index] + "    " + roseDist[2, index] + "    " + roseDist[3, index]
                //    + "    " + roseDist[4, index] + "    " + getAveSpeed(index));


                wndList.Add(new windDistList()
                {
                    wind1 = roseDist[0, index],
                    wind2 = roseDist[1, index],
                    wind3 = roseDist[2, index],
                    wind4 = roseDist[3, index],
                    wind5 = roseDist[4, index],
                    wind6 = roseDist[5, index]
                });

            }
            //Console.WriteLine(getAveSpeed(0));


        }

        public int CkeckSpeed(double speed)
        {
            if (speed > 0.0 && speed < 1.0)
                return 0;
            if (speed > 1.0 && speed < 2.0)
                return 1;
            if (speed > 2.0 && speed < 3.0)
                return 2;
            if (speed > 3.0 && speed < 4.0)
                return 3;
            if (speed > 4.0 && speed < 5.0)
                return 4;
            if (speed > 5.0)
                return 5;

            else
                return -1;
        }

        //public string CheckDir(double dir)
        //{
        //    double DirStart = 11.25;
        //    const double DirStep = 22.5;
        //    string dir_name = "X";
        //    if ((dir > 348.75 && dir < 360.0) || (dir > 0.0 && dir < 11.25))
        //    {
        //        dir_name = "N";
        //        _dirEvent[0]++;
        //    }
        //    for (int index = 1; index < 16; index++)
        //        if (dir > DirStart && dir < (DirStart += DirStep))
        //        {
        //            dir_name = dirname[index];
        //            _dirEvent[index]++;
        //        }
        //    return dir_name;
        //}

        public string CheckDir(double dir)
        {
            double DirStart = 22.5;
            const double DirStep = 45;
            string dir_name = "X";
            if ((dir > (360-22.5)  && dir < 360.0) || (dir > 0.0 && dir < 22.5))
            {
                dir_name = "N";
                _dirEvent[0]++;
            }
            for (int index = 1; index < 8; index++)
                if (dir > DirStart && dir < (DirStart += DirStep))
                {
                    dir_name = dirname[index];
                    _dirEvent[index]++;
                }
            return dir_name;
        }
    
    }
}
