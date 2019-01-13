using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis : Form
    {
        public static int[,] trackbarLimits;

        public static void DefineTrackbarLimitsAndInitFullSpectrum()
        {
            Spectrum.InitSplitSpectrum(fmin, f1, f2, fmax);

            trackbarLimits = new int[numRanges, 2]
            {
                {Spectrum.bandsBefore[0], Spectrum.bandsBefore[0] + Spectrum.bandsPerRange[0]},
                {Spectrum.bandsBefore[1], Spectrum.bandsBefore[1] + Spectrum.bandsPerRange[1]},
                {Spectrum.bandsBefore[2], Spectrum.bandsBefore[2] + Spectrum.bandsPerRange[2]},
            };

            Spectrum.InitFullSpectrum();
        }

        Converter cvt;
        int converterScale = 15;

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

            if (Spectrum.drawSpectrum) DrawData(transformedData, gSpectrum, cvt);
        }

        private void DrawData(double[] data, Graphics g, Converter cvter)
        {
            if (data == null || data.Length == 0 || AudioIn.sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.bandsPerRange[selectedRange];
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = pnlSpectrum.Width / AudioIn.sourceData.Length;
            float value = 0;

            g.Clear(Color.White);

            int sx, sy;

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = Spectrum.bandsBefore[selectedRange];

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
            for (; bandIndexRelative < Spectrum.bandsPerRange[selectedRange] && bandIndexRelative < Spectrum.numBands; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            #endregion
        }


        private void DrawData_Original(double[] data, Graphics g, Converter cvter)
        {
            var chkpoint2 = DateTime.Now;
            if (data == null || data.Length == 0 || AudioIn.sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / Spectrum.bandsPerRange[selectedRange];
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = (float)pnlSpectrum.Width / (float)AudioIn.sourceData.Length;
            float value = 0;


            gTempBuffer.DrawImage(mainBuffer, new Rectangle(0, 0, tempBuffer.Width, tempBuffer.Height), 0, 0, mainBuffer.Width, mainBuffer.Height, GraphicsUnit.Pixel);

            //BitmapFilter.GaussianBlur(mainBuffer as Bitmap, 5);

            gMainBuffer.DrawImage(tempBuffer, new Rectangle(-2, -1, mainBuffer.Width + 4, mainBuffer.Height + 3), 0, 0, tempBuffer.Width, tempBuffer.Height, GraphicsUnit.Pixel, imgAttribute);


            int sx, sy;

            g.DrawImage(mainBuffer, new Point(0, 0));

            pnlSpectrum.Refresh();

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = Spectrum.bandsBefore[selectedRange];

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
            for (; bandIndexRelative < Spectrum.bandsPerRange[selectedRange] && bandIndexRelative < Spectrum.numBands; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            #endregion

            osdPanel.AddSet("Drawing delay(ms):", DateTime.Now.Subtract(chkpoint2).TotalMilliseconds.ToString());
        }

        #endregion

        const int fmin = 0, f1 = 300, f2 = 5000, fmax = 20000;

        private void btnSpectrumMode_Click(object sender, EventArgs e)
        {
            if (Spectrum.bandsBefore[2] > 0)
            {
                Spectrum.InitFullSpectrum();
            }
            else
            {
                Spectrum.InitSplitSpectrum(fmin, f1, f2, fmax);
            }
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

        public static int numBands;
        public static int[] bandsPerRange, bandsBefore;
        public static Dictionary<int, int> freqOfBand;

        public static void SyncBandsAndFreqs()
        {
            freqOfBand = new Dictionary<int, int>();
            if (FFT.rawFFT)
            {
                numBands = FFT.N_FFTBuffer;
                for (int i = 0; i < numBands; i++)
                {
                    freqOfBand[i] = i * AudioIn.RATE / FFT.N_FFT;
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
                    freqOfBand[n++] = mappedFreq;
                }
                numBands = n;
            }
        }

        public static int GetBandForFreq(int freqInHz)
        {
            int i;
            for (i = 0; i < numBands; i++)
            {
                if (freqInHz <= freqOfBand[i])
                {
                    break;
                }
            }
            return i;
        }

        public static int GetNumBandsForFreqRange(int freqMin, int freqMax)
        {
            int i, minBand = 0, maxBand = numBands;
            for (i = 0; i < numBands; i++)
            {
                if (freqMin <= freqOfBand[i])
                {
                    minBand = i;
                    break;
                }
            }

            for (i = minBand; i < numBands; i++)
            {
                if (freqMax <= freqOfBand[i])
                {
                    maxBand = i;
                    break;
                }
            }

            return maxBand - minBand;
        }


        public static void InitFullSpectrum()
        {
            bandsPerRange = new int[]
            {
                GetBandForFreq(22050), GetBandForFreq(22050), GetBandForFreq(22050)
            };
            bandsBefore = new int[]
            {
                0,0,0
            };
        }

        public static void InitSplitSpectrum(int freqMin, int freq1, int freq2, int freqMax)
        {
            bandsPerRange = new int[]
            {
                GetNumBandsForFreqRange(freqMin,freq1), GetNumBandsForFreqRange(freq1,freq2), GetNumBandsForFreqRange(freq2,freqMax)
            };

            bandsBefore = new int[]
            {
                0,bandsPerRange[0],bandsPerRange[0]+bandsPerRange[1]
            };
        }
    }
}
