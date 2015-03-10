using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Windows.Forms;


namespace WindRose
{
    class WindENoseAnalyze : Form
    {
        private ZedGraphControl zedGraphControl2;

        //ZedWindIndex windIndex;
        public WindENoseAnalyze(ZedGraphControl zedGraphControl2)
        {
            // TODO: Complete member initialization
            // windIndex = new ZedWindIndex(zedGraphControl2);
            this.zedGraphControl2 = zedGraphControl2;
            setupZedGraph();
        }

        private void setupZedGraph()
        {
            // First, clear out any old GraphPane's from the MasterPane collection
            MasterPane master = zedGraphControl2.MasterPane;
            master.PaneList.Clear();

            // Display the MasterPane Title, and set the outer margin to 10 points
            master.Title.IsVisible = true;
            master.Title.Text = "Enose - Wind Profile";
            master.Margin.All = 5;
            master.InnerPaneGap = 0;
            master.Fill = new Fill(Color.White, Color.MediumSlateBlue, 45.0F);


            // Create some GraphPane's (normally you would add some curves too
            GraphPane pane1 = new GraphPane(new Rectangle(10, 10, 10, 10),
                   "",
                   "Time, Days",
                   "Wind Index");
            GraphPane pane2 = new GraphPane(new Rectangle(10, 10, 10, 10),
                   "",
                   "Time, Days",
                   "Enose Data");

            // Add all the GraphPanes to the MasterPane
            master.Add(pane1);
            master.Add(pane2);

            // Refigure the axis ranges for the GraphPanes
            zedGraphControl2.AxisChange();

            pane1.Fill = new Fill(Color.White, Color.LightYellow, 45.0F);
            pane2.Fill = new Fill(Color.White, Color.LightYellow, 45.0F);


            //pane1.Fill.IsVisible = false;
            //pane1.Margin.All = 0;
            pane1.BaseDimension = 3F;
            pane1.YAxis.MinSpace = 80;
            pane1.Y2Axis.MinSpace = 18;
            pane1.Title.IsVisible = false;
            //// Hide the XAxis scale and title
            pane1.XAxis.Title.IsVisible = false;
            pane1.XAxis.Scale.IsVisible = true;
            //// Hide the legend, border, and GraphPane title
            //pane1.Legend.IsVisible = false;
            pane1.Border.IsVisible = false;
            //// Get rid of the tics that are outside the chart rect
            pane1.XAxis.MajorTic.IsOutside = false;
            pane1.XAxis.MinorTic.IsOutside = false;
            //// Show the X grids
            pane1.XAxis.MajorGrid.IsVisible = true;
            pane1.XAxis.MinorGrid.IsVisible = true;
            //// Remove all margins
            pane1.Margin.All = 0;
            pane1.Margin.Top = 10;
            pane1.Margin.Right = 0;

            pane1.XAxis.Type = AxisType.Date;
            pane1.XAxis.Title.Text = "Time (HH:MM)";
            pane1.XAxis.Scale.Format = "HH:mm\ndd/MM/yyyy";

            pane1.XAxis.Scale.FontSpec.FontColor = Color.LightYellow;

            pane1.Legend.Position = ZedGraph.LegendPos.InsideTopRight;

            pane1.YAxis.Scale.IsSkipLastLabel = true;

            pane2.BaseDimension = 6F;
            //// Hide the XAxis scale and title
            //pane2.XAxis.Title.IsVisible = false;
            //pane2.XAxis.Scale.IsVisible = false;
            //// Hide the legend, border, and GraphPane title
            //pane2.Legend.IsVisible = false;
            pane2.Border.IsVisible = false;
            //pane2.Title.IsVisible = false;

            pane2.Legend.Position = ZedGraph.LegendPos.InsideTopRight;
            //// Get rid of the tics that are outside the chart rect
            pane2.XAxis.MajorTic.IsOutside = false;
            pane2.XAxis.MinorTic.IsOutside = false;
            //// Show the X grids
            pane2.XAxis.MajorGrid.IsVisible = true;
            pane2.XAxis.MinorGrid.IsVisible = true;

            pane2.XAxis.Title.IsVisible = true;
            pane2.XAxis.Scale.IsVisible = true;

            //// Remove all margins
            pane2.Margin.All = 0;

            pane2.Margin.Bottom = 10;
            pane2.Margin.Right = 0;
            //pane2.Margin.Top = 5;
            pane2.XAxis.Type = AxisType.Date;
            pane2.XAxis.Title.Text = "Time";
            pane2.XAxis.Scale.Format = "HH:mm - dd/MM";
            //pane2.Margin.Right = 20;
            pane2.YAxis.MinSpace = 80;
            pane2.Y2Axis.MinSpace = 10;



            zedGraphControl2.IsEnableVZoom = false;
            zedGraphControl2.IsEnableVPan = false;

            using (Graphics g = this.CreateGraphics())
            {
                master.SetLayout(g, true, new int[] { 1, 1 }, new float[] { 1f, 2f });
                //master.SetLayout(g, PaneLayout.SingleColumn);
                master.AxisChange();


                //// Synchronize the Axes
                zedGraphControl2.IsAutoScrollRange = true;
                zedGraphControl2.IsShowHScrollBar = true;
                zedGraphControl2.IsSynchronizeXAxes = true;

                //g.Dispose();
            }

        }

