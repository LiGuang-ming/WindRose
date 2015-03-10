using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using ZedGraph;


namespace WindRose
{
    public partial class Form1 : Form
    {

        GMapMarker currentMarker;
        List<GMapMarker> mapmarker = new List<GMapMarker>();
        readonly GMapOverlay top = new GMapOverlay();
        readonly GMapOverlay stations = new GMapOverlay();

        List<Color> seriesColor = new List<Color>();
        string OpenedFilename = "";
        ZedWindIndex zedWindIndex;
        WindENoseAnalyze weAnalyze;
        zedEnose zEnose;


        Info info = new Info();
        public Form1()
        {
            InitializeComponent();
            zedWindIndex = new ZedWindIndex(zedGraphControl1);
            weAnalyze = new WindENoseAnalyze(zedGraphControl2);
            zEnose = new zedEnose(zedGraphControl2);

            InitialMap();
            //propertyGrid1.SelectedObject = info;
            initialColor();
            AnalyzeWindEnose();
        }

        private void initialColor()
        {
            seriesColor.Add(Color.DarkGreen);
            seriesColor.Add(Color.YellowGreen);
            seriesColor.Add(Color.LawnGreen);
            seriesColor.Add(Color.Orange);
            seriesColor.Add(Color.Red);
            seriesColor.Add(Color.DarkRed);

            windRoseCtrl1.seriesColor.Add(seriesColor[0]);
            windRoseCtrl1.seriesColor.Add(seriesColor[1]);
            windRoseCtrl1.seriesColor.Add(seriesColor[2]);
            windRoseCtrl1.seriesColor.Add(seriesColor[3]);
            windRoseCtrl1.seriesColor.Add(seriesColor[4]);
            windRoseCtrl1.seriesColor.Add(seriesColor[5]);
        }

        private void InitialMap()
        {
            mapmarker.Add(new GMarkerGoogle(new PointLatLng(12.666559, 101.316387), GMarkerGoogleType.blue));
            mapmarker.Add(new GMarkerGoogle(new PointLatLng(12.699527, 101.333742), GMarkerGoogleType.blue));
            mapmarker.Add(new GMarkerGoogle(new PointLatLng(12.686432, 101.341347), GMarkerGoogleType.blue));
            mapmarker.Add(new GMarkerGoogle(new PointLatLng(12.685096, 101.300576), GMarkerGoogleType.blue));
            mapmarker.Add(new GMarkerGoogle(new PointLatLng(12.660450, 101.314343), GMarkerGoogleType.blue));
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "xls files (*.xls*)|*.xls*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                 
                string extension = Path.GetExtension(filename);
                Console.WriteLine(filename);
                Console.WriteLine(extension);
                info.Filename = filename;

                if (extension == ".xls" || extension == ".xlsx")
                {
                    Accord.IO.ExcelReader db = new Accord.IO.ExcelReader(filename, true, false);
                    TableSelectDialog t = new TableSelectDialog(db.GetWorksheetList());

                    if (t.ShowDialog(this) == DialogResult.OK)
                    {
                        this.dgvAnalysisSource.DataSource = db.GetWorksheet(t.Selection);
                        //this.dgvProjectionSource.DataSource = db.GetWorksheet(t.Selection);
                    }
                }
                else if (extension == ".xml")
                {
                    DataTable dataTableAnalysisSource = new DataTable();
                    dataTableAnalysisSource.ReadXml(openFileDialog.FileName);

                    this.dgvAnalysisSource.DataSource = dataTableAnalysisSource;
                    //this.dgvProjectionSource.DataSource = dataTableAnalysisSource.Clone();
                }
                button1_Click(null, null);
                 button2_Click(null, null);
                button6_Click(null, null);
                zedGraphControl2.MasterPane.Title.Text = "Enose - Wind Profile [" + System.IO.Path.GetFileNameWithoutExtension(filename) + "]";

                zedGraphControl1.GraphPane.Title.Text = "Wind Index [" + System.IO.Path.GetFileNameWithoutExtension(filename) + "]";
            
            }
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<weatherData> wd = new List<weatherData>();
            CultureInfo provider = CultureInfo.CurrentCulture;

