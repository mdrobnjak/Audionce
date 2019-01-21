using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class frmAudioAnalyzer : Form
    {
        void InitAutoSettings()
        {
            btnDynamicThresholds_Click(null, null);

            AutoSettings.SecondsToCollect = 5;

            txtSeconds.Text = AutoSettings.SecondsToCollect.ToString();
            txtBandwidth1.Text = Ranges[0].AutoSettings.Bandwidth.ToString();
            txtBandwidth2.Text = Ranges[1].AutoSettings.Bandwidth.ToString();
            txtBandwidth3.Text = Ranges[2].AutoSettings.Bandwidth.ToString();

            txtThreshMultiplier1.Text = Ranges[0].AutoSettings.ThresholdMultiplier.ToString();
            txtThreshMultiplier2.Text = Ranges[1].AutoSettings.ThresholdMultiplier.ToString();
            txtThreshMultiplier3.Text = Ranges[2].AutoSettings.ThresholdMultiplier.ToString();
        }

        private void btnAutoRange_Click(object sender, EventArgs e)
        {
            AutoSettings.BeginRanging();
        }

        private void btnCommitAutoSettings_Click(object sender, EventArgs e)
        {
            int intVar;
            double dblVar;

            if (!Int32.TryParse(txtBandwidth1.Text, out intVar)) return;
            Ranges[0].AutoSettings.Bandwidth = intVar;
            if (!Int32.TryParse(txtBandwidth2.Text, out intVar)) return;
            Ranges[1].AutoSettings.Bandwidth = intVar;
            if (!Int32.TryParse(txtBandwidth3.Text, out intVar)) return;
            Ranges[2].AutoSettings.Bandwidth = intVar;

            if (!Double.TryParse(txtSeconds.Text, out dblVar)) return;
            AutoSettings.SecondsToCollect = dblVar;

            if (!Double.TryParse(txtThreshMultiplier1.Text, out dblVar)) return;
            Ranges[0].AutoSettings.ThresholdMultiplier = dblVar;
            if (!Double.TryParse(txtThreshMultiplier2.Text, out dblVar)) return;
            Ranges[1].AutoSettings.ThresholdMultiplier = dblVar;
            if (!Double.TryParse(txtThreshMultiplier3.Text, out dblVar)) return;
            Ranges[2].AutoSettings.ThresholdMultiplier = dblVar;
        }

        private void btnDynamicThresholds_Click(object sender, EventArgs e)
        {
            Ranges[0].AutoSettings.DynamicThreshold = !Ranges[0].AutoSettings.DynamicThreshold;
            Ranges[1].AutoSettings.DynamicThreshold = !Ranges[1].AutoSettings.DynamicThreshold;
            Ranges[2].AutoSettings.DynamicThreshold = !Ranges[2].AutoSettings.DynamicThreshold;

            if (Ranges[0].AutoSettings.DynamicThreshold) btnDynamicThresholds.BackColor = Color.LightGreen;
            else btnDynamicThresholds.BackColor = Color.Transparent;
        }
    }

    public class AutoSettings
    {
        public Range Range;

        public AutoSettings()
        {

        }

        public static void Reset()
        {
            if (fftDataHistory != null)
            {
                fftDataHistory.Clear();
                fftDataHistory = null;
            }
        }

        #region Threshold
        public bool DynamicThreshold = true;
        public double highestPeakNewCenter = 0;
        public double ThresholdMultiplier;

        public void ApplyAutoSettings()
        {
            if (Range.AutoSettings.DynamicThreshold) Range.Threshold = Range.GetMaxAudioFromLast200() * Range.AutoSettings.ThresholdMultiplier;
        }

        public void Threshold()
        {
            Range.Threshold = (int)(highestPeakNewCenter * ThresholdMultiplier);
        }

        public void HighestPeakNewBandwidth(out double highestPeakBandwidth)
        {
            highestPeakBandwidth = highestPeakNewCenter;
            for (int i = Math.Max((centerBandIndex - Bandwidth / 2), 0); i < centerBandIndex + Bandwidth / 2; i++)
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
        public static bool Ranging = false, ReadyToProcess = false;
        public int centerBandIndex;
        public static double SecondsToCollect;
        public int Bandwidth;
        public static DateTime started;

        public static void BeginRanging()
        {
            Ranging = true;
            started = DateTime.Now;
        }

        public static void CollectFFTData(double[] fftData)
        {
            if (fftDataHistory == null)
            {
                fftDataHistory = new List<List<double>>();
                for (int i = 0; i < fftData.Length; i++)
                {
                    fftDataHistory.Add(new List<double>());
                }
            }

            for (int i = 0; i < fftData.Length; i++)
            {
                fftDataHistory[i].Add(fftData[i]);
            }

            if ((DateTime.Now - started).TotalSeconds >= SecondsToCollect)
            {
                Ranging = false;
                ReadyToProcess = true;
            }
        }

        public void KickSelector()
        {
            Range.LowCutIndex = Math.Max(HighestSingleChange() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + Bandwidth;
            Threshold();
        }

        public void SnareSelector()
        {
            Range.LowCutIndex = Math.Max(HighestPeak() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + Bandwidth;
            Threshold();
        }

        public void HatSelector()
        {
            Range.LowCutIndex = Math.Max(HighestSingleChange() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + 1 + Bandwidth;
            Threshold();
        }

        public int HighestPeak()
        {
            highestPeakNewCenter = 0;
            centerBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
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
                //Band Analysis:
                PassAlgorithmData(max);
            }
            return centerBandIndex;
        }

        public int HighestSingleChange()
        {
            highestPeakNewCenter = 0;
            double singleChange = 0;
            centerBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
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
                //Band Analysis:
                PassAlgorithmData(changePerBand);
            }
            return centerBandIndex;
        }

        public int HighestTotalChange()
        {
            highestPeakNewCenter = 0;
            double totalChange = 0;
            centerBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
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
                //Band Analysis:
                PassAlgorithmData(change);
            }
            return centerBandIndex;
        }

        public int HighestDynamicRange()
        {
            highestPeakNewCenter = 0;
            double peakToPeak = 0;
            centerBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                double max = 0, min = Int32.MaxValue;
                for (int j = 0; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }
                    else if (fftDataHistory[i][j] < min)
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
                //Band Analysis:
                PassAlgorithmData(max - min);
            }
            return centerBandIndex;
        }

        public int BestPeakToAverage()
        {
            highestPeakNewCenter = 0;
            double peakToAverageRatio = 0;
            centerBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
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
                //Band Analysis:
                PassAlgorithmData(peak / average);
            }
            return centerBandIndex;
        }

        #endregion

        #region BandAnalysis

        Dictionary<string,List<double>> AlgorithmNamesAndDatas = null;

        void PassAlgorithmData(double data)
        {
            if (AlgorithmNamesAndDatas == null) return;
            
            AlgorithmNamesAndDatas[AlgorithmNamesAndDatas.Last().Key].Add(data);
        }

        public Dictionary<string, List<double>> DoBandAnalysis()
        {
            AlgorithmNamesAndDatas = new Dictionary<string, List<double>>();

            AlgorithmNamesAndDatas["HighestSingleChange"] = new List<double>();
            HighestSingleChange();

            AlgorithmNamesAndDatas["HighestTotalChange"] = new List<double>();
            HighestTotalChange();

            AlgorithmNamesAndDatas["BestPeakToAverage"] = new List<double>();
            BestPeakToAverage();

            AlgorithmNamesAndDatas["HgihestDynamicRange"] = new List<double>();
            HighestDynamicRange();

            AlgorithmNamesAndDatas["HighestPeak"] = new List<double>();
            HighestPeak();

            Dictionary<string, List<double>> copy = new Dictionary<string, List<double>>(AlgorithmNamesAndDatas);
            AlgorithmNamesAndDatas = null;

            return copy;
        }

        void ExportToExcel()
        {

        }

        #endregion

    }
}