        //http://stackoverflow.com/questions/26073673/zedgraph-multiple-panes
        public void GraphIt()
        {
            MasterPane master = zedGraphControl2.MasterPane;
            master.PaneList.Clear();

            // Display the MasterPane Title, and set the outer margin to 10 points
            master.Title.IsVisible = true;
            master.Title.Text = "Angles";
            master.Margin.All = 10;

            // Create some GraphPane's (normally you would add some curves too
            GraphPane pane1 = new GraphPane();
            GraphPane pane2 = new GraphPane();
            GraphPane pane3 = new GraphPane();

            // Add all the GraphPanes to the MasterPane
            master.Add(pane1);
            master.Add(pane2);
            master.Add(pane3);

            pane1.XAxis.Scale.MinorStep = pane2.XAxis.Scale.MinorStep = pane3.XAxis.Scale.MinorStep = 1;
            pane1.XAxis.Scale.MajorStep = pane2.XAxis.Scale.MajorStep = pane3.XAxis.Scale.MajorStep = 50;

            PointPairList dummylist = new PointPairList();
            myCurve1 = pane1.AddCurve("Angle X", dummylist, Color.Red);
            myCurve2 = pane2.AddCurve("Angle Y", dummylist, Color.Blue);
            myCurve3 = pane3.AddCurve("Angle Z", dummylist, Color.Green);
            myCurve1.Line.Width = myCurve2.Line.Width = myCurve3.Line.Width = 5;
            myCurve1.Symbol.Size = myCurve2.Symbol.Size = myCurve3.Symbol.Size = 0;

            // Refigure the axis ranges for the GraphPanes
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();

            // Layout the GraphPanes using a default Pane Layout
            //using (Graphics g = this.CreateGraphics())
            //{
            //    master.SetLayout(g, PaneLayout.SquareColPreferred);
            //}

        }




        public LineItem myCurve1 { get; set; }

        public LineItem myCurve2 { get; set; }

        public LineItem myCurve3 { get; set; }

        internal void graphit2()
        {
            MasterPane master = zedGraphControl2.MasterPane;
            GraphPane pane1 = master.PaneList[0];

            LineItem myCurve = pane1.AddCurve("Type ", WindIndex.pplNorth, Color.MediumBlue, SymbolType.None);
            pane1.AddCurve("Type ", WindIndex.pplEast, Color.Red, SymbolType.None);
            myCurve.Symbol.Fill = new Fill(Color.White);

            master.Add(pane1);




            using (Graphics g = this.CreateGraphics())
            {

                master.SetLayout(g, true, new int[] { 1, 1 }, new float[] { 1f, 2f });
                //master.SetLayout(g, PaneLayout.SingleColumn);
                master.AxisChange(g);

                // Synchronize the Axes
                zedGraphControl2.IsAutoScrollRange = true;
                zedGraphControl2.IsShowHScrollBar = true;
                zedGraphControl2.IsSynchronizeXAxes = true;

                //g.Dispose();
            }


            zedGraphControl2.Invalidate();
        }



        internal void syncPane()
        {
            MasterPane master = zedGraphControl2.MasterPane;

            // Fill the background
            master.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);
            // Clear out the initial GraphPane
            master.PaneList.Clear();

            // Show the masterpane title
            master.Title.IsVisible = true;
            master.Title.Text = "Synchronized Panes Demo";

            // Leave a margin around the masterpane, but only a small gap between panes
            master.Margin.All = 10;
            master.InnerPaneGap = 5;

            // The titles for the individual GraphPanes
            string[] yLabels = { "Rate, m/s", "Pressure, dynes/cm", "Count, units/hr" };

            ColorSymbolRotator rotator = new ColorSymbolRotator();

