using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class frmAudioAnalyzer : Form
    {
        Converter cvt;
        int converterScale = 8;

        private void InitConverter(int yMult)
        {
            double maxScaledY = (4096d / FFT.N_FFT) * yMult;
            cvt = new Converter(0, pnlSpectrum.Location.Y + pnlSpectrum.Height, 1, maxScaledY);
        }

        #region Draw Spectrum
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

            DrawData(transformedData, gSpectrum, cvt);
        }

        private void DrawData(double[] data, Graphics g, Converter cvter)
        {
            if (data == null || data.Length == 0 || AudioIn.sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.DisplayBands;
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = pnlSpectrum.Width / AudioIn.sourceData.Length;
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
      
        private void btnSpectrumMode_Click(object sender, EventArgs e)
        {
            Spectrum.Full = !Spectrum.Full;
            UpdateControls();
        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            InitBufferAndGraphicForSpectrum();
            InitConverter(converterScale);
            cvt._yCenter = pnlSpectrum.Location.Y + pnlSpectrum.Height;
        }

        private void btnToggleSpectrum_Click(object sender, EventArgs e)
        {
            Spectrum.drawSpectrum = !Spectrum.drawSpectrum;
            gSpectrum.Clear(Color.White);
        }
    }

    public class Spectrum : System.Windows.Forms.Panel
    {
        public static bool drawSpectrum = true;

        public Spectrum()
        {

        }

        public static int TotalBands;
        public static int DisplayBands
        {
            get
            {
                return Full ? TotalBands : Range.Active.NumBands;
            }
        }
        public static bool Full = true;

        public static Dictionary<int, int> FreqOfBand;

        public static void SyncBandsAndFreqs()
        {
            FreqOfBand = new Dictionary<int, int>();
            if (FFT.rawFFT)
            {
                TotalBands = FFT.N_FFTBuffer;
                for (int i = 0; i < TotalBands; i++)
                {
                    FreqOfBand[i] = i * AudioIn.RATE / FFT.N_FFT;
                }
            }
            else
            {
                int k = 1, n = 0;
                int mappedFreq;
                for (int i = 0; i < FFT.N_FFTBuffer / 2; i += k)
                {
                    mappedFreq = i * AudioIn.RATE / 2 / (FFT.N_FFTBuffer / 2);
                    for (int l = 0; l < FFT.chunk_freq.Length; l++)
                    {
                        if (mappedFreq < FFT.chunk_freq[l] || l == FFT.chunk_freq.Length - 1)
                        {
                            k = FFT.chunk_freq_jump[l];//chunk_freq[l] / chunk_freq[0];
                            break;
                        }
                    }
                    FreqOfBand[n++] = mappedFreq;
                }
                TotalBands = n;
            }
        }

        public static int GetBandForFreq(int freqInHz)
        {
            int i;
            for (i = 0; i < TotalBands; i++)
            {
                if (freqInHz <= FreqOfBand[i])
                {
                    break;
                }
            }
            return i;
        }

        public static int GetNumBandsForFreqRange(int freqMin, int freqMax)
        {
            int i, minBand = 0, maxBand = TotalBands;
            for (i = 0; i < TotalBands; i++)
            {
                if (freqMin <= FreqOfBand[i])
                {
                    minBand = i;
                    break;
                }
            }

            for (i = minBand; i < TotalBands; i++)
            {
                if (freqMax <= FreqOfBand[i])
                {
                    maxBand = i;
                    break;
                }
            }

            return maxBand - minBand;
        }
    }
}
