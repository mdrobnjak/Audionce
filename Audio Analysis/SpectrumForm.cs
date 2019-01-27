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
            double maxScaledY = (4096d / FFT.N_FFT) * yMult;
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
                });
                paintInitiated = true;
            }
            DrawData(FFT.transformedData, gSpectrum, cvt);
        }

        private void DrawData(double[] data, Graphics g, Converter cvter)
        {
            if (data == null || data.Length == 0 || AudioIn.sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.DisplayBands;
            float value = 0;

            g.Clear(Color.White);

            int sx, sy;

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = Spectrum.Full ? 0 : Range.Active.NumBandsBefore;

            for (; bandIndexRelative < trkbrMin.Value; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            for (; bandIndexRelative < trkbrMax.Value; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.redBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            for (; bandIndexRelative < Spectrum.DisplayBands; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            #endregion
        }

        #endregion
        
        public void UpdateControls()
        {
            trkbrMax.Maximum = trkbrMin.Maximum = Spectrum.DisplayBands;

            trkbrMin.Value = Range.Active.LowCutIndex;
            trkbrMax.Value = Range.Active.HighCutIndex;
        }

        public void IncrementRange()
        {
            if (trkbrMax.Value == trkbrMax.Maximum) return;
            trkbrMin.Value++;
            trkbrMax.Value++;
        }

        public void DecrementRange()
        {
            if (trkbrMin.Value == trkbrMin.Minimum) return;
            trkbrMin.Value--;
            trkbrMax.Value--;
        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            InitBufferAndGraphicForSpectrum();
            InitConverter(converterScale);
            cvt._yCenter = pnlSpectrum.Location.Y + pnlSpectrum.Height;
        }

        private void msActiveRangeOnly_CheckStateChanged(object sender, EventArgs e)
        {
            Spectrum.Full = !msActiveRangeOnly.Checked;
            UpdateControls();
        }

        private void trkbrMin_ValueChanged(object sender, EventArgs e)
        {
            Range.Active.LowCutIndex = trkbrMin.Value;
        }

        private void trkbrMax_ValueChanged(object sender, EventArgs e)
        {
            Range.Active.HighCutIndex = trkbrMax.Value;
        }

        private void txtmsScale_TextChanged(object sender, EventArgs e)
        {
            int intVar;

            if (!Int32.TryParse(txtmsiScale.Text, out intVar)) return;
            InitConverter(intVar);
        }
    }
}
