using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindRoseCtrl
{
    public class windDistList
    {
        double _wind1;
        double _wind2;
        double _wind3;
        double _wind4;
        double _wind5;
        double _wind6;
        int retVal;
        public void retType(int type)
        {
            retVal = type;
        }

        [System.ComponentModel.DisplayName("0.0 - 1.0\n(m/s)")]
        public double wind1
        {
            get { return _wind1; }
            set { _wind1 = value; }
        }
        [System.ComponentModel.DisplayName("1.0 - 2.0 (m/s)")]
        public double wind2
        {
            get { return _wind2; }
            set { _wind2 = value; }
        }
        [System.ComponentModel.DisplayName("2.0 - 3.0 (m/s)")]
        public double wind3
        {
            get { return _wind3; }
            set { _wind3 = value; }
        }
        [System.ComponentModel.DisplayName("3.0 - 4.0 (m/s)")]
        public double wind4
        {
            get { return _wind4; }
            set { _wind4 = value; }
        }
        [System.ComponentModel.DisplayName("4.0 - 5.0 (m/s)")]
        public double wind5
        {
            get { return _wind5; }
            set { _wind5 = value; }
        }

        [System.ComponentModel.DisplayName("> 5.0 (m/s)")]
        public double wind6
        {
            get { return _wind6; }
            set { _wind6 = value; }
        }

        [System.ComponentModel.DisplayName("sum")]
        public double sum
        {
            get { return _wind1 + _wind2 + _wind3 + _wind4 + _wind5 + _wind6; }
            //get {
            //    switch (retVal)
            //    {
            //        case 1:
            //            return _wind1;
            //        case 2:
            //            return _wind2;
            //        case 3:
            //            return _wind3;
            //        case 4:
            //            return _wind4;
            //        case 5:
            //            return _wind5;
            //        default :
            //            return 0;
            //    }
            
            //}

            //set { _wind5 = value; }
        }
    }
}