            string format;
            //provider = new CultureInfo("en-US");
            format = "dd/MM/yyyy hh:mm";
            try
            {
                wd = (from row in dgvAnalysisSource.Rows.OfType<DataGridViewRow>()
                      select new weatherData()
                      {
                          date = Convert.ToDateTime(row.Cells["DateTime"].Value),
                          //date = DateTime.TryParseExact(row.Cells["Date-Time"].Value.ToString(), format, provider),
                          Speed = Convert.ToDouble(row.Cells["WS"].Value),
                          Direction = Convert.ToDouble(row.Cells["WD"].Value),
                          Temperature = Convert.ToDouble(row.Cells["Temp"].Value)
                      }).ToList();
            }
            catch (InvalidCastException err)
            {
                Console.WriteLine(err.Message);
            }

            windRoseCtrl1.windata.Clear();
            wd.ForEach(delegate(weatherData w)
            {
                //Console.WriteLine(w.date + "\t" + w.Speed + "\t" + w.Direction + "\t" + w.Temperature);

                windRoseCtrl1.windata.Add(new WindRoseCtrl.WindData(w.date, w.Temperature, w.Direction, w.Speed));
                windRoseCtrl1.Invalidate();
            });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            windRoseCtrl1.ShowInfo();

            //windRoseCtrl1.windGrade = comboBox1.SelectedIndex + 1;
            //info.WinSpeedGrade = comboBox1.SelectedItem.ToString();
            this.dgvWindRose.DataSource = windRoseCtrl1.roseDist.wndList;
            for (int i = 0; i < windRoseCtrl1.roseDist.wndList.Count; i++)
            {
                this.dgvWindRose.Rows[i].HeaderCell.Value = windRoseCtrl1.roseDist.dirname[i];
                this.dgvWindRose.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            }
            //windRoseCtrl1.ShowInfo();
            windRoseCtrl1.UpdateChart();
            this.dgvWindRose.Invalidate();
            UpdateSpeedHistogram();
            UpdateDirectionHistogram();
            updatePropertyGrid1();
        }

        private void updatePropertyGrid1()
        {
            //propertyGrid1.SelectedObject = info;
            //propertyGrid1.Invalidate();
            //propertyGrid1.Invalidate();
        }

        private void UpdateDirectionHistogram()
        {
            //string[] seriesArray = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            string[] seriesArray = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            int[] pointsArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Set palette.
            this.DirectionHistogram.Palette = ChartColorPalette.Excel;

            this.DirectionHistogram.Series.Clear();
            this.DirectionHistogram.ChartAreas[0].AxisX.Interval = 1;

            // Set title.
            this.DirectionHistogram.Titles.Clear();
            this.DirectionHistogram.Titles.Add("Wind Direction Distribution");
            Series series = this.DirectionHistogram.Series.Add(seriesArray[0]);
            Series series1 = this.DirectionHistogram.Series.Add(seriesArray[1]);
            Series series2 = this.DirectionHistogram.Series.Add(seriesArray[2]);
            Series series3 = this.DirectionHistogram.Series.Add(seriesArray[3]);
            Series series4 = this.DirectionHistogram.Series.Add(seriesArray[4]);
            Series series5 = this.DirectionHistogram.Series.Add(seriesArray[5]);

            series.ChartType = series1.ChartType =
                series2.ChartType = series3.ChartType =
                series4.ChartType = series5.ChartType = SeriesChartType.StackedColumn;

            for (int i = 0; i < 8; i++)
            {
                series.Points.Add(windRoseCtrl1.roseDist.dirEvent(0, i));
                series1.Points.Add(windRoseCtrl1.roseDist.dirEvent(1, i));
                series2.Points.Add(windRoseCtrl1.roseDist.dirEvent(2, i));
                series3.Points.Add(windRoseCtrl1.roseDist.dirEvent(3, i));
                series4.Points.Add(windRoseCtrl1.roseDist.dirEvent(4, i));
                series5.Points.Add(windRoseCtrl1.roseDist.dirEvent(5, i));

            }

            // define column label
            for (int i = 0; i < series.Points.Count; i++)
            {
                this.DirectionHistogram.Series[0].Points[i].AxisLabel = seriesArray[i];
            }
            this.DirectionHistogram.Legends[0].Enabled = false;
            this.DirectionHistogram.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.DirectionHistogram.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.DirectionHistogram.ChartAreas[0].RecalculateAxesScale();
            this.DirectionHistogram.Series[0].IsValueShownAsLabel = false;
            for (int i = 0; i < 6; i++)
            {
                this.DirectionHistogram.Series[i].Color = seriesColor[i];
            }
        }

