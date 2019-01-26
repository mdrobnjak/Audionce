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

            InitBufferAndGraphicForGate();
            InitConverter(converterScale);

            Pass = new bool[Range.Count];
            Levels = new float[Range.Count];
        }

        private void InitConverter(int yMult)
        {
            double maxScaledY = (4096d / FFT.N_FFT) * yMult;
            cvt = new Converter(0, pnlGate.Location.Y + pnlGate.Height, 1, maxScaledY);
        }

        Image mainBuffer;
        Graphics gMainBuffer;

        Image tempBuffer;
        Graphics gTempBuffer;

        ColorMatrix colormatrix = new ColorMatrix(new float[][]
                        {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                          //  new float[]{0, 0, 0, -0.001f, 1},
                            new float[]{-0.01f, -0.01f, -0.01f, 0, 0}
                          //  new float[]{0, 0, 0, 0, 1}
                        });
        ImageAttributes imgAttribute;

        Graphics gGate;

        private void InitBufferAndGraphicForGate()
        {
            mainBuffer = new Bitmap(pnlGate.Width, pnlGate.Height, PixelFormat.Format32bppArgb);
            gMainBuffer = Graphics.FromImage(mainBuffer);
            gMainBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            gMainBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            
            tempBuffer = new Bitmap(mainBuffer.Width, mainBuffer.Height);
            gTempBuffer = Graphics.FromImage(tempBuffer);
            imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            gTempBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            gTempBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            
            Constants.Init();

            this.gGate = pnlGate.CreateGraphics();
        }

        #region Draw Spectrum

        delegate void DrawCallback();

        public void Draw()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (pnlGate.InvokeRequired)
            {
                DrawCallback d = new DrawCallback(Draw);
                this.Invoke(d);
            }
            else
            {
                if (Enabled)
                {
                    try
                    {
                        PaintGate();
                    }
                    catch
                    {
                    }
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
            DrawData(gGate, cvt);
        }

        private void DrawData(Graphics g, Converter cvter)
        {

            float ratioFreq = (float)pnlGate.Width / Range.Count;

            g.Clear(Color.White);

            int sx, sy;

            #region Fill Rectangles

            for (int i = 0; i < Range.Count; i++)
            {
                cvter.FromReal(i * ratioFreq, 0, out sx, out sy);
                Levels[i] = Pass[i] ? (float)(pnlGate.Height) : Levels[i] - pnlGate.Height / 20;
                g.FillRectangle(Constants.Brushes.blackBrush, i * ratioFreq, sy - Levels[i] / 1, ratioFreq - 1, Levels[i] / 2);
            }
            #endregion
        }

        #endregion

        private void pnlGate_SizeChanged(object sender, EventArgs e)
        {
            InitBufferAndGraphicForGate();
            InitConverter(converterScale);
            cvt._yCenter = pnlGate.Location.Y + pnlGate.Height;
        }
    }
}
