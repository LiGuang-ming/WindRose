using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace WindRose
{
    public class Info
    {
        string _filename;
        [CategoryAttribute("File Name"),
        DescriptionAttribute("Loaded File Name.")] 
        public string Filename
        {
            get
            {
                return Path.GetFileName( _filename);
            }
            set
            {
                _filename = value;
            }
        }

        double speed;
        string _curWindSpeed;
        [CategoryAttribute("Win speed sange"),
        DescriptionAttribute("wind speed")]
        public string WinSpeedGrade
        {
            get
            {
                return _curWindSpeed + " m/s";
                //switch (_curWindSpeed)
                //{ 
                //    case "0-1":
                //        speed = 1 * 3.600; 
                //    break;
                //    case "1-2":
                //    speed = 2 * 3.600;
                //    break;
                //    case "2-3":
                //    speed = 3 * 3.600;
                //    break;
                //    case "3-4":
                //    speed = 4 * 3.600;
                //    break;
                //    case "4-5":
                //    speed = 5 * 3.600;
                //    break;
                //}
                //return speed  + " km/Hr";
            }
            set
            {
                _curWindSpeed= value;
            }
        }

    
    }
}
