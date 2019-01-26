using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public class Range
    {
        public Range()
        {

        }

        public static void Init(ref Range[] ranges)
        {
            ranges = new Range[Count];

            for (int i = Count - 1; i >= 0; i--)
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

        int Index;

        static int ActiveIndex;

        static Range[] Ranges = new Range[Count]
        {
                new Range()
                {
                    Index = 0,

                    LowFreq = 0, HighFreq = 210,

                    AutoSettings = new AutoSettings()
                    {
                        Bandwidth = 1,
                        ThresholdMultiplier = 0.6
                    },

                    Color = Color.Pink
                },
                new Range()
                {
                    Index = 1,

                    LowFreq = 200, HighFreq = 1200,

                    AutoSettings = new AutoSettings()
                    {
                        Bandwidth = 1,
                        ThresholdMultiplier = 0.6
                    },

                    Color = Color.LightBlue
                },
                new Range()
                {
                    Index = 2,

                    LowFreq = 1000, HighFreq = 20000,

                    AutoSettings = new AutoSettings()
                    {
                        Bandwidth = Spectrum.TotalBands / 7,
                        ThresholdMultiplier = 0.6
                    },

                    Color = Color.Gold
                }
        };

        public static ref Range Active
        {
            get { return ref Ranges[ActiveIndex]; }
        }

        public int Threshold { get; set; }

        public int LowCutAbsolute { get; private set; }
        private int lowCutFreq;
        public int LowCutFreq
        {
            get
            {
                return lowCutFreq;
            }
            set
            {
                lowCutFreq = value;
                LowCutAbsolute = Spectrum.GetBandForFreq(value);
            }
        }
        public int LowCutIndex
        {
            get
            {
                return Spectrum.Full ? Spectrum.GetBandForFreq(lowCutFreq) : Math.Max(0, Spectrum.GetBandForFreq(lowCutFreq) - NumBandsBefore);
            }
            set
            {
                lowCutFreq = Spectrum.Full ? Spectrum.FreqOfBand[value] : Spectrum.FreqOfBand[value + NumBandsBefore];
                LowCutAbsolute = Spectrum.Full ? value : value + NumBandsBefore;
            }
        }

        public int HighCutAbsolute { get; private set; }
        private int highCutFreq;
        public int HighCutFreq
        {
            get
            {
                return highCutFreq;
            }
            set
            {
                highCutFreq = value;
                HighCutAbsolute = Spectrum.GetBandForFreq(value);
            }
        }
        public int HighCutIndex
        {
            get
            {
                return Spectrum.Full ? Spectrum.GetBandForFreq(highCutFreq) : Math.Max(0, Spectrum.GetBandForFreq(highCutFreq) - NumBandsBefore);
            }
            set
            {
                highCutFreq = Spectrum.Full ? Spectrum.FreqOfBand[value] : Spectrum.FreqOfBand[value + NumBandsBefore];
                HighCutAbsolute = Spectrum.Full ? value : value + NumBandsBefore;
            }
        }

        public int LowFreq;
        public int LowFreqIndex
        {
            get
            {
                return Spectrum.GetBandForFreq(LowFreq);
            }
        }

        public int HighFreq;
        public int HighFreqIndex
        {
            get
            {
                return Spectrum.GetBandForFreq(HighFreq);
            }
        }

        private List<double> audios = new List<double>();
        private double audio;
        public double Audio
        {
            get
            {
                return audio;
            }
            set
            {
                audio = value;

                audios.Add(value);
                if (audios.Count > 199) audios.RemoveAt(0);
            }
        }

        public double GetMaxAudioFromLast200()
        {
            return audios.Max();
        }

        public AutoSettings AutoSettings;

        public int NumBands
        {
            get
            {
                return Spectrum.GetNumBandsForFreqRange(LowFreq, HighFreq);
            }
        }

        public int NumBandsBefore
        {
            get
            {
                return Spectrum.GetNumBandsForFreqRange(0, LowFreq);
            }
        }

        public Color Color;
    }
}
