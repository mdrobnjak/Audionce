using CSCore.DSP;
using System;
using System.Collections.Generic;

namespace WinformsVisualization.Visualization
{
    /// <summary>
    /// BasicSpectrumProvider
    /// </summary>
    public class SpectrumProvider : FftProvider
    {
        private readonly int _sampleRate;
        private readonly List<object> _contexts = new List<object>();

        public SpectrumProvider(int channels, int sampleRate, FftSize fftSize) 
            : base(channels, fftSize)
        {
            if(sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            _sampleRate = sampleRate;
        }

        public int GetFftBandIndex(float frequency)
        {
            int fftSize = (int) FftSize;
            double f = _sampleRate / 2.0;
            return (int)((frequency / f) * (fftSize));
        }

        public bool GetFftData(float[] fftResultBuffer, object context)
        {
            GetFftData(fftResultBuffer);
            return true;
        }

        public override void Add(float[] samples, int count)
        {
            base.Add(samples, count);
            if(count > 0)
                _contexts.Clear();
        }

        public override void Add(float left, float right)
        {
            base.Add(left, right);
            _contexts.Clear();
        }
    }
}