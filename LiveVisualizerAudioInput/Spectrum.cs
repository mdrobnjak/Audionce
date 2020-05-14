using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveVisualizerAudioInput
{
    class Spectrum
    {

        public static bool Full = true;

        public static int TotalBands;
        public static int DisplayBands
        {
            get
            {
                return Full ? TotalBands : Range.Active.NumBands;
            }
        }

        public static Dictionary<int, int> FreqOfBand;

        public static void SyncBandsAndFreqs()
        {
            FreqOfBand = new Dictionary<int, int>();
            int k = 1, n = 0;
            int N2 = FFT.N_FFT / 2;
            int mappedFreq;
            for (int i = 0; i < N2; i += k)
            {
                mappedFreq = i * AudioIn.RATE / N2 / 2;

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

        public static int GetBandForFreq(int freqInHz)
        {
            int i;
            for (i = 0; i < FreqOfBand.Last().Key; i++)
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
