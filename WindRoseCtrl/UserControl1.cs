using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindRoseCtrl
{
    public partial class UserControl1 : UserControl
    {
        public List<WindData> windata;

        public RoseDistribute roseDist = new RoseDistribute();

        public List<PolarChartData> polarRose = new List<PolarChartData>();
        public int windGrade;
        public List<Color> seriesColor = new List<Color>();

        public UserControl1()
        {
            InitializeComponent();
            windata = new List<WindData>();
        }


        private void UserControl1_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("Wind Rose Control loaded.");
            //windata.ForEach(delegate(WindData w)
            //{
            //    roseDist.Add(w.WindSpeed, w.WindDir);
            //    //if(w.WindSpeed >0 && w.WindSpeed <1)

            //    //Console.WriteLine(w.TimeStamp+ "\t" + w.Temp+ "\t" + w.WindDir+ "\t" + w.WindSpeed);
            //});
        }

        public void ShowInfo()
        {
            roseDist.Clear();
            windata.ForEach(delegate(WindData w)
            {
                roseDist.Add(w.WindSpeed, w.WindDir);
                //if(w.WindSpeed >0 && w.WindSpeed <1)

                //Console.WriteLine(w.TimeStamp+ "\t" + w.Temp+ "\t" + w.WindDir+ "\t" + w.WindSpeed);
            });
            roseDist.showDist();

            FillChart();

            chart1.Invalidate();


        }
        public void UpdateChart()
        {
            FillChart();
            chart1.Invalidate();
        }
        private void FillChart()
        {

            const double x = 360.0 / 8.0;
            List<PolarPoint> items = new List<PolarPoint>();
            List<PolarMultiPoint> multiItems = new List<PolarMultiPoint>();

            items.Clear();
            multiItems.Clear();

            for (int i = 0; i < 9; i++)
            {
                multiItems.Add(new PolarMultiPoint(x * (i % 8), 
                    roseDist.wndList[(i % 8)].wind1, 
                    roseDist.wndList[(i % 8)].wind2,
                    roseDist.wndList[(i % 8)].wind3,
                    roseDist.wndList[(i % 8)].wind4,
                    roseDist.wndList[(i % 8)].wind5
                    
                    ));

                switch (windGrade)
                { 
                    case 1:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind1));
                        break;
                    case 2:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind2));
                        break;
                    case 3:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind3));
                        break;
                    case 4:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind4));
                        break;
                    case 5:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind5));
                        break;
                    case 6:
                        items.Add(new PolarPoint(x * (i % 8), roseDist.wndList[(i % 8)].wind6));
                        break;

                    default:
                        items.Add(new PolarPoint(x * (i % 8), 0));
                        break;
        
                }
            }
            this.chart1.Series.Clear();
            var seriesLines1 = this.chart1.Series.Add("1m-s");
            seriesLines1.XValueMember = "Angle";
            seriesLines1.YValueMembers = "Y1";
            seriesLines1.ChartType = SeriesChartType.Polar;

            var seriesLines2 = this.chart1.Series.Add("2m-s");
            seriesLines2.XValueMember = "Angle";
            seriesLines2.YValueMembers = "Y2";
            seriesLines2.ChartType = SeriesChartType.Polar;

            var seriesLines3 = this.chart1.Series.Add("3m-s");
            seriesLines3.XValueMember = "Angle";
            seriesLines3.YValueMembers = "Y3";
            seriesLines3.ChartType = SeriesChartType.Polar;

            var seriesLines4 = this.chart1.Series.Add("4m-s");
            seriesLines4.XValueMember = "Angle";
            seriesLines4.YValueMembers = "Y4";
            seriesLines4.ChartType = SeriesChartType.Polar;

            var seriesLines5 = this.chart1.Series.Add("5m-s");
            seriesLines5.XValueMember = "Angle";
            seriesLines5.YValueMembers = "Y5";
            seriesLines5.ChartType = SeriesChartType.Polar;


            this.chart1.ChartAreas[0].AxisY.LineColor = Color.Black;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightBlue;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightBlue;


            for (int i = 0; i < 5; i++)
            {

                this.chart1.Series[i].Color = seriesColor[i];
                this.chart1.Series[i].BorderWidth = 3;

            }

      
            this.chart1.DataSource = multiItems;
        }


    }

    internal class PolarPoint
    {
        public double Angle { get; private set; }
        public double Y { get; private set; }

        public PolarPoint(double angle, double y)
        {
            this.Angle = angle;
            this.Y = y;
        }
    }

    internal class PolarMultiPoint
    {
        public double Angle { get; private set; }
        public double Y1 { get; private set; }
        public double Y2 { get; private set; }
        public double Y3 { get; private set; }
        public double Y4 { get; private set; }
        public double Y5 { get; private set; }


        public PolarMultiPoint(double angle, double y1, double y2, double y3, double y4, double y5)
        {
            this.Angle = angle;
            this.Y1 = y1;
            this.Y2 = y2;
            this.Y3 = y3;
            this.Y4 = y4;
            this.Y5 = y5;
        }
    }



}
