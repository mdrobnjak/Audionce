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

        Converter cvt;
        int converterScale = 8;

        public SpectrumForm()
        {
            InitializeComponent();

            InitBufferAndGraphicForSpectrum();
            InitConverter(converterScale);

            txtmsiScale.Text = converterScale.ToString();
        }

        private void InitConverter(int yMult)
        {
            float maxScaledY = (4096f / FFT.N_FFT) * yMult;
            cvt = new Converter(0, pnlSpectrum.Location.Y + pnlSpectrum.Height, 1, maxScaledY);
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

        Graphics gSpectrum;

        private void InitBufferAndGraphicForSpectrum()
        {
            //mainBuffer is a new Bitmap.
            //What is panel1?
            //What is the PixelFormat?
            mainBuffer = new Bitmap(pnlSpectrum.Width, pnlSpectrum.Height, PixelFormat.Format32bppArgb);
            //gMainBuffer is a new Graphics made from mainBuffer.
            gMainBuffer = Graphics.FromImage(mainBuffer);
            //CompositingQuality is set to HighSpeed.
            gMainBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            //InterpolationMode is set to Low.
            gMainBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            //tempBuffer is a new Bitmap.
            tempBuffer = new Bitmap(mainBuffer.Width, mainBuffer.Height);
            //gTempBuffer is a new Graphics made from tempBuffer.
            gTempBuffer = Graphics.FromImage(tempBuffer);
            //imgAttribute is a new ImageAttributes.
            imgAttribute = new ImageAttributes();
            //Set imgAttribute's color adjustment matrix.
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            //CompositingQuality is set to HighSpeed.
            gTempBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            //InterpolationMode is set to NearestNeighbor.
            gTempBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //Constants is initialized.
            //(3 SolidBrushes are initialized.)
            Constants.Init();

            this.gSpectrum = pnlSpectrum.CreateGraphics();
        }

        #region Draw Spectrum

        delegate void DrawCallback();

        public void Draw()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (pnlSpectrum.InvokeRequired)
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
                        PaintSpectrum();
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

        private void PaintSpectrum()
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

                    InitRectangles();

                });
                paintInitiated = true;
            }
            DrawData(FFT.transformedData, gSpectrum, cvt);
        }

        public void InitRectangles()
        {
            int iLow = Math.Min(Range.Active.LowCutIndex, Spectrum.DisplayBands);
            int iHigh = Math.Min(Range.Active.HighCutIndex, Spectrum.DisplayBands);
            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.DisplayBands;

            int bandIndexRelative = 0; //Spectrum.Full ? 0 : Range.Active.NumBandsBefore; //

            //Init Rectangles

            rects = new RectangleF[Spectrum.DisplayBands];
            rectsR = new RectangleF[Spectrum.DisplayBands];

            for (; bandIndexRelative < iLow; bandIndexRelative++)
            {
                rects[bandIndexRelative] = new RectangleF(
                    bandIndexRelative * ratioFreq,
                    1,
                    ratioFreq - 1,
                    1);
            }
            for (; bandIndexRelative < iHigh; bandIndexRelative++)
            {
                rectsR[bandIndexRelative] = new RectangleF(
                    bandIndexRelative * ratioFreq,
                    1,
                    ratioFreq - 1,
                    1);
            }
            for (; bandIndexRelative < Spectrum.DisplayBands; bandIndexRelative++)
            {
                rects[bandIndexRelative] = new RectangleF(
                    bandIndexRelative * ratioFreq,
                    1,
                    ratioFreq - 1,
                    1);
            }
        }

        RectangleF[] rects = new RectangleF[1];
        RectangleF[] rectsR = new RectangleF[1];

        private void DrawData(float[] data, Graphics g, Converter cvter)
        {
            if (data == null || data.Length == 0 /*|| AudioIn.sourceData == null*/)
                return;

            int iLow = Math.Min(Range.Active.LowCutIndex, Spectrum.DisplayBands);
            int iHigh = Math.Min(Range.Active.HighCutIndex, Spectrum.DisplayBands);
            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.DisplayBands;

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = Spectrum.Full ? 0 : Range.Active.NumBandsBefore;

            //if (rects.Length != Spectrum.DisplayBands)
            //{
            //    //Init Rectangles

            //    rects = new RectangleF[Spectrum.DisplayBands];
            //    rectsR = new RectangleF[Spectrum.DisplayBands];

            //    for (; bandIndexRelative < iLow; bandIndexRelative++, bandIndexAbsolute++)
            //    {
            //        rects[bandIndexRelative] = new RectangleF(
            //            bandIndexRelative * ratioFreq,
            //            cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2,
            //            ratioFreq - 1,
            //            (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2);
            //    }
            //    for (; bandIndexRelative < iHigh; bandIndexRelative++, bandIndexAbsolute++)
            //    {
            //        rectsR[bandIndexRelative] = new RectangleF(
            //            bandIndexRelative * ratioFreq,
            //            cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2,
            //            ratioFreq - 1,
            //            (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2);
            //    }
            //    for (; bandIndexRelative < Spectrum.DisplayBands; bandIndexRelative++, bandIndexAbsolute++)
            //    {
            //        rects[bandIndexRelative] = new RectangleF(
            //            bandIndexRelative * ratioFreq,
            //            cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2,
            //            ratioFreq - 1,
            //            (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2);
            //    }
            //}

            for (; bandIndexRelative < iLow; bandIndexRelative++, bandIndexAbsolute++)
            {
                rects[bandIndexRelative].Y = cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
                rects[bandIndexRelative].Height = (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
            }
            for (; bandIndexRelative < iHigh; bandIndexRelative++, bandIndexAbsolute++)
            {
                rectsR[bandIndexRelative].Y = cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
                rectsR[bandIndexRelative].Height = (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
            }
            for (; bandIndexRelative < Spectrum.DisplayBands; bandIndexRelative++, bandIndexAbsolute++)
            {
                rects[bandIndexRelative].Y = cvter._yCenter - (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
                rects[bandIndexRelative].Height = (data[bandIndexAbsolute] * cvt.MaxScaledY) / 2;
            }

            g.Clear(Color.White);
            g.FillRectangles(Constants.Brushes.blackBrush, rects);
            g.FillRectangles(Constants.Brushes.redBrush, rectsR);

            #endregion
        }

        #endregion

        public void IncrementRange()
        {
            Range.Active.LowCutAbsolute++;
            Range.Active.HighCutAbsolute++;
            InitRectangles();
        }

        public void DecrementRange()
        {
            Range.Active.LowCutAbsolute--;
            Range.Active.HighCutAbsolute--;
            InitRectangles();
        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.MdiParent.WindowState == FormWindowState.Minimized) return;
            InitBufferAndGraphicForSpectrum();
            InitConverter(converterScale);
            cvt._yCenter = pnlSpectrum.Location.Y + pnlSpectrum.Height;
            InitRectangles();
        }

        private void msActiveRangeOnly_CheckStateChanged(object sender, EventArgs e)
        {
            Spectrum.Full = !msActiveRangeOnly.Checked;
            InitRectangles();
        }

        private void txtmsScale_TextChanged(object sender, EventArgs e)
        {
            int intVar;

            if (!Int32.TryParse(txtmsiScale.Text, out intVar)) return;
            InitConverter(intVar);
        }

        int offset = 0;
        int tempBW = 0;
        double sizePerBand;

        private void pnlSpectrum_MouseDown(object sender, MouseEventArgs e)
        {
            sizePerBand = (double)pnlSpectrum.Size.Width / Spectrum.DisplayBands;

            int mouseDownBandIndex = (int)(e.Location.X / sizePerBand);

            this.pnlSpectrum.MouseLeave += new System.EventHandler(this.pnlSpectrum_MouseLeave);
            this.pnlSpectrum.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSpectrum_MouseMove);

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

        private void pnlSpectrum_MouseUp(object sender, MouseEventArgs e)
        {
            tempBW = 0;
            this.pnlSpectrum.MouseLeave -= new System.EventHandler(this.pnlSpectrum_MouseLeave);
            this.pnlSpectrum.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.pnlSpectrum_MouseMove);
            InitRectangles();
        }

        private void pnlSpectrum_MouseMove(object sender, MouseEventArgs e)
        {
            int mouseHoverBandIndex = (int)(e.Location.X / sizePerBand);

            if (tempBW == 0)
            {
                if (mouseHoverBandIndex > Range.Active.LowCutIndex && mouseHoverBandIndex != Range.Active.HighCutIndex)
                    Range.Active.HighCutIndex = mouseHoverBandIndex;
            }
            else
            {
                Range.Active.LowCutIndex = mouseHoverBandIndex + offset;
                Range.Active.HighCutIndex = mouseHoverBandIndex + tempBW + offset;
            }
            InitRectangles();
        }

        private void pnlSpectrum_MouseLeave(object sender, EventArgs e)
        {
            pnlSpectrum_MouseUp(null, null);
        }
    }
}
