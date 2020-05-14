using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveVisualizerAudioInput
{
    public class FFT
    {
        public static float[] transformedData;

        public static int N_FFT = (int)SoundCapture.FFTSize;
        public static float dropOffScale = .04f;
        public static bool DropOff = false;

        public static int[] chunk_freq = { 800, 1600, 3200, 6400, 12800, 30000 };
        public static int[] chunk_freq_jump = { 1, 2, 4, 6, 8, 10, 16 };

        static DateTime chkpoint1;
        static float[] lastData = null;

        public static float[] LogScale(float[] rawData)
        {
            int N2 = N_FFT / 2;
            float[] finalresult = new float[rawData.Length];
            int k = 1, transformedDataIndex = 0;
            float value = 0;

            int mappedFreq;
            for (int i = 0; i < N2; i += k)
            {
                value = 0;
                mappedFreq = i * AudioIn.RATE / N2 / 2;
                for (int l = 0; l < chunk_freq.Length; l++)
                {
                    if (mappedFreq < chunk_freq[l] || l == chunk_freq.Length - 1)
                    {
                        k = chunk_freq_jump[l];
                        break;
                    }
                }

                for (int j = i; j < i + k && j < N2; j++)
                {
                    value += rawData[j];
                }


                if (DropOff && lastData != null)
                {
                    lastData[transformedDataIndex] -= lastData[transformedDataIndex] * dropOffScale;
                    finalresult[transformedDataIndex] = value > lastData[transformedDataIndex] ? value : lastData[transformedDataIndex];
                }
                else
                {
                    finalresult[transformedDataIndex] = value;
                }

                    transformedDataIndex++;
            }

            Array.Resize(ref finalresult, transformedDataIndex);
            lastData = finalresult;

            return finalresult;
        }
    }
}