            for (int j = 0; j < 2; j++)
            {
                // Create a new graph -- dimensions to be set later by MasterPane Layout
                GraphPane myPaneT = new GraphPane(new Rectangle(10, 10, 10, 10),
                   "",
                   "Time, Days",
                   yLabels[j]);

                myPaneT.Fill.IsVisible = false;

                // Fill the Chart background
                myPaneT.Chart.Fill = new Fill(Color.White, Color.LightYellow, 45.0F);
                // Set the BaseDimension, so fonts are scaled a little bigger
                myPaneT.BaseDimension = 3.0F;

                // Hide the XAxis scale and title
                myPaneT.XAxis.Title.IsVisible = false;
                myPaneT.XAxis.Scale.IsVisible = false;
                // Hide the legend, border, and GraphPane title
                myPaneT.Legend.IsVisible = false;
                myPaneT.Border.IsVisible = false;
                //myPaneT.Title.IsVisible = false;
                // Get rid of the tics that are outside the chart rect
                myPaneT.XAxis.MajorTic.IsOutside = false;
                myPaneT.XAxis.MinorTic.IsOutside = false;
                // Show the X grids
                myPaneT.XAxis.MajorGrid.IsVisible = true;
                myPaneT.XAxis.MinorGrid.IsVisible = true;
                // Remove all margins
                myPaneT.Margin.All = 0;
                // Except, leave some top margin on the first GraphPane
                if (j == 0)
                    myPaneT.Margin.Top = 20;
                // And some bottom margin on the last GraphPane
                // Also, show the X title and scale on the last GraphPane only
                if (j == 1)
                {
                    myPaneT.XAxis.Title.IsVisible = true;
                    myPaneT.XAxis.Scale.IsVisible = true;
                    myPaneT.Margin.Bottom = 10;
                }

                if (j > 0)
                    myPaneT.YAxis.Scale.IsSkipLastLabel = true;

                // This sets the minimum amount of space for the left and right side, respectively
                // The reason for this is so that the ChartRect's all end up being the same size.
                myPaneT.YAxis.MinSpace = 80;
                myPaneT.Y2Axis.MinSpace = 20;

                // Make up some data arrays based on the Sine function
                double x, y;
                PointPairList list = new PointPairList();
                for (int i = 0; i < 36; i++)
                {
                    x = (double)i + 5 + j * 3;
                    y = (j + 1) * (j + 1) * 10 * (1.5 + Math.Sin((double)i * 0.2 + (double)j));
                    list.Add(x, y);
                }

                LineItem myCurve = myPaneT.AddCurve("Type " + j.ToString(),
                   list, rotator.NextColor, rotator.NextSymbol);
                myCurve.Symbol.Fill = new Fill(Color.White);

                master.Add(myPaneT);
            }

            using (Graphics g = this.CreateGraphics())
            {

                master.SetLayout(g, PaneLayout.SingleColumn);
                master.AxisChange(g);

                // Synchronize the Axes
                zedGraphControl2.IsAutoScrollRange = true;
                zedGraphControl2.IsShowHScrollBar = true;
                zedGraphControl2.IsSynchronizeXAxes = true;

                //g.Dispose();
            }
        }

        public ZedWindIndex WindIndex { get; set; }

        bool cbN, cbNE, cbE, cbSE, cbS, cbSW, cbW, cbNW;
        bool _tgs825, _tgs830, _tgs2600, _tgs2602, _tgs2610, _tgs4161;

        public void setCheck(string dir, bool checkState)
        {
            switch (dir)  // For wind direction
            {
                case "N":
                    cbN = checkState;
                    break;
                case "NE":
                    cbNE = checkState;
                    break;
                case "E":
                    cbE = checkState;
                    break;
                case "SE":
                    cbSE = checkState;
                    break;
                case "S":
                    cbS = checkState;
                    break;
                case "SW":
                    cbSW = checkState;
                    break;
                case "W":
                    cbW = checkState;
                    break;
                case "NW":
                    cbNW = checkState;
                    break;
            }

            switch (dir)  // For Sensor
            {
                case "TGS825":
                    _tgs825 = checkState;
                    break;
                case "TGS830":
                    _tgs830 = checkState;
                    break;
                case "TGS2600":
                    _tgs2600 = checkState;
                    break;
                case "TGS2602":
                    _tgs2602 = checkState;
                    break;
                case "TGS2610":
                    _tgs2610 = checkState;
                    break;
                case "TGS4161":
                    _tgs4161 = checkState;
                    break;
            }
        }
        public zedEnose _zedEnose { get; set; }

