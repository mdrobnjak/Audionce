using AudioAnalysis.External;
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
    public class Spectrum : System.Windows.Forms.Panel
    {
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
