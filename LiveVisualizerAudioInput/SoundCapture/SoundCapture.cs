using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LiveVisualizerAudioInput
{
    public static partial class AudioIn
    {
        public const int RATE = 48000;
    }
}

/// <summary>
/// This module is a black box.
/// </summary>
public static class SoundCapture
{
    private static WasapiCapture capture;
    private static IWaveSource finalSource;    
    
    public static readonly FftSize FFTSize = FftSize.Fft8192;
    private static float[] fftBuffer;

    private static LineSpectrum lineSpectrum;
    private static SpectrumProvider spectrumProvider;

    public static void Init()
    {
        capture = new WasapiLoopbackCapture(0);
        capture.Initialize();
        
        IWaveSource source = new SoundInSource(capture);

        fftBuffer = new float[(int)FFTSize];
        
        spectrumProvider = new SpectrumProvider(capture.WaveFormat.Channels,
                    capture.WaveFormat.SampleRate, FFTSize);

        lineSpectrum = new LineSpectrum(FFTSize)
        {
            SpectrumProvider = spectrumProvider,
            UseAverage = false,
            IsXLogScale = false,
            ScalingStrategy = ScalingStrategy.Linear
        };

        // Tells us when data is available to send to our spectrum
        var notificationSource = new SingleBlockNotificationStream(source.ToSampleSource());

        notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;

        // We use this to request data so it actualy flows through (figuring this out took forever...)
        finalSource = notificationSource.ToWaveSource();

        capture.DataAvailable += Capture_DataAvailable;
        capture.Start();
    }
    
    private static void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
    {
        finalSource.Read(e.Data, e.Offset, e.ByteCount);
    }

    private static void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
    {
        spectrumProvider.Add(e.Left, e.Right);
    }
    
    public static void StopAndDisposeResources()
    {
        capture.Stop();
        capture.Dispose();
    }

    public static float[] GetFFTData()
    {
        lineSpectrum.SpectrumProvider.GetFftData(fftBuffer);
        return lineSpectrum.GetSpectrumPoints(100.0f, fftBuffer);
    }
}
