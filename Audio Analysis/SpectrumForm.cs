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
    public partial class SpectrumForm : Form
    {
        public static new bool Enabled = true;

        static Converter cvt;
        int converterScale = 8;

        public SpectrumForm()
        {
            InitializeComponent();

            InitRectangles();
            
            this.DoubleBuffered = true;
            InitConverter(converterScale);

            txtmsiScale.Text = converterScale.ToString();
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
                    try
                    {
                        UpdateSpectrum();
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

        private void UpdateSpectrum()
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
            UpdateRectangles(FFT.transformedData, cvt);
        }

        public void InitRectangles()
        {
            float ratioFreq = (float)this.Width / Spectrum.DisplayBands;

            //Init Rectangles
            rects = new RectangleF[Spectrum.DisplayBands];

            for(int i = 0; i < Spectrum.DisplayBands; i++)
            {
                rects[i] = new RectangleF(
                    i * ratioFreq,
                    0,
                    ratioFreq - 1,
                    0);
            }
        }

        RectangleF[] rects = new RectangleF[1];

        private void UpdateRectangles(float[] data, Converter cvter)
        {
            if (data == null || data.Length == 0 /*|| AudioIn.sourceData == null*/)
                return;

            if (rects.Count() != Spectrum.DisplayBands)
            {
                InitRectangles();
            }

            float ratioFreq = (float)this.Width / Spectrum.DisplayBands;

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = Spectrum.Full ? 0 : Range.Active.NumBandsBefore;

            for (; bandIndexRelative < Spectrum.DisplayBands; bandIndexRelative++, bandIndexAbsolute++)
            {
                rects[bandIndexRelative].Y = cvter._containerHeight - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
                rects[bandIndexRelative].Height = (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
            }

            Invalidate();

            #endregion
        }

        #endregion

        public void IncrementRange()
        {
            Range.Active.LowCutAbsolute++;
            Range.Active.HighCutAbsolute++;
        }

        public void DecrementRange()
        {
            Range.Active.LowCutAbsolute--;
            Range.Active.HighCutAbsolute--;
        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitConverter(converterScale);
            InitRectangles();
            cvt._containerHeight = this.Height-40;
        }

        private void msActiveRangeOnly_CheckStateChanged(object sender, EventArgs e)
        {
            Spectrum.Full = !msActiveRangeOnly.Checked;
        }

        private void txtmsScale_TextChanged(object sender, EventArgs e)
        {
            int intVar;

            if (!Int32.TryParse(txtmsiScale.Text, out intVar)) return;
            converterScale = intVar;
            InitConverter(converterScale);
        }

        int offset = 0;
        int tempBW = 0;
        int mouseDownBandIndex;
        double sizePerBand;

        private void SpectrumForm_MouseDown(object sender, MouseEventArgs e)
        {
            sizePerBand = (double)this.Size.Width / Spectrum.DisplayBands;

            mouseDownBandIndex = (int)(e.Location.X / sizePerBand);

            this.MouseLeave += new System.EventHandler(this.SpectrumForm_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpectrumForm_MouseMove);

            if (e.Button == MouseButtons.Right)
            {
                offset = Range.Active.LowCutIndex - mouseDownBandIndex;
                tempBW = Range.Active.HighCutIndex - Range.Active.LowCutIndex;
            }
            else
            {
                Range.Active.LowCutIndex = mouseDownBandIndex;
                Range.Active.HighCutIndex = mouseDownBandIndex + 1;
            }
        }

        private void SpectrumForm_MouseUp(object sender, MouseEventArgs e)
        {
            tempBW = 0;
            this.MouseLeave -= new System.EventHandler(this.SpectrumForm_MouseLeave);
            this.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.SpectrumForm_MouseMove);
        }

        private void SpectrumForm_MouseMove(object sender, MouseEventArgs e)
        {
            int mouseHoverBandIndex = (int)(e.Location.X / sizePerBand);

            if (tempBW == 0)
            {
                if (mouseHoverBandIndex > mouseDownBandIndex)
                {
                    Range.Active.HighCutIndex = mouseHoverBandIndex;
                }
                else if (mouseHoverBandIndex < mouseDownBandIndex)
                {
                    Range.Active.LowCutIndex = mouseHoverBandIndex;
                }
            }
            else
            {
                Range.Active.LowCutIndex = mouseHoverBandIndex + offset;
                Range.Active.HighCutIndex = mouseHoverBandIndex + tempBW + offset;
            }
        }

        private void SpectrumForm_MouseLeave(object sender, EventArgs e)
        {
            SpectrumForm_MouseUp(null, null);
        }

        private void SpectrumForm_Paint(object sender, PaintEventArgs e)
        {
            int iLow = Math.Min(Range.Active.LowCutIndex, Spectrum.DisplayBands);
            int iHigh = Math.Min(Range.Active.HighCutIndex, Spectrum.DisplayBands);

            int i = 0;

            for (; i < iLow; i++)
            {
                e.Graphics.FillRectangle(Constants.Brushes.blackBrush,rects[i]);
            }
            for (; i < iHigh; i++)
            {
                e.Graphics.FillRectangle(Constants.Brushes.redBrush, rects[i]);
            }
            for (; i < Spectrum.DisplayBands; i++)
            {
                e.Graphics.FillRectangle(Constants.Brushes.blackBrush, rects[i]);
            }
        }
    }
}
