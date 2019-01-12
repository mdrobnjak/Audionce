using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis : Form
    {
        void InitAutoSettings()
        {
            AutoSet.secondsToCollect = 10;
            AutoSet.bandwidth = 10;
            AutoSet.threshMultiplier = 0.5;

            txtSeconds.Text = AutoSet.secondsToCollect.ToString();
            txtBandwidth.Text = AutoSet.bandwidth.ToString();
            txtThreshMultiplier.Text = AutoSet.threshMultiplier.ToString();
        }

        private void btnAutoRange_Click(object sender, EventArgs e)
        {
            AutoSet.BeginRanging();
        }

        private void btnCommitAutoSettings_Click(object sender, EventArgs e)
        {
            int intVar;
            double dblVar;

            if (!Int32.TryParse(txtBandwidth.Text, out intVar)) return;
            AutoSet.bandwidth = intVar;
            if (!Double.TryParse(txtSeconds.Text, out dblVar)) return;
            AutoSet.secondsToCollect = dblVar;
            if (!Double.TryParse(txtThreshMultiplier.Text, out dblVar)) return;
            AutoSet.threshMultiplier = dblVar;
        }
    }

    public static class AutoSet
    {
        public static void Reset()
        {
            if (fftDataHistory != null)
            {
                fftDataHistory.Clear();
                fftDataHistory = null;
            }
            readyToProcess = false;
            highestPeak = highestPeakBandwidth = 0;
        }

        #region Threshold
        public static double highestPeak = 0, highestPeakBandwidth = 0, threshMultiplier;

        public static int Threshold(double max = 0)
        {
            if (max == 0)
            {
                HighestPeakBandwidth(out max);
            }
            return (int)(max * threshMultiplier);
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
        #endregion

        #region Range
        private static List<List<double>> fftDataHistory = null;
        public static bool ranging = false, readyToProcess = false;
        public static int mostDynamicIndex;
        public static double secondsToCollect;
        public static int bandwidth;
        public static DateTime started;

        public static void BeginRanging()
        {
            ranging = true;
            started = DateTime.Now;
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

            if ((DateTime.Now - started).TotalSeconds >= secondsToCollect)
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
                    mostDynamicIndex = i;
                }
                if (peak > highestPeak)
                {
                    highestPeak = peak;
                }
            }
            return mostDynamicIndex;
        }
        #endregion

    }
}