        internal void UpdateSensorChart(string p1, bool p2)
        {
            setCheck(p1, p2);
            MasterPane master = zedGraphControl2.MasterPane;
            GraphPane pane1 = master.PaneList[1];
            pane1.CurveList.Clear();

            //LineItem myCurve;
            if (_tgs825 == true)
            {
                LineItem line = pane1.AddCurve("TGS825", _zedEnose.pplTGS825, Color.Green, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (_tgs830 == true)
            {
                LineItem line = pane1.AddCurve("TGS830", _zedEnose.pplTGS830, Color.Brown, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (_tgs2600 == true)
            {
                LineItem line = pane1.AddCurve("TGS2600", _zedEnose.pplTGS2600, Color.Blue, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            if (_tgs2602 == true)
            {
                LineItem line = pane1.AddCurve("TGS2602", _zedEnose.pplTGS2602, Color.Orange, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (_tgs2610 == true)
            {
                LineItem line = pane1.AddCurve("TGS2610", _zedEnose.pplTGS2610, Color.Red, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (_tgs4161 == true)
            {
                LineItem line = pane1.AddCurve("TGS4161", _zedEnose.pplTGS4161, Color.Violet, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            // master.Add(pane1);

            using (Graphics g = this.CreateGraphics())
            {
                master.SetLayout(g, true, new int[] { 1, 1 }, new float[] { 1f, 2f });
                //master.SetLayout(g, PaneLayout.SingleColumn);
                master.AxisChange();

                // Synchronize the Axes
                zedGraphControl2.IsAutoScrollRange = true;
                zedGraphControl2.IsShowHScrollBar = true;
                zedGraphControl2.IsSynchronizeXAxes = true;

                //g.Dispose();
            }
            zedGraphControl2.Invalidate();

        }
        internal void UpdateWindEnoseChart(string p1, bool p2)
        {
            setCheck(p1, p2);

            MasterPane master = zedGraphControl2.MasterPane;
            GraphPane pane1 = master.PaneList[0];
            pane1.CurveList.Clear();

            //LineItem myCurve;
            if (cbN == true)
            {
                LineItem line = pane1.AddCurve("N", WindIndex.pplNorth, Color.Green, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (cbNE == true)
            {
                LineItem line = pane1.AddCurve("NE", WindIndex.pplNorthEast, Color.Brown, SymbolType.None);
                line.Line.Width = 2.0f;

            }

            if (cbE == true)
            {
                LineItem line = pane1.AddCurve("E", WindIndex.pplEast, Color.Blue, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            if (cbSE == true)
            {
                LineItem line = pane1.AddCurve("SE", WindIndex.pplSouthEast, Color.Orange, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (cbS == true)
            {
                LineItem line = pane1.AddCurve("S", WindIndex.pplSouth, Color.Red, SymbolType.None);
                line.Line.Width = 2.0f;
            }

            if (cbSW == true)
            {
                LineItem line = pane1.AddCurve("SW", WindIndex.pplSouthWest, Color.MediumBlue, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            if (cbW == true)
            {
                LineItem line = pane1.AddCurve("W", WindIndex.pplWest, Color.Violet, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            if (cbNW == true)
            {
                LineItem line = pane1.AddCurve("NW", WindIndex.pplNorthWest, Color.DarkSeaGreen, SymbolType.None);
                line.Line.Width = 2.0f;
            }
            // master.Add(pane1);





            using (Graphics g = this.CreateGraphics())
            {
                master.SetLayout(g, true, new int[] { 1, 1 }, new float[] { 1f, 2f });
                //master.SetLayout(g, PaneLayout.SingleColumn);
                master.AxisChange();

                // Synchronize the Axes
                zedGraphControl2.IsAutoScrollRange = true;
                zedGraphControl2.IsShowHScrollBar = true;
                zedGraphControl2.IsSynchronizeXAxes = true;

                //g.Dispose();
            }
            zedGraphControl2.Invalidate();
        }

        internal void graphit3()
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
                //// Generate a red curve 
                //LineItem myCurve = myPane.AddCurve("label" + j.ToString(), list, Color.Red, SymbolType.None);
                //// Add the new GraphPane to the MasterPane
                myMaster.Add(myPane);
            }



            // Tell ZedGraph to auto layout all the panes
            using (Graphics g = CreateGraphics())
            {
                //Setlayout is used here to show the second graph twice as large as  
                //the first one and to stack one chart over the other
                myMaster.SetLayout(g, true, new int[] { 1, 1 }, new float[] { 1f, 2f, });
                zedGraphControl2.AxisChange();
            }
        }



    }
}
