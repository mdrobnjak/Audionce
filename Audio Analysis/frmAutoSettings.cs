using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class frmAutoSettings : Form
    {
        public static Range[] Ranges;

        public frmAutoSettings()
        {
            InitializeComponent();

            InitGV();

            this.gvAutoSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAutoSettings_CellValueChanged);
        }

        void InitGV()
        {
            int[] bandwidths = new int[Range.Count];
            double[] threshMults = new double[Range.Count];

            gvAutoSettings.Columns.Add("Range", "Range " + 1);

            //Per Range Rows
            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[0].HeaderCell.Value = "Bandwidth (Bands)";

            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[1].HeaderCell.Value = "Threshold Multiplier";
            
            for (int i = 0; i < Range.Count; i++)
            {
                gvAutoSettings.Columns.Add("Range", "Range " + (i + 1));
                gvAutoSettings.Rows[0].Cells[i].Value = Ranges[i].AutoSettings.Bandwidth;
                gvAutoSettings.Rows[1].Cells[i].Value = Ranges[i].AutoSettings.ThresholdMultiplier;
            }

            gvAutoSettings.Columns.RemoveAt(Range.Count);

            //Single Rows
            gvAutoSettings.Rows.Add(); //Spacer
            gvAutoSettings.Rows.Add();
            gvAutoSettings.Rows[3].HeaderCell.Value = "Seconds to Collect";
            gvAutoSettings.Rows[3].Cells[0].Value = AutoSettings.SecondsToCollect;

        }

        private void gvAutoSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object cellValue = gvAutoSettings.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                switch (e.RowIndex)
                {
                    case 0:
                        Ranges[e.ColumnIndex].AutoSettings.Bandwidth = Convert.ToInt32(cellValue);
                        break;
                    case 1:
                        Ranges[e.ColumnIndex].AutoSettings.ThresholdMultiplier = Convert.ToDouble(cellValue);
                        break;
                    case 3:
                        AutoSettings.SecondsToCollect = Convert.ToDouble(cellValue);
                        break;
                    default:
                        break;

                }
            }
            catch
            {
            }
        }


    }

    public class AutoSettings : Form
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
        public bool DynamicThreshold = false;//true;
        public double highestPeakNewCenter = 0;
        public double ThresholdMultiplier;

        public void ApplyAutoSettings()
        {
            if (Range.AutoSettings.DynamicThreshold) Range.Threshold = (int)(Range.GetMaxAudioFromLast200() * Range.AutoSettings.ThresholdMultiplier);
        }

        public void Threshold()
        {
            Range.Threshold = (int)(highestPeakNewCenter * ThresholdMultiplier);
        }

        public void HighestPeakNewBandwidth(out double highestPeakBandwidth)
        {
            highestPeakBandwidth = highestPeakNewCenter;
            for (int i = Math.Max((autoBandIndex - Bandwidth / 2), 0); i < autoBandIndex + Bandwidth / 2; i++)
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
        public int autoBandIndex;
        public static double SecondsToCollect = 5;
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
            Range.LowCutIndex = Math.Max(SingleChangePositive() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + Bandwidth;
            Threshold();
        }

        public void SnareSelector()
        {
            Range.LowCutIndex = Math.Max(Peak() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + Bandwidth;
            Threshold();
        }

        public void HatSelector()
        {
            Range.LowCutIndex = Math.Max(SingleChangePositive() - Bandwidth / 2, 0);
            Range.HighCutIndex = Range.LowCutIndex + 1 + Bandwidth;
            Threshold();
        }

        public int Peak()
        {
            highestPeakNewCenter = 0;
            autoBandIndex = 0;
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
                    autoBandIndex = i;
                }
                //Band Analysis:
                PassAlgorithmData(max);
            }
            return autoBandIndex;
        }

        public int SingleChange()
        {
            highestPeakNewCenter = 0;
            double singleChange = 0;
            autoBandIndex = 0;
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
                    autoBandIndex = i;
                    highestPeakNewCenter = max;
                }
                //Band Analysis:
                //PassAlgorithmData(changePerBand);
            }
            return autoBandIndex;
        }

        public int SingleChangePositive()
        {
            highestPeakNewCenter = 0;
            double singleChange = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                double max = 0, changePerBand = 0;
                for (int j = 1; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }

                    if (changePerBand < fftDataHistory[i][j] - fftDataHistory[i][j - 1])
                    {
                        changePerBand = fftDataHistory[i][j] - fftDataHistory[i][j - 1];
                    }
                }
                if (changePerBand > singleChange)
                {
                    singleChange = changePerBand;
                    autoBandIndex = i;
                    highestPeakNewCenter = max;
                }
                //Band Analysis:
                PassAlgorithmData(changePerBand);
            }
            return autoBandIndex;
        }

        /// <summary>
        /// Single Change Positive to Subsequent Average
        /// </summary>
        /// <returns></returns>
        public int SCPtoSA()
        {
            highestPeakNewCenter = 0;
            double scpToSa = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                double max = 0, scpPerBand = 0, subsequentAverage = 0, scpToSaPerBand = 0;
                int scpIndex = 0;
                for (int j = 1; j < fftDataHistory[i].Count(); j++)
                {
                    if (fftDataHistory[i][j] > max)
                    {
                        max = fftDataHistory[i][j];
                    }

                    if (scpPerBand < fftDataHistory[i][j] - fftDataHistory[i][j - 1])
                    {
                        scpPerBand = fftDataHistory[i][j] - fftDataHistory[i][j - 1];
                        scpIndex = j;
                    }
                }

                double sum = 0;
                int count = 0;
                for (int j = scpIndex; j < fftDataHistory[i].Count(); j++)
                {
                    sum += fftDataHistory[i][j];
                    count++;
                }

                subsequentAverage = sum / count;
                scpToSaPerBand = scpPerBand / subsequentAverage;

                if (scpToSaPerBand > scpToSa)
                {
                    scpToSa = scpPerBand;
                    autoBandIndex = i;
                    highestPeakNewCenter = max;
                }
                //Band Analysis:
                PassAlgorithmData(scpPerBand);
            }
            return autoBandIndex;
        }

        public int TotalChange()
        {
            highestPeakNewCenter = 0;
            double totalChange = 0;
            autoBandIndex = 0;
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
                    autoBandIndex = i;
                    highestPeakNewCenter = max;
                }
                //Band Analysis:
                PassAlgorithmData(change);
            }
            return autoBandIndex;
        }

        public int DynamicRange()
        {
            highestPeakNewCenter = 0;
            double peakToPeak = 0;
            autoBandIndex = 0;
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
                    autoBandIndex = i;
                    highestPeakNewCenter = max;
                }
                //Band Analysis:
                PassAlgorithmData(max - min);
            }
            return autoBandIndex;
        }

        public int PeakToAverage()
        {
            highestPeakNewCenter = 0;
            double peakToAverageRatio = 0;
            autoBandIndex = 0;
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
                    autoBandIndex = i;
                    highestPeakNewCenter = peak;
                }
                //Band Analysis:
                PassAlgorithmData(peak / average);
            }
            return autoBandIndex;
        }

        #endregion

        #region BandAnalysis

        Dictionary<string, List<double>> AlgorithmNamesAndDatas = null;

        void PassAlgorithmData(double data)
        {
            if (AlgorithmNamesAndDatas == null) return;

            AlgorithmNamesAndDatas[AlgorithmNamesAndDatas.Last().Key].Add(data);
        }

        public Dictionary<string, List<double>> DoBandAnalysis()
        {
            AlgorithmNamesAndDatas = new Dictionary<string, List<double>>();

            AlgorithmNamesAndDatas["Peak"] = new List<double>();
            Peak();

            AlgorithmNamesAndDatas["SingleChangePositive"] = new List<double>();
            SingleChangePositive();

            AlgorithmNamesAndDatas["TotalChange"] = new List<double>();
            TotalChange();

            AlgorithmNamesAndDatas["DynamicRange"] = new List<double>();
            DynamicRange();

            AlgorithmNamesAndDatas["PeakToAverage"] = new List<double>();
            PeakToAverage();

            Dictionary<string, List<double>> copy = new Dictionary<string, List<double>>(AlgorithmNamesAndDatas);
            AlgorithmNamesAndDatas = null;

            return copy;
        }

        #endregion

    }
}
