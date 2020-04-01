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
            this.BackColor = Color.Black;

            InitializeComponent();
            this.SizeChanged += new System.EventHandler(this.ChartForm_SizeChanged);

            DoubleBuffered = true;
            
            InitRectangles();
        }

        delegate void DrawCallback();

        public void Draw()
        {
            if (pauseDrawing)
            {
                //SoundCapture.GetBlocksSinceLastCapture();
                return;
            }
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

        const int XAxis = 160000;
        float[] scopeData = new float[XAxis];
        RectangleF[] rects = new RectangleF[XAxis];

        private void UpdateRectangles(float[] chartData)
        {
            float barWidth = (float)this.Width / chartData.Length;

            //for (int i = 0; i < chartData.Length; i++)
            //{
            //    if (chartData[i] > 0)
            //    {
            //        rects[i].Y = (this.Height) - chartData[i];
            //        rects[i].Y /= 2;
            //        rects[i].Height = chartData[i];
            //        rects[i].Height /= 2;
            //    }
            //    else
            //    {
            //        rects[i].Y = (this.Height) / 2;
            //        rects[i].Height = -chartData[i] / 2;
            //    }
            //} 

            Parallel.For(0, chartData.Length, i =>
            {
                if (chartData[i] > 0)
                {
                    rects[i].Y = (this.Height) - chartData[i];
                    rects[i].Y /= 2;
                    rects[i].Height = chartData[i];
                    rects[i].Height /= 2;
                }
                else
                {
                    rects[i].Y = (this.Height) / 2;
                    rects[i].Height = -chartData[i] / 2;
                }
            });

            Invalidate();
        }

        void InitRectangles()
        {
            float barWidth = (float)this.Width / scopeData.Length;

            for (int i = 0; i < scopeData.Length; i++)
            {
                rects[i] = new RectangleF(
                    i * barWidth,
                    this.Height - scopeData[i],
                    barWidth,
                    scopeData[i]);
            }
        }

        private void DrawChart()
        {
            List<float> blocks = new List<float>(/*SoundCapture.GetBlocksSinceLastCapture()*/);
            Array.Copy(scopeData, blocks.Count, scopeData, 0, scopeData.Length - blocks.Count);
            for (int i = 0; i < blocks.Count; i++)
            {
                scopeData[scopeData.Length - (blocks.Count - i)] = blocks[i] * 1000;
            }

            UpdateRectangles(scopeData);
        }

        private void ChartForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitRectangles(); 
        }

        private void ChartForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangles(Brushes.whiteBrush, rects);
        }

        bool pauseDrawing = false;
        private void ChartForm_MouseClick(object sender, MouseEventArgs e)
        {
            pauseDrawing = !pauseDrawing;
        }
    }
}
