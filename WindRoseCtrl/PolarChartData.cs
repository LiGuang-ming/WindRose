using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindRoseCtrl
{
    public class PolarChartData
    {
        double _degree;
        double _value;

        [System.ComponentModel.DisplayName("Degree")]
        public double degree
        {
            get
            {
                return this._degree;
            }
            set
            {
                this._degree = value;
            }
        }

        [System.ComponentModel.DisplayName("Value")]
        public double value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}
