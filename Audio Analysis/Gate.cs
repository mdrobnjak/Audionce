using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public static class Gate
    {
        public static Range[] Ranges;

        public static bool[] Passed = new bool[Range.Count];

        static float tmpRangeAudio;

        public static void Filter(int r)
        {
            if (FFT.transformedData.Count() > Ranges[r].HighCutAbsolute)
            {
                tmpRangeAudio = 0;
                for (int i = Ranges[r].LowCutAbsolute; i < Ranges[r].HighCutAbsolute; i++)
                {
                    tmpRangeAudio += FFT.transformedData[i];
                }
                Ranges[r].Audio = tmpRangeAudio;
            }
        }

        public static bool TransientPass(int r)
        {
            if(Ranges[r].Audio > Ranges[r].Threshold)
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

        public static bool Pass(int r)
        {
            return Ranges[r].Audio > Ranges[r].Threshold ? true : false;
        }

        public static bool Subtract = false;
        public static int subtractor = 1, subtractFrom = 0;

        public static void ApplySubtraction(int r)
        {
            if (Subtract)
            {
                if (r == subtractFrom)
                {
                    Ranges[r].Audio -= Ranges[subtractor].Audio;
                }
            }
        }
    }
}
