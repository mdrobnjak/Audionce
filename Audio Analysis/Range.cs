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
                        Bandwidth = frmSpectrum.TotalBands / 7,
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
        public int LowCutIndex
        {
            get
            {
                return frmSpectrum.Full ? frmSpectrum.GetBandForFreq(lowCutFreq) : Math.Max(0, frmSpectrum.GetBandForFreq(lowCutFreq) - NumBandsBefore);
            }
            set
            {
                lowCutFreq = frmSpectrum.Full ? frmSpectrum.FreqOfBand[value] : frmSpectrum.FreqOfBand[value + NumBandsBefore];
                LowCutAbsolute = frmSpectrum.Full ? value : value + NumBandsBefore;
            }
        }

        public int HighCutAbsolute { get; private set; }
        private int highCutFreq;
        public int HighCutIndex
        {
            get
            {
                return frmSpectrum.Full ? frmSpectrum.GetBandForFreq(highCutFreq) : Math.Max(0, frmSpectrum.GetBandForFreq(highCutFreq) - NumBandsBefore);
            }
            set
            {
                highCutFreq = frmSpectrum.Full ? frmSpectrum.FreqOfBand[value] : frmSpectrum.FreqOfBand[value + NumBandsBefore];
                HighCutAbsolute = frmSpectrum.Full ? value : value + NumBandsBefore;
            }
        }

        public int LowFreq;
        public int LowFreqIndex
        {
            get
            {
                return frmSpectrum.GetBandForFreq(LowFreq);
            }
        }

        public int HighFreq;
        public int HighFreqIndex
        {
            get
            {
                return frmSpectrum.GetBandForFreq(HighFreq);
            }
        }

        //public bool SignalPassed = false;

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
                return frmSpectrum.GetNumBandsForFreqRange(LowFreq, HighFreq);
            }
        }

        public int NumBandsBefore
        {
            get
            {
                return frmSpectrum.GetNumBandsForFreqRange(0, LowFreq);
            }
        }

        public Color Color;
    }
}
