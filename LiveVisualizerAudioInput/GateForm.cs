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

namespace LiveVisualizerAudioInput
{
    public partial class GateForm : Form
    {
        GraphicsConverter cvt;
        int converterScale = 8;

        public bool[] Pass;
        float[] Levels;

        public GateForm()
        {
            InitializeComponent();
            InitConverter(converterScale);
            this.SizeChanged += new System.EventHandler(this.GateForm_SizeChanged);

            this.DoubleBuffered = true;

            SetBackgroundImage();

            Pass = new bool[Range.Count];
            Levels = new float[Range.Count];
        }

        private void InitConverter(int yMult)
        {
            float maxScaledY = (4096f / FFT.N_FFT) * yMult;
            cvt = new GraphicsConverter(this.Height - 40, maxScaledY);
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
                if (Enabled)
                {
                    UpdateRectangles(cvt);
                }
            }
        }

        RectangleF[] rects = new RectangleF[Range.Count];

        private void UpdateRectangles(GraphicsConverter cvter)
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

        private void GateForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitConverter(converterScale);
            SetBackgroundImage();
            cvt._containerHeight = this.Height - 40;
        }

        private void GateForm_MouseClick(object sender, MouseEventArgs e)
        {
            int sizePerRange = this.Width / Range.Count;

            ((LiveVisualizerAudioInputMDIForm)MdiParent).MakeActive(e.Location.X / sizePerRange);
        }

        private void GateForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < rects.Count(); i++)
            {
                e.Graphics.FillRectangle(Brushes.gateBrushes[i], rects[i]);
            }
        }

        Bitmap background;

        void SetBackgroundImage()
        {
            int sizePerRange = this.Width / Range.Count;

            background = new Bitmap(this.Width, this.Height);
            using (Graphics g = Graphics.FromImage(background))
            {
                for (int i = 0; i < Range.Count; i++)
                {
                    int X = sizePerRange * i;
                    int width = sizePerRange;

                    g.FillRectangle(
                        Brushes.rangeBrushes[i],
                        new Rectangle(
                            X,
                            0,
                            width,
                            this.Height));
                }

                this.BackgroundImage = background;
            }
        }
    }
}