        private void UpdateSpeedHistogram()
        {
            string[] seriesArray = { "0-1 m/s", "1-2 m/s", "2-3 m/s", "3-4 m/s", "4-5 m/s", ">5 m/s" };
            int[] pointsArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Set palette.
            this.SpeedHistogram.Palette = ChartColorPalette.Excel;
            this.SpeedHistogram.Series.Clear();
            ;
            // Set title.
            this.SpeedHistogram.Titles.Clear();
            this.SpeedHistogram.Titles.Add("Wind Speed Distribution");
            Series series = this.SpeedHistogram.Series.Add(seriesArray[0]);

            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(0));
            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(1));
            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(2));
            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(3));
            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(4));
            series.Points.Add(windRoseCtrl1.roseDist.sumSpeedEvent(5));
            for (int i = 0; i < 6; i++) // 6 points
            {
                this.SpeedHistogram.Series[0].Points[i].AxisLabel = seriesArray[i];
            }
            this.SpeedHistogram.Legends[0].Enabled = false;
            this.SpeedHistogram.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.SpeedHistogram.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.SpeedHistogram.ChartAreas[0].RecalculateAxesScale();
            this.SpeedHistogram.Series[0].IsValueShownAsLabel = true;
            for (int i = 0; i < 6; i++) // set color
            {
                this.SpeedHistogram.Series[0].Points[i].Color = seriesColor[i];
            }

        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    info.WinSpeedGrade = comboBox1.SelectedItem.ToString();

        //    button2_Click(null, null);
        //}



        //private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (tabControl1.SelectedTab == tabControl1.TabPages["Wind Rose"])
        //        button2_Click(null, null);
        //}


        private void gMapControl1_Load(object sender, EventArgs e)
        {
            MainMap.MapProvider = GMapProviders.GoogleMap;
            MainMap.Position = new PointLatLng(12.699527, 101.333742);
            MainMap.MinZoom = 0;
            MainMap.MaxZoom = 24;
            MainMap.Zoom = 13;
            MainMap.Overlays.Add(stations);


            currentMarker = new GMarkerGoogle(MainMap.Position, GMarkerGoogleType.blue);
            currentMarker.ToolTip = new GMapToolTip(currentMarker);
            currentMarker.ToolTipText = "บ้านแลง";
            currentMarker.ToolTipMode = MarkerTooltipMode.Always;

            mapmarker[0].ToolTip = new GMapToolTip(mapmarker[0]);
            mapmarker[0].ToolTipText = "Techno IRPC";
            mapmarker[0].ToolTipMode = MarkerTooltipMode.Always;

            mapmarker[1].ToolTip = new GMapToolTip(mapmarker[1]);
            mapmarker[1].ToolTipText = "Ban Lang";
            mapmarker[1].ToolTipMode = MarkerTooltipMode.Always;

            mapmarker[2].ToolTip = new GMapToolTip(mapmarker[2]);
            mapmarker[2].ToolTipText = "Kon Nong";
            mapmarker[2].ToolTipMode = MarkerTooltipMode.Always;

            mapmarker[3].ToolTip = new GMapToolTip(mapmarker[3]);
            mapmarker[3].ToolTipText = "Housing";
            mapmarker[3].ToolTipMode = MarkerTooltipMode.Always;

            mapmarker[4].ToolTip = new GMapToolTip(mapmarker[4]);
            mapmarker[4].ToolTipText = "Pluag Gate";
            mapmarker[4].ToolTipMode = MarkerTooltipMode.Always;



            stations.Markers.Add(currentMarker);
            stations.Markers.Add(mapmarker[0]);
            stations.Markers.Add(mapmarker[1]);
            stations.Markers.Add(mapmarker[2]);
            stations.Markers.Add(mapmarker[3]);
            stations.Markers.Add(mapmarker[4]);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMap.Zoom = ((int)MainMap.Zoom) + 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBar1.Value / 100.0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Value = (int)MainMap.Zoom * 100;
        }

        private void MainMap_OnMapZoomChanged()
        {
            trackBar1.Value = (int)MainMap.Zoom * 100;
        }



        private void DirectionHistogram_DoubleClick(object sender, EventArgs e)
        {
            if (this.DirectionHistogram.Series[0].ChartType == SeriesChartType.StackedBar)
            {
                this.DirectionHistogram.Series[0].ChartType = SeriesChartType.StackedColumn;
                this.DirectionHistogram.Series[1].ChartType = SeriesChartType.StackedColumn;
                this.DirectionHistogram.Series[2].ChartType = SeriesChartType.StackedColumn;
                this.DirectionHistogram.Series[3].ChartType = SeriesChartType.StackedColumn;
                this.DirectionHistogram.Series[4].ChartType = SeriesChartType.StackedColumn;
                this.DirectionHistogram.Series[5].ChartType = SeriesChartType.StackedColumn;
            }
            else
            {
                this.DirectionHistogram.Series[0].ChartType = SeriesChartType.StackedBar;
                this.DirectionHistogram.Series[1].ChartType = SeriesChartType.StackedBar;
                this.DirectionHistogram.Series[2].ChartType = SeriesChartType.StackedBar;
                this.DirectionHistogram.Series[3].ChartType = SeriesChartType.StackedBar;
                this.DirectionHistogram.Series[4].ChartType = SeriesChartType.StackedBar;
                this.DirectionHistogram.Series[5].ChartType = SeriesChartType.StackedBar;

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            zedWindIndex.AddDataSource(dgvAnalysisSource);
            zEnose.AddDataSource(dgvAnalysisSource);
            zedWindIndex.Invalidate();
            zEnose.UpdateGraph();

            //FillZedGraph();
            //RefreshRoseWindIndexChart();


        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            zedWindIndex.AddDataSource(dgvAnalysisSource);
            zedWindIndex.Invalidate();
        }

        private void UpdateZedGraph(string p, bool checkState)
        {
            if (zedWindIndex.haveDtasource == false)
            {
                zedWindIndex.AddDataSource(dgvAnalysisSource);
            }

            zedWindIndex.setCheck(p, checkState);
            zedWindIndex.Invalidate();
        }


        private void cbNorth_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateZedGraph("N", cbNorth.Checked);
            UpdateZedGraph("NE", cbNorthEast.Checked);
            UpdateZedGraph("E", cbEast.Checked);
            UpdateZedGraph("SE", cbSouthEast.Checked);
            UpdateZedGraph("S", cbSouth.Checked);
            UpdateZedGraph("SW", cbSouthWest.Checked);
            UpdateZedGraph("W", cbWest.Checked);
            UpdateZedGraph("NW", cbNorthWest.Checked);
        }

        private void tabControl1_Layout(object sender, LayoutEventArgs e)
        {
          //  tabPage2.Parent = null; // Hide WinRose tab
            //tabPage5.Parent = tabControl1;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AnalyzeWindEnose();
        }

        private void AnalyzeWindEnose()
        {
            //weAnalyze.GraphIt();
            weAnalyze.WindIndex = zedWindIndex;
            weAnalyze._zedEnose = zEnose;
            //weAnalyze.graphit2();
            //weAnalyze.syncPane();
            //weAnalyze.graphit3();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            weAnalyze.UpdateWindEnoseChart("N", cb_N.Checked);
            weAnalyze.UpdateWindEnoseChart("NE", cb_NE.Checked);
            weAnalyze.UpdateWindEnoseChart("E", cb_E.Checked);
            weAnalyze.UpdateWindEnoseChart("SE", cb_SE.Checked);
            weAnalyze.UpdateWindEnoseChart("S", cb_S.Checked);
            weAnalyze.UpdateWindEnoseChart("SW", cb_SW.Checked);
            weAnalyze.UpdateWindEnoseChart("W", cb_W.Checked);
            weAnalyze.UpdateWindEnoseChart("NW", cb_NW.Checked);
        }

        private void cbTGS825_CheckedChanged(object sender, EventArgs e)
        {
            weAnalyze.UpdateSensorChart("TGS825",cbTGS825.Checked);
            weAnalyze.UpdateSensorChart("TGS830",cbTGS830.Checked);
            weAnalyze.UpdateSensorChart("TGS2600",cbTGS2600.Checked);
            weAnalyze.UpdateSensorChart("TGS2602",cbTGS2602.Checked);
            weAnalyze.UpdateSensorChart("TGS2610",cbTGS2610.Checked);
            weAnalyze.UpdateSensorChart("TGS4161",cbTGS4161.Checked);

        }

    }


}


