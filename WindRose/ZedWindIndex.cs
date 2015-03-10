using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WindRose
{
    class ZedWindIndex : Form
    {
        PointPairList N = new PointPairList();
        PointPairList NE = new PointPairList();
        PointPairList E = new PointPairList();
        PointPairList SE = new PointPairList();
        PointPairList S = new PointPairList();
        PointPairList SW = new PointPairList();
        PointPairList W = new PointPairList();
        PointPairList NW = new PointPairList();
        private ZedGraphControl zedGraphControl1 = null;
        private bool datasrc = false;
        public bool haveDtasource
        {
            get { return datasrc; }
        }
        bool cbN, cbNE, cbE, cbSE, cbS, cbSW, cbW, cbNW;

        public void setCheck(string dir, bool checkState)
        {
            switch (dir)
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
        }

        public ZedWindIndex(ZedGraphControl zedGraphControl1)
        {
            this.zedGraphControl1 = zedGraphControl1;

        }

        System.Windows.Forms.DataGridView dgvAnalysisSource;
        #region properties
        public PointPairList pplNorth
        {
            get { return N; }
            set { N = value; }
        }


        public PointPairList pplNorthEast
        {
            get { return NE; }
            set { NE = value; }
        }

        public PointPairList pplEast
        {
            get { return E; }
            set { E = value; }
        }

        public PointPairList pplSouthEast
        {
            get { return SE; }
            set { SE = value; }
        }

        public PointPairList pplSouth
        {
            get { return S; }
            set { S = value; }
        }

        public PointPairList pplSouthWest
        {
            get { return SW; }
            set { SW = value; }
        }

        public PointPairList pplWest
        {
            get { return W; }
            set { W = value; }
        }

        public PointPairList pplNorthWest
        {
            get { return NW; }
            set { NW = value; }
        }

        #endregion


        internal void AddDataSource(System.Windows.Forms.DataGridView dgvAnalysisSource)
        {
            this.dgvAnalysisSource = dgvAnalysisSource;
            datasrc = true;
        }

        internal void Invalidate()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            // Set the Titles
            //myPane.Title.Text = "Wind Index";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "Effective wind factor";
            myPane.XAxis.Type = ZedGraph.AxisType.Date;
            DrawWinIndexCurve();
            myPane.CurveList.Clear();

            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.Title.Text = "Time (HH:MM)";
            myPane.XAxis.Scale.Format = "HH:mm\ndd/MM/yyyy";
 
            if (cbN == true)
            {
               LineItem line =  myPane.AddCurve("N", pplNorth, Color.Green, SymbolType.Diamond);
               line.Line.Width = 2.0f;
            }

            if (cbNE == true)
            {
                LineItem line = myPane.AddCurve("NE", pplNorthEast, Color.SaddleBrown, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }

            if (cbE == true)
            {
                LineItem line = myPane.AddCurve("E", pplEast, Color.Blue, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }
            if (cbSE == true)
            {
                LineItem line = myPane.AddCurve("SE", pplSouthEast, Color.Orange, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }

            if (cbS == true)
            {
                LineItem line = myPane.AddCurve("S", pplSouth, Color.Red, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }

            if (cbSW == true)
            {
                LineItem line = myPane.AddCurve("SW", pplSouthWest, Color.MediumBlue, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }
            if (cbW == true)
            {
                LineItem line = myPane.AddCurve("W", pplWest, Color.Brown, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }
            if (cbNW == true)
            {
                LineItem line = myPane.AddCurve("NW", pplNorthWest, Color.Magenta, SymbolType.Diamond);
                line.Line.Width = 2.0f;
            }
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void CopyToClip(GraphPane thePane)
        {
            Graphics g = this.CreateGraphics();
            IntPtr hdc = g.GetHdc();
            Metafile metaFile = new Metafile(hdc, EmfType.EmfOnly);
            g.ReleaseHdc(hdc);
            g.Dispose();

            Graphics gMeta = Graphics.FromImage(metaFile);
            thePane.Draw(gMeta);
            gMeta.Dispose();

            ClipboardMetafileHelper.PutEnhMetafileOnClipboard(this.Handle, metaFile);

            MessageBox.Show("Copied to ClipBoard");
        }
        public class ClipboardMetafileHelper
        {
            [DllImport("user32.dll")]
            static extern bool OpenClipboard(IntPtr hWndNewOwner);
            [DllImport("user32.dll")]
            static extern bool EmptyClipboard();
            [DllImport("user32.dll")]
            static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
            [DllImport("user32.dll")]
            static extern bool CloseClipboard();
            [DllImport("gdi32.dll")]
            static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr hNULL);
            [DllImport("gdi32.dll")]
            static extern bool DeleteEnhMetaFile(IntPtr hemf);

            // Metafile mf is set to a state that is not valid inside this function.
            static public bool PutEnhMetafileOnClipboard(IntPtr hWnd, Metafile mf)
            {
                bool bResult = false;
                IntPtr hEMF, hEMF2;
                hEMF = mf.GetHenhmetafile(); // invalidates mf
                if (!hEMF.Equals(new IntPtr(0)))
                {
                    hEMF2 = CopyEnhMetaFile(hEMF, new IntPtr(0));
                    if (!hEMF2.Equals(new IntPtr(0)))
                    {
                        if (OpenClipboard(hWnd))
                        {
                            if (EmptyClipboard())
                            {
                                IntPtr hRes = SetClipboardData(14 /*CF_ENHMETAFILE*/, hEMF2);
                                bResult = hRes.Equals(hEMF2);
                                CloseClipboard();
                            }
                        }
                    }
                    DeleteEnhMetaFile(hEMF);
                }
                return bResult;
            }
        }



        private void DrawWinIndexCurve()
        {
            if (dgvAnalysisSource == null)
                return;
            if (dgvAnalysisSource.Rows.Count == 0)
                return;

            ClearPpl();


            for (int i = 0; i < dgvAnalysisSource.Rows.Count - 1; i++)
            {
                string dateString = dgvAnalysisSource.Rows[i].Cells["DateTime"].Value.ToString();
                DateTime DT = DateTime.Parse(dateString, CultureInfo.DefaultThreadCurrentCulture);
                double WS = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["WS"].Value);
                double WD = Convert.ToDouble(dgvAnalysisSource.Rows[i].Cells["WD"].Value);
                XDate xdt = new XDate(DT);

                double eff = Math.Max(0.0, Math.Cos(Math.PI * 0.0 / 4.0 - WD.ToRadians())) * WS;
                pplNorth.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 1.0 / 4.0 - WD.ToRadians())) * WS;
                pplNorthEast.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 2.0 / 4.0 - WD.ToRadians())) * WS;
                pplEast.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 3.0 / 4.0 - WD.ToRadians())) * WS;
                pplSouthEast.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 4.0 / 4.0 - WD.ToRadians())) * WS;
                pplSouth.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 5.0 / 4.0 - WD.ToRadians())) * WS;
                pplSouthWest.Add(xdt, eff);

                eff = Math.Max(0.0, Math.Cos(Math.PI * 6.0 / 4.0 - WD.ToRadians())) * WS;
                pplWest.Add(xdt, eff);
                
                eff = Math.Max(0.0, Math.Cos(Math.PI * 7.0 / 4.0 - WD.ToRadians())) * WS;
                pplNorthWest.Add(xdt, eff);
            }
        }

        private void ClearPpl()
        {
            pplNorth.Clear();
            pplNorthEast.Clear();
            pplEast.Clear();
            pplSouthEast.Clear();
            pplSouth.Clear();
            pplSouthWest.Clear();
            pplWest.Clear();
            pplNorthWest.Clear();
        }
    }

    public static class MathExt
    {
        public static double Squared(this double x)
        {
            return x * x;
        }
        public static double ToRadians(this double degrees)
        {
            return degrees / (180D / Math.PI);
        }
        public static double ToDegrees(this double radians)
        {
            return radians * (180D / Math.PI);
        }
    }
}
