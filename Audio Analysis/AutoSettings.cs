using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{

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
        public bool DynamicThreshold = false;
        public float highestPeakNewCenter = 0;
        public float ThresholdMultiplier;

        public void ApplyAutoSettings()
        {
            if (Range.AutoSettings.DynamicThreshold) Range.Threshold = (int)(Range.GetMaxAudioFromLast200() * Range.AutoSettings.ThresholdMultiplier);
        }

        public void Threshold()
        {
            Range.Threshold = (int)(highestPeakNewCenter * ThresholdMultiplier);
        }

        public void HighestPeakNewBandwidth(out float highestPeakBandwidth)
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
        private static List<List<float>> fftDataHistory = null;
        public static bool Ranging = false, ReadyToProcess = false;
        public int autoBandIndex;
        public static float SecondsToCollect = 5;
        public int Bandwidth;
        public static DateTime started;

        public static void BeginRanging()
        {
            Ranging = true;
            started = DateTime.Now;
        }

        public static void CollectFFTData(float[] fftData)
        {
            if (fftDataHistory == null)
            {
                fftDataHistory = new List<List<float>>();
                for (int i = 0; i < fftData.Length; i++)
                {
                    fftDataHistory.Add(new List<float>());
                }
            }

            for (int i = 0; i < fftData.Length; i++)
            {
                fftDataHistory[i].Add(fftData[i]);
            }

            if ((DateTime.Now - started).TotalSeconds >= SecondsToCollect)
            {
                List<List<float>> trainingData = new List<List<float>>();

                for (int i = Range.Active.LowFreqIndex; i < Range.Active.HighFreqIndex; i++)
                {
                    trainingData.Add(fftDataHistory[i]);
                }

                Task.Run(()=>BandAnalysis.CreateTrainingRowFromAudioData(trainingData));

                Ranging = false;
                ReadyToProcess = true;
            }
        }

        public void KickSelector()
        {
            Range.LowCutAbsolute = Math.Max(SingleChangePositive() - Bandwidth / 2, 0);
            Range.HighCutAbsolute = Range.LowCutAbsolute + Bandwidth;
            Threshold();
        }

        public void SnareSelector()
        {
            Range.LowCutAbsolute = Math.Max(SingleChangePositive() - Bandwidth / 2, 0);
            Range.HighCutAbsolute = Range.LowCutAbsolute + Bandwidth;
            Threshold();
        }

        public void HatSelector()
        {
            Range.LowCutAbsolute = Math.Max(SingleChangePositive() - Bandwidth / 2, 0);
            Range.HighCutAbsolute = Range.LowCutAbsolute + Bandwidth;
            Threshold();
        }

        public int Peak()
        {
            highestPeakNewCenter = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0;
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
            }
            return autoBandIndex;
        }

        public int SingleChange()
        {
            highestPeakNewCenter = 0;
            float singleChange = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0, changePerBand = 0;
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
            }
            return autoBandIndex;
        }

        public int SingleChangePositive()
        {
            highestPeakNewCenter = 0;
            float singleChange = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0, changePerBand = 0;
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
            float scpToSa = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0, scpPerBand = 0, subsequentAverage = 0, scpToSaPerBand = 0;
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

                float sum = 0;
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
            }
            return autoBandIndex;
        }

        public int TotalChange()
        {
            highestPeakNewCenter = 0;
            float totalChange = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0, change = 0;
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
            }
            return autoBandIndex;
        }

        public int DynamicRange()
        {
            highestPeakNewCenter = 0;
            float peakToPeak = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float max = 0, min = Int32.MaxValue;
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
            }
            return autoBandIndex;
        }

        public int PeakToAverage()
        {
            highestPeakNewCenter = 0;
            float peakToAverageRatio = 0;
            autoBandIndex = 0;
            for (int i = Range.LowFreqIndex; i < Range.HighFreqIndex; i++)
            {
                float peak = 0, sum = 0, average = 0;
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
            }
            return autoBandIndex;
        }

        #endregion
    }
}
