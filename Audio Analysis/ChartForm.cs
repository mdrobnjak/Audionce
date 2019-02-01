﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AudioAnalyzer
{
    public partial class ChartForm : Form
    {
        new bool Enabled = true;

        Converter cvt;
        int converterScale = 8;

        float maxYmult = .7f;

        Rectangle stripLine;

        public ChartForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

            InitConverter(converterScale);

            InitStripLine();
        }

        void InitStripLine()
        {
            stripLine = new Rectangle(0, this.Height - Range.Active.Threshold, this.Width, 3);
        }

        void UpdateStripLineByThreshold()
        {
            stripLine.Y = (int)(this.Height - ((Range.Active.Threshold * this.Height) / (Range.Active.GetMaxAudioFromLast200() * maxYmult)));
        }

        void UpdateStripLine(int y)
        {
            stripLine.Y = y;
        }

        private void InitConverter(int yMult)
        {
            float maxScaledY = (4096f / FFT.N_FFT) * yMult;
            cvt = new Converter(0, this.Height - 40, 1, maxScaledY);
        }

        delegate void DrawCallback();

        public void Draw()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                DrawCallback d = new DrawCallback(Draw);
                this.Invoke(d);
            }
            else
            {
                if (Enabled) DrawChart();
            }
        }

        bool paintInitiated = false;
        Font pen;
        OSD osdPanel = new OSD();

        private void PaintChart()
        {
            if (!paintInitiated)
            {
                pen = new Font("Arial", 12);
                osdPanel.ImplementDrawAction(delegate (object[] pars)
                {
                    Graphics g = pars[0] as System.Drawing.Graphics;
                    OSD osd = pars[1] as OSD;
                    if (osd.Info == null)
                        return;
                    int yIndex = 0;
                    foreach (var key in osd.Info.Keys)
                    {
                        g.DrawString(string.Format("{0}:{1}", key, osdPanel.Info[key]), pen, new SolidBrush(Color.Red), new PointF(10, this.Height / 2 + yIndex * 20));
                        yIndex++;
                    }
                });

                InitRectangles(cvt);

                paintInitiated = true;
            }

            UpdateRectangles(chartData, cvt);
        }

        const int XAxis = 200;
        float[] chartData = new float[XAxis];
        RectangleF[] rects = new RectangleF[XAxis];

        private void UpdateRectangles(float[] chartData, Converter cvter)
        {
            float barWidth = (float)this.Width / chartData.Length;

            for (int i = 0; i < chartData.Length; i++)
            {
                rects[i].Y = cvter._containerHeight - ((chartData[i] / Range.Active.GetMaxAudioFromLast200() * maxYmult) * this.Height);
                rects[i].Height = ((chartData[i] / Range.Active.GetMaxAudioFromLast200() * maxYmult) * this.Height);
            }

            UpdateStripLineByThreshold();

            Invalidate();
        }

        void InitRectangles(Converter cvter)
        {
            float barWidth = (float)this.Width / chartData.Length;

            for (int i = 0; i < chartData.Length; i++)
            {
                rects[i] = new RectangleF(
                    i * barWidth,
                    cvter._containerHeight - ((chartData[i] / Range.Active.GetMaxAudioFromLast200() * maxYmult) * this.Height),
                    barWidth,
                    ((chartData[i] / Range.Active.GetMaxAudioFromLast200() * maxYmult) * this.Height));
            }
        }

        private void DrawChart()
        {
            if (Range.Active.AutoSettings.DynamicThreshold) AutoThreshold();

            Array.Copy(chartData, 1, chartData, 0, chartData.Length - 1);
            chartData[chartData.Length - 1] = Range.Active.Audio;
            PaintChart();
        }

        public void AutoThreshold()
        {
            Range.Active.Threshold = (int)(Range.Active.AutoSettings.ThresholdMultiplier * Range.Active.GetMaxAudioFromLast200());
        }

        private void ChartForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitConverter(converterScale);
            InitStripLine();
            cvt._containerHeight = this.Height - 40;
            InitRectangles(cvt);
        }

        private void ChartForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangles(Constants.Brushes.blackBrush, rects);

            e.Graphics.FillRectangle(Constants.Brushes.redBrush, stripLine);
        }

        private void ChartForm_MouseClick(object sender, MouseEventArgs e)
        {
            Range.Active.Threshold = (int)(Range.Active.GetMaxAudioFromLast200() * maxYmult * (this.Height - e.Location.Y) / this.Height);
            //UpdateStripLine(e.Location.Y);
        }
    }
}
