using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    class Range
    {
        public static int Count { get; set; }

        public static int Active
        {
            get { return Active; }
            set { if (value >= 0 && value < Count) Active = value; }
        }

        double Threshold { get; set; }
        int Low { get; set; }
        int High { get; set; }

        BeatDetector beatDetector;

        
    }
}
