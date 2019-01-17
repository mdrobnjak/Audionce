using System;
using System.Collections.Generic;
using System.Drawing;
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
            btnDynamicThresholds_Click(null,null);

            AutoSet.secondsToCollect = 5;
            AutoSet.bandwidth = 1;
            AutoSet.threshMultipliers = new double[3] { 0.7, 0.6, 0.6 };

            txtSeconds.Text = AutoSet.secondsToCollect.ToString();
            txtBandwidth.Text = AutoSet.bandwidth.ToString();

            txtThreshMultiplier1.Text = AutoSet.threshMultipliers[0].ToString();
            txtThreshMultiplier2.Text = AutoSet.threshMultipliers[1].ToString();
            txtThreshMultiplier3.Text = AutoSet.threshMultipliers[2].ToString();
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
            if (!Double.TryParse(txtThreshMultiplier1.Text, out dblVar)) return;
            AutoSet.threshMultipliers[0] = dblVar;
            if (!Double.TryParse(txtThreshMultiplier2.Text, out dblVar)) return;
            AutoSet.threshMultipliers[1] = dblVar;
            if (!Double.TryParse(txtThreshMultiplier3.Text, out dblVar)) return;
            AutoSet.threshMultipliers[2] = dblVar;
        }

        private void btnDynamicThresholds_Click(object sender, EventArgs e)
        {
            AutoSet.dynamicThresholds = !AutoSet.dynamicThresholds;
            if(AutoSet.dynamicThresholds) btnDynamicThresholds.BackColor = Color.LightGreen;
            else btnDynamicThresholds.BackColor = Color.Transparent;
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
            highestPeakNewCenter = 0;
        }

        #region Threshold
        public static bool dynamicThresholds = false;
        public static double highestPeakNewCenter = 0;
        public static double[] threshMultipliers;

        public static int Threshold(int rangeIndex)
        {
            return (int)(highestPeakNewCenter * threshMultipliers[rangeIndex]);
        }

        public static void HighestPeakNewBandwidth(out double highestPeakBandwidth)
        {
            highestPeakBandwidth = highestPeakNewCenter;
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

        public static int BassFreqSelector(int minBandIndex, int maxBandIndex)
        {
            return HighestSingleChange(minBandIndex, maxBandIndex);
        }

        public static int SnareFreqSelector(int minBandIndex, int maxBandIndex)
        {
            return HighestPeak(minBandIndex, maxBandIndex);
        }

        public static int HatFreqSelector(int minBandIndex, int maxBandIndex)
        {
            return HighestSingleChange(minBandIndex, maxBandIndex);
        }

        public static int HighestPeak(int minBandIndex, int maxBandIndex)
        {
            highestPeakNewCenter = 0;
            centerBandIndex = 0;
            for (int i = minBandIndex; i < maxBandIndex; i++)
            {
                double max = 0;
                for (int j = 1; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }                    
                }
                if (max > highestPeakNewCenter)
                {
                    highestPeakNewCenter = max;
                    centerBandIndex = i;
                }
            }
            return centerBandIndex;
        }

        public static int HighestSingleChange(int minBandIndex, int maxBandIndex)
        {
            highestPeakNewCenter = 0;
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
                    highestPeakNewCenter = max;
                }
            }
            return centerBandIndex;
        }        

        public static int HighestTotalChange(int minBandIndex, int maxBandIndex)
        {
            highestPeakNewCenter = 0;
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
                    highestPeakNewCenter = max;
                }
            }
            return centerBandIndex;
        }

        public static int HighestDynamicRange(int minBandIndex, int maxBandIndex)
        {
            highestPeakNewCenter = 0;
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
                    highestPeakNewCenter = max;
                }
            }
            return centerBandIndex;
        }

        public static int BestPeakToAverage(int minBandIndex, int maxBandIndex)
        {
            highestPeakNewCenter = 0;
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
                    highestPeakNewCenter = peak;
                }
            }
            return centerBandIndex;
        }

        #endregion

    }
}
