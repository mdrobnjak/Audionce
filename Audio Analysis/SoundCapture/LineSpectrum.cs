using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CSCore.DSP;

namespace WinformsVisualization.Visualization
{
    public class LineSpectrum : SpectrumBase
    {
        public LineSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
        }

        public float[] GetSpectrumPoints(float height, float[] fftBuffer)
        {
            SpectrumPointData[] dats = CalculateSpectrumPoints(height, fftBuffer);
            float[] res = new float[dats.Length];
            for (int i = 0; i < dats.Length; i++)
            {
                res[i] = (float)dats[i].Value;
            }

            return res;
        }

        protected override void UpdateFrequencyMapping()
        {
            base.UpdateFrequencyMapping();
        }
    }
}