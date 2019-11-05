using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public static class Gate
    {
        public static bool[] Passed = new bool[Range.Count];

        static float tmpRangeAudio;
        static float max = 0;
        static float secondMax = 0;

        public static void Filter(int r)
        {
            if (r == 0) FilterRejectMax(r);
            else if (r == 1) FilterStandard(r);
            else if (r == 2) FilterStandard(r);
        }

        public static void FilterStandard(int r)
        {
            if (FFT.transformedData.Count() > Range.Ranges[r].HighCutAbsolute)
            {
                tmpRangeAudio = 0;
                for (int i = Range.Ranges[r].LowCutAbsolute; i < Range.Ranges[r].HighCutAbsolute; i++)
                {
                    tmpRangeAudio += FFT.transformedData[i];
                }
                Range.Ranges[r].Audio = tmpRangeAudio;
            }
        }

        public static void FilterRejectMax(int r)
        {
            if (FFT.transformedData.Count() > Range.Ranges[r].HighCutAbsolute)
            {
                tmpRangeAudio = 0;
                max = 0;
                for (int i = Range.Ranges[r].LowCutAbsolute; i < Range.Ranges[r].HighCutAbsolute; i++)
                {
                    tmpRangeAudio += FFT.transformedData[i];
                    if (FFT.transformedData[i] > max) max = FFT.transformedData[i];
                }
                Range.Ranges[r].Audio = tmpRangeAudio - max;
            }
        }

        public static void FilterRejectTop2(int r)
        {
            if (FFT.transformedData.Count() > Range.Ranges[r].HighCutAbsolute)
            {
                tmpRangeAudio = 0;
                max = 0;
                secondMax = 0;
                for (int i = Range.Ranges[r].LowCutAbsolute; i < Range.Ranges[r].HighCutAbsolute; i++)
                {
                    tmpRangeAudio += FFT.transformedData[i];
                    if (FFT.transformedData[i] > max)
                    {
                        secondMax = max;
                        max = FFT.transformedData[i];
                    }
                }
                Range.Ranges[r].Audio = tmpRangeAudio - max - secondMax;
            }
        }

        public static void FilterForMax(int r)
        {
            if (FFT.transformedData.Count() > Range.Ranges[r].HighCutAbsolute)
            {
                max = 0;
                for (int i = Range.Ranges[r].LowCutAbsolute; i < Range.Ranges[r].HighCutAbsolute; i++)
                {
                    if (FFT.transformedData[i] > max) max = FFT.transformedData[i];
                }
                Range.Ranges[r].Audio = max;
            }
        }

        public static int GetSub()
        {
            //return the index of the 'sub' (highest bar?)
            float max = Single.MinValue;
            int maxIndex = 0;
            for(int i = 0; i < Range.Ranges[0].NumBands; i ++ )
            {
                if(FFT.transformedData[i] > max)
                {
                    max = FFT.transformedData[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public static bool Pass(int r)
        {
            if (r == 0) return TransientPass(r);
            else if (r == 1) return TransientPass(r);
            else if (r == 2) return AllPass(r);
            else return false;
        }
        
        public static bool TransientPass(int r)
        {
            if(Range.Ranges[r].Audio > Range.Ranges[r].Threshold)
            {
                if (!Passed[r])
                {
                    Passed[r] = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Passed[r] = false;
                return false;
            }
        }

        public static bool AllPass(int r)
        {
            return Range.Ranges[r].Audio > Range.Ranges[r].Threshold ? true : false;
        }

        public static bool Subtract = false;
        public static int subtractor = 1, subtractFrom = 0;

        public static void ApplySubtraction(int r)
        {
            if (Subtract)
            {
                if (r == subtractFrom)
                {
                    Range.Ranges[r].Audio -= Range.Ranges[subtractor].Audio;
                }
            }
        }
    }
}
