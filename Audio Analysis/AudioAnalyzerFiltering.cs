using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public partial class AudioAnalyzerMDIForm
    {
        double tmpRangeAudio;

        void Filter(int r)
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

        bool Gate(int r)
        {
            return Ranges[r].Audio > Ranges[r].Threshold ? true : false;
        }

        bool Subtract = false;
        int subtractor = 1, subtractFrom = 0;

        public void ApplySubtraction(int r)
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
