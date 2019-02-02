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

namespace AudioAnalyzer
{
    public partial class GateForm : Form
    {
        Converter cvt;
        int converterScale = 8;

        public bool[] Pass;
        float[] Levels;

        public GateForm()
        {
            InitializeComponent();

            InitConverter(converterScale);

            DoubleBuffered = true;

            Pass = new bool[Range.Count];
            Levels = new float[Range.Count];
        }

        private void InitConverter(int yMult)
        {
            float maxScaledY = (4096f / FFT.N_FFT) * yMult;
            cvt = new Converter(0, this.Height - 40, 1, maxScaledY);
        }

        #region Draw Spectrum

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
                if (Enabled)
                {
                    PaintGate();
                }
            }
        }

        bool paintInitiated = false;
        Font pen;
        OSD osdPanel = new OSD();

        private void PaintGate()
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
                paintInitiated = true;
            }
            UpdateRectangles(cvt);
        }

        RectangleF[] rects = new RectangleF[Range.Count];

        private void UpdateRectangles(Converter cvter)
        {
            float ratioFreq = (float)this.Width / Range.Count;

            for (int i = 0; i < Range.Count; i++)
            {
                Levels[i] = Pass[i] ? (float)(this.Height) : Levels[i] - this.Height / 20;
                rects[i].X = i * ratioFreq;
                rects[i].Y = cvter._containerHeight - Levels[i];
                rects[i].Width = ratioFreq - 1;
                rects[i].Height = Levels[i] / 2;
            }
            Invalidate();
        }

        #endregion

        private void GateForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitConverter(converterScale);
            cvt._containerHeight = this.Height - 40;
        }

        private void GateForm_MouseClick(object sender, MouseEventArgs e)
        {
            int sizePerRange = this.Size.Width / Range.Count;

            ((AudioAnalyzerMDIForm)MdiParent).MakeActive(e.Location.X / sizePerRange);
        }

        private void GateForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < rects.Count(); i++)
            {
                e.Graphics.FillRectangle(Constants.Brushes.gateBrushes[i], rects[i]);
            }
        }
    }
}
