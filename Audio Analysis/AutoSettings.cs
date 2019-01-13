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
            AutoSet.secondsToCollect = 5;
            AutoSet.bandwidth = 1;
            AutoSet.threshMultiplier = 0.75;

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
            for (int i = Math.Max((centerBandIndex - bandwidth / 2), 0); i < centerBandIndex + bandwidth / 2; i++)
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
        public static int centerBandIndex;
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

        public static int CenterFreqSelector(int minBandIndex, int maxBandIndex)
        {
            return HighestSingleChange(minBandIndex, maxBandIndex);
        }

        public static int HighestSingleChange(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double singleChange = 0;
            centerBandIndex = 0;
            for (int i = minBandIndex; i < maxBandIndex; i++)
            {
                double max = 0, changePerBand = 0;
                for (int j = 1; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }

                    if (changePerBand < Math.Abs(fftDataHistory[i][j] - fftDataHistory[i][j - 1]))
                    {
                        changePerBand = Math.Abs(fftDataHistory[i][j] - fftDataHistory[i][j - 1]);
                    }
                }
                if (changePerBand > singleChange)
                {
                    singleChange = changePerBand;
                    centerBandIndex = i;
                }
                if (max > highestPeak)
                {
                    highestPeak = max;
                }
            }
            return centerBandIndex;
        }

        //Good for bass.
        public static int HighestTotalChange(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double totalChange = 0;
            centerBandIndex = 0;
            for (int i = minBandIndex; i < maxBandIndex; i++)
            {
                double max = 0, change = 0;
                for (int j = 1; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }

                    change += Math.Abs(fftDataHistory[i][j] - fftDataHistory[i][j - 1]);
                }
                if (change > totalChange)
                {
                    totalChange = change;
                    centerBandIndex = i;
                }
                if (max > highestPeak)
                {
                    highestPeak = max;
                }
            }
            return centerBandIndex;
        }

        public static int HighestDynamicRange(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double peakToPeak = 0;
            centerBandIndex = 0;
            for (int i = minBandIndex; i < maxBandIndex; i++)
            {
                double max = 0, min = Int32.MaxValue;
                for (int j = 0; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }
                    else if(fftDataHistory[i][j] < min)
                    {
                        min = fftDataHistory[i][j];
                    }
                }
                if (max - min > peakToPeak)
                {
                    peakToPeak = max - min;
                    centerBandIndex = i;
                }
                if (max > highestPeak)
                {
                    highestPeak = max;
                }
            }
            return centerBandIndex;
        }

        public static int BestPeakToAverage(int minBandIndex, int maxBandIndex)
        {
            highestPeak = 0;
            double peakToAverageRatio = 0;
            centerBandIndex = 0;
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
                average = sum / fftDataHistory[i].Count;
                if (peak / average > peakToAverageRatio)
                {
                    peakToAverageRatio = peak / average;
                    centerBandIndex = i;
                }
                if (peak > highestPeak)
                {
                    highestPeak = peak;
                }
            }
            return centerBandIndex;
        }

        #endregion

    }
}
