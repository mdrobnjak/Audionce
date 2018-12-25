using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public static class AutoSettings
    {

        public static double highestPeak = 0;
        public static int AutoThreshold()
        {
            return (int)(highestPeak * 0.75);
        }

        private static List<List<double>> fftDataHistory = null;
        public static bool autoRanging = false, readyToProcess = false;
        public static int startBand;
        public static double secondsToCollect = 2.0;

        public static void BeginAutoRange()
        {
            autoRanging = true;
        }

        public static void CollectFFTData(int minBandIndex, int maxBandIndex, double[] fftData)
        {
            if (fftDataHistory == null)
            {
                fftDataHistory = new List<List<double>>();
                for (int i = minBandIndex; i <= maxBandIndex; i++)
                {
                    fftDataHistory.Add(new List<double>());                    
                }
            }

            for (int i = 0; i < maxBandIndex - minBandIndex; i++)
            {
                fftDataHistory[i].Add(fftData[i]);
            }

            secondsToCollect -= 0.01;

            if (secondsToCollect <= 0.0)
            {
                autoRanging = false;
                readyToProcess = true;
            }
        }

        public static int MostDynamic(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double peakToAverageRatio = 0;
            int mostDynamicIndex = 0;
            for (int i = minBandIndex; i < maxBandIndex; i++)
            {
                double peak = 0, sum = 0, average = 0;
                for (int j = 0; j < fftDataHistory[i].Count(); j++)
                {
                    sum += fftDataHistory[i][j];
                    if (fftDataHistory[i][j] > peak)
                    {
                        peak = fftDataHistory[i][j];
                    }
                }
                average = sum / fftDataHistory[i].Count();
                if (peak / average > peakToAverageRatio)
                {
                    peakToAverageRatio = peak / average;
                    highestPeak = peak;
                    mostDynamicIndex = i;
                }
            }
            return mostDynamicIndex;
        }

        public static void Reset()
        {
            if (fftDataHistory != null)
            {
                fftDataHistory.Clear();
                fftDataHistory = null;
            }
            secondsToCollect = 2.0;
            readyToProcess = false;
            highestPeak = 0;
        }
    }
}
