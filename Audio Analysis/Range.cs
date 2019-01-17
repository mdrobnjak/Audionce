using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public class Range
    {
        public Range()
        {

        }

        public static void Init(ref Range[] ranges)
        {
            ranges = new Range[Count];

            for (int i = 0; i < Count; i++)
            {
                SetActive(i);
                Active.AutoSettings.Range = Active;
                ranges[i] = Active;
            }
        }

        public static void SetActive(int i)
        {
            ActiveIndex = i;
        }

        public const int Count = 3;

        static int ActiveIndex;

        static Range[] Ranges = new Range[Count]
        {
            new Range()
            {
                FreqLo = 0, FreqHi = 300,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 1,
                    ThresholdMultiplier = 0.7
                },

                Color = Color.Pink
            },
            new Range()
            {
                FreqLo = 1000, FreqHi = 20000,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 10,
                    ThresholdMultiplier = 0.6
                },

                Color = Color.LightBlue
            },
            new Range()
            {
                FreqLo = 1000, FreqHi = 20000,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 5,
                    ThresholdMultiplier = 0.6
                },

                Color = Color.Gold
            }
        };

        public static ref Range Active
        {
            get { return ref Ranges[ActiveIndex]; }
        }

        private double threshold;
        public double Threshold
        {
            get
            {
                return threshold;
            }
            set
            {
                threshold = value;
            }
        }

        public int BandLo { get; set; }
        public int BandHi { get; set; }

        private int freqLo;
        public int FreqLo
        {
            get
            {
                return freqLo;
            }
            set
            {
                freqLo = value;
                BandLo = Spectrum.GetBandForFreq(value);
            }
        }

        private int freqHi;
        public int FreqHi
        {
            get
            {
                return freqHi;
            }
            set
            {
                freqHi = value;
                BandHi = Spectrum.GetBandForFreq(value);
            }
        }

        //NewAudio needs renaming
        private List<double> newAudios = new List<double>();
        private double newAudio;
        public double NewAudio
        {
            get
            {
                return newAudio;
            }
            set
            {
                newAudio = value;

                newAudios.Add(value);
                if (newAudios.Count > 199) newAudios.RemoveAt(0);
            }
        }

        public double GetMaxFromLast()
        {
            return newAudios.Max();
        }


        public AutoSettings AutoSettings;

        public int NumBands;
        public int NumBandsBefore;

        public Color Color;
    }
}