#region archive
//private void DirectionHistogram_DoubleClick(object sender, EventArgs e)
//{
//    if (this.DirectionHistogram.Series[0].ChartType == SeriesChartType.StackedBar)
//    {
//        this.DirectionHistogram.Series[0].ChartType = SeriesChartType.StackedColumn;
//        this.DirectionHistogram.Series[1].ChartType = SeriesChartType.StackedColumn;
//        this.DirectionHistogram.Series[2].ChartType = SeriesChartType.StackedColumn;
//        this.DirectionHistogram.Series[3].ChartType = SeriesChartType.StackedColumn;
//        this.DirectionHistogram.Series[4].ChartType = SeriesChartType.StackedColumn;
//    }
//    else
//    {
//        this.DirectionHistogram.Series[0].ChartType = SeriesChartType.StackedBar;
//        this.DirectionHistogram.Series[1].ChartType = SeriesChartType.StackedBar;
//        this.DirectionHistogram.Series[2].ChartType = SeriesChartType.StackedBar;
//        this.DirectionHistogram.Series[3].ChartType = SeriesChartType.StackedBar;
//        this.DirectionHistogram.Series[4].ChartType = SeriesChartType.StackedBar;

//    }
//}

//private void FillZedGraph()
//{
//    CultureInfo provider = CultureInfo.CurrentUICulture;
//    zedWindIndex.pplNorth = new PointPairList();
//    string format = "d/M/yyyy hh:mm";

