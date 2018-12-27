using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public static class AutoSet
    {

        public static double highestPeak = 0, highestPeakBandwidth = 0;
        public static int Threshold(double max = 0)
        {
            if(max == 0)
            {
                HighestPeakBandwidth(out max);
            }
            return (int)(max * 0.75);
        }

        private static List<List<double>> fftDataHistory = null;
        public static bool ranging = false, readyToProcess = false;
        public static int startBand;
        public static int mostDynamicIndex;
        public static double secondsToCollect = 2.0;
        public static int bandwidth = 6;

        public static void BeginRanging()
        {
            ranging = true;
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
                ranging = false;
                readyToProcess = true;
            }
        }

        public static int MostDynamic(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double peakToAverageRatio = 0;
            mostDynamicIndex = 0;
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

        public static void HighestPeakBandwidth(out double highestPeakBandwidth)
        {
            highestPeakBandwidth = highestPeak;
            for (int i = Math.Max((mostDynamicIndex - bandwidth / 2), 0); i < mostDynamicIndex + bandwidth / 2; i++)
            {
                for (int j = 0; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > highestPeakBandwidth)
                        highestPeakBandwidth = fftDataHistory[i][j];
                }
            }
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
            highestPeak = highestPeakBandwidth = 0;
        }
    }
}
