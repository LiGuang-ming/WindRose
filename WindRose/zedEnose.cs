using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WindRose
{
    class zedEnose : Form
    {
        PointPairList tgs825 = new PointPairList();
        PointPairList tgs830 = new PointPairList();
        PointPairList tgs2600 = new PointPairList();
        PointPairList tgs2602 = new PointPairList();
        PointPairList tgs2610 = new PointPairList();
        PointPairList tgs4161 = new PointPairList();
        private ZedGraphControl zedGraphControl2;
        private DataGridView dgvAnalysisSource;

        public zedEnose(ZedGraphControl zedGraphControl2)
        {
            // TODO: Complete member initialization
            this.zedGraphControl2 = zedGraphControl2;
            setupZedGraph();
            
            //testgraph();
        }

        private void FillCurve()
        {
            for (int i = 0; i < dgvAnalysisSource.Rows.Count - 1; i++)
            {
                string dateString = dgvAnalysisSource.Rows[i].Cells["DateTime"].Value.ToString();
                DateTime DT = DateTime.Parse(dateString, CultureInfo.CurrentCulture);
                XDate xdt = new XDate(DT);

                double Tgs825 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS825"].Value);
                pplTGS825.Add(xdt, Tgs825);
                
                double Tgs830 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS830"].Value);
                pplTGS830.Add(xdt, Tgs830);

                double Tgs2600 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS2600"].Value);
                pplTGS2600.Add(xdt, Tgs2600);

                double Tgs2602 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS2602"].Value);
                pplTGS2602.Add(xdt,  Tgs2602);
                
                double Tgs2610 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS2610"].Value);
                pplTGS2610.Add(xdt, Tgs2610);
                
                double Tgs4161 = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["TGS4161"].Value);
                pplTGS4161.Add(xdt, Tgs4161);
            }  
        }

        public PointPairList pplTGS825
        {
            get { return tgs825; }
            set { tgs825 = value; }
        }

        public PointPairList pplTGS830
        {
            get { return tgs830; }
            set { tgs830 = value; }
        }

        public PointPairList pplTGS2600
        {
            get { return tgs2600; }
            set { tgs2600 = value; }
        }

        public PointPairList pplTGS2602
        {
            get { return tgs2602; }
            set { tgs2602 = value; }
        }

        public PointPairList pplTGS2610
        {
            get { return tgs2610; }
            set { tgs2610 = value; }
        }

        public PointPairList pplTGS4161
        {
            get { return tgs4161; }
            set { tgs4161 = value; }
        }

        private void testgraph()
        {
            MasterPane myMaster = zedGraphControl2.MasterPane;
            myMaster.PaneList.Clear();
            myMaster.Title.IsVisible = true;
            // Fill the pane background with a color gradient
            myMaster.Fill = new Fill(Color.White, Color.MediumSlateBlue, 45.0F);
            // Set the margins and the space between panes 
            myMaster.Margin.All = 0;
            myMaster.InnerPaneGap = 0;
            for (int j = 0; j < 2; j++)
            {
                // Create a new GraphPane
                GraphPane myPane = new GraphPane();
                Graphics g = CreateGraphics();
                myPane.XAxis.MajorGrid.IsVisible = true;
                myPane.YAxis.MajorGrid.IsVisible = true;
                // Fill the pane background with a color gradient
                myPane.Fill = new Fill(Color.White, Color.LightYellow, 45.0F);
                if (j == 0)//alingn Y axes of both charts
                {
                    //Change the base dimension to compensate for SetLayout()below
                    myPane.BaseDimension = 6F;
                    myPane.XAxis.Scale.IsVisible = true;

                }
                if (j == 1)//alingn Y axes of both charts
                {
                    //Change the base to twice as above because 
                    //SetLayout()below doubles the  size of the second graph
                    myPane.BaseDimension = 12F;
                }
                // Make up some data arrays based on the Sine function
                //PointPairList list = new PointPairList();
                //for (int i = 0; i < 50; i++)
                //{
                //    double x = (double)i + 5;
                //    double y = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                //    list.Add(x, y);
                //}
                // Generate a red curve 
                LineItem myCurve = myPane.AddCurve("label", pplTGS825, Color.Red, SymbolType.None);
                myPane.AddCurve("label", pplTGS830, Color.Green, SymbolType.None);
                // Add the new GraphPane to the MasterPane
                myMaster.Add(myPane);

            }
        }

        private void setupZedGraph()
        {
            MasterPane master = zedGraphControl2.MasterPane;
            GraphPane pane1 = master.PaneList[0];
        }


        internal void AddDataSource(DataGridView dgvAnalysisSource)
        {
            this.dgvAnalysisSource = dgvAnalysisSource;
            //datasrc = true;
        }

        internal void UpdateGraph()
        {
            if (dgvAnalysisSource == null)
                return;
            if (dgvAnalysisSource.Rows.Count == 0)
                return;
            ClearPpl();
            FillCurve();




        }

        private void ClearPpl()
        {
            pplTGS825.Clear();
            pplTGS830.Clear();
            pplTGS2600.Clear();
            pplTGS2602.Clear();
            pplTGS2610.Clear();
            pplTGS4161.Clear();

        }
    }
}