//    for (int i = 0; i < dgvAnalysisSource.Rows.Count - 1; i++)
//    {
//        string dateString = dgvAnalysisSource.Rows[i].Cells["Date-Time"].Value.ToString();
//        DateTime DT = DateTime.Parse(dateString, CultureInfo.CurrentCulture);

//        double WS = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["WS"].Value);
//        //DateTime DT;
//        //DateTime.TryParseExact(dateString, format, provider, System.Globalization.DateTimeStyles.None, out DT);

//        XDate xdt = new XDate(DT);
//        zedWindIndex.pplNorth.Add(xdt, WS);
//        Console.WriteLine();
//        Console.WriteLine(i);
//    }


//}

//private void RefreshRoseWindIndexChart()
//{
//    GraphPane myPane = zedGraphControl1.GraphPane;
//    // Set the Titles
//    myPane.Title.Text = "Wind Index";
//    myPane.XAxis.Title.Text = "Time";
//    myPane.YAxis.Title.Text = "Effective wind";
//    myPane.XAxis.Type = ZedGraph.AxisType.Date;

//    // Make up some data arrays based on the Sine function
//    double x, y1, y2;
//    PointPairList list1 = new PointPairList();
//    PointPairList list2 = new PointPairList();



//    //for (int i = 0; i < 36; i++)
//    //{
//    //    x = (double)i + 5;
//    //    y1 = 1.5 + Math.Sin((double)i * 0.2);
//    //    y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
//    //    //zedWindIndex.pplNorth.Add(x, y1); 
//    //        //list1.Add(x, y1);
//    //    //list2.Add(x, y2);
//    //}


//    // Generate a red curve with diamond
//    // symbols, and "Porsche" in the legend
//    LineItem myCurve = myPane.AddCurve("Porsche", zedWindIndex.pplNorth, Color.Red, SymbolType.Diamond);

//    // Generate a blue curve with circle
//    // symbols, and "Piper" in the legend
//    //  LineItem myCurve2 = myPane.AddCurve("Piper", list2, Color.Blue, SymbolType.Circle);

//    // Tell ZedGraph to refigure the
//    // axes since the data have changed
//    zedGraphControl1.AxisChange();



//    zedGraphControl1.Invalidate();
//}
//private void toolStripMenuItem1_Click(object sender, EventArgs e)
//{
//    //if (toolStripMenuItem1.Checked == false)
//    //{
//    //    toolStripMenuItem1.Checked = true;
//    //    if (toolStripMenuItem2.Checked == true)
//    //        toolStripMenuItem2.Checked = false;
//    //}
//}
//private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
//{

//}

//private void stackedBarToolStripMenuItem_Click(object sender, EventArgs e)
//{
//}

//private void stackColumnToolStripMenuItem_Click(object sender, EventArgs e)
//{

//}

//private void DirectionHistogram_MouseClick(object sender, MouseEventArgs e)
//{

//    //contextMenuStrip1.Show(this, Control.MousePosition);
//}



//private void toolStripMenuItem2_Click(object sender, EventArgs e)
//{

//}

#endregion