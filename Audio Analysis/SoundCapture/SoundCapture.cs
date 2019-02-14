using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;
using System;

namespace AudioAnalyzer
{
    public static partial class AudioIn
    {
        public const int RATE = 48000;
    }
}

public static class SoundCapture
{
    public static int minFreq = 0;
    public static int maxFreq = 20000;
    public static bool logScale = false;
    public static bool useAverage = false;

    public static float highScaleAverage = 2.0f;
    public static float highScaleNotAverage = 3.0f;

    static LineSpectrum lineSpectrum;
        
    public static WasapiCapture Capture;
    public static FftSize FFTSize = FftSize.Fft2048;
    static float[] fftBuffer;

    static SpectrumProvider spectrumProvider;

    static IWaveSource finalSource;

    public static void Init()
    {

        // This uses the wasapi api to get any sound data played by the computer
        Capture = new WasapiLoopbackCapture(33);

        Capture.Initialize();

        // Get our capture as a source
        IWaveSource source = new SoundInSource(Capture);


        // From https://github.com/filoe/cscore/blob/master/Samples/WinformsVisualization/Form1.cs
        
        // Actual fft data
        fftBuffer = new float[(int)FFTSize];

        // These are the actual classes that give you spectrum data
        // The specific vars of lineSpectrum are changed below in the editor so most of these aren't that important here
        spectrumProvider = new SpectrumProvider(Capture.WaveFormat.Channels,
                    Capture.WaveFormat.SampleRate, FFTSize);

        lineSpectrum = new LineSpectrum(FFTSize)
        {
            SpectrumProvider = spectrumProvider,
            UseAverage = useAverage,
            IsXLogScale = logScale,
            ScalingStrategy = ScalingStrategy.Linear
        };

        // Tells us when data is available to send to our spectrum
        var notificationSource = new SingleBlockNotificationStream(source.ToSampleSource());

        notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;

        // We use this to request data so it actualy flows through (figuring this out took forever...)
        finalSource = notificationSource.ToWaveSource();

        Capture.DataAvailable += Capture_DataAvailable;
        Capture.Start();
    }

    private static void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
    {
        finalSource.Read(e.Data, e.Offset, e.ByteCount);
    }

    private static void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
    {
        spectrumProvider.Add(e.Left, e.Right);
    }

    public static void OnApplicationQuit()
    {
        Capture.Stop();
        Capture.Dispose();
    }    

    public static float[] GetFFtData()
    {
        lineSpectrum.SpectrumProvider.GetFftData(fftBuffer);
        return lineSpectrum.GetSpectrumPoints(100.0f, fftBuffer);
    }

    public static float[] Update()
    {
        return GetFFtData();

    }
}
