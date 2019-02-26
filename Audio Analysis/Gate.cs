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

        public static bool Pass(int r)
        {
            if (r == 0) return TransientPass(r);
            else if (r == 1) return AllPass(r);
            else if (r == 2) return true;
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
