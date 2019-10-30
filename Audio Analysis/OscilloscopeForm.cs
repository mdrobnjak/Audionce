using System;
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
    public partial class OscilloscopeForm : Form
    {
        new bool Enabled = true;

        public OscilloscopeForm()
        {
            this.BackColor = Color.White;

            InitializeComponent();
            this.SizeChanged += new System.EventHandler(this.ChartForm_SizeChanged);

            DoubleBuffered = true;

            InitRectangles();
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

        const int XAxis = 200;
        float[] chartData = new float[XAxis];
        RectangleF[] rects = new RectangleF[XAxis];

        private void UpdateRectangles(float[] chartData)
        {
            float barWidth = (float)this.Width / chartData.Length;

            for (int i = 0; i < chartData.Length; i++)
            {
                rects[i].Y = (this.Height) - chartData[i];
                rects[i].Height = chartData[i];
            } 

            Invalidate();
        }

        void InitRectangles()
        {
            float barWidth = (float)this.Width / chartData.Length;

            for (int i = 0; i < chartData.Length; i++)
            {
                rects[i] = new RectangleF(
                    i * barWidth,
                    this.Height - chartData[i],
                    barWidth,
                    chartData[i]);
            }
        }

        private void DrawChart()
        {
            Array.Copy(chartData, 1, chartData, 0, chartData.Length - 1);
            chartData[chartData.Length - 1] = SoundCapture.GetSingleBlock() * 1000;

            UpdateRectangles(chartData);
        }

        private void ChartForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitRectangles();
        }

        private void ChartForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangles(Brushes.blackBrush, rects);
        }

        private void ChartForm_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
