using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;
using System;

public static class SoundCapture
{
    public static int minFreq = 0;
    public static int maxFreq = 20000;
    public static int barSpacing = 0;
    public static bool logScale = false;
    public static bool useAverage = false;

    public static float highScaleAverage = 2.0f;
    public static float highScaleNotAverage = 3.0f;

    static LineSpectrum lineSpectrum;

    static WasapiCapture capture;
    static WaveWriter writer;
    static FftSize fftSize;
    static float[] fftBuffer;

    static SingleBlockNotificationStream notificationSource;

    static BasicSpectrumProvider spectrumProvider;

    static IWaveSource finalSource;

    public static void Start()
    {

        // This uses the wasapi api to get any sound data played by the computer
        capture = new WasapiLoopbackCapture();

        capture.Initialize();

        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);


        // From https://github.com/filoe/cscore/blob/master/Samples/WinformsVisualization/Form1.cs

        // This is the typical size, you can change this for higher detail as needed
        fftSize = FftSize.Fft8192;

        // Actual fft data
        fftBuffer = new float[(int)fftSize];

        // These are the actual classes that give you spectrum data
        // The specific vars of lineSpectrum are changed below in the editor so most of these aren't that important here
        spectrumProvider = new BasicSpectrumProvider(capture.WaveFormat.Channels,
                    capture.WaveFormat.SampleRate, fftSize);

        lineSpectrum = new LineSpectrum(fftSize)
        {
            SpectrumProvider = spectrumProvider,
            UseAverage = useAverage,
            BarCount = AudioAnalyzer.FFT.N_FFTBuffer,
            BarSpacing = barSpacing,
            IsXLogScale = logScale,
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

    public static void OnApplicationQuit()
    {
        capture.Stop();
        capture.Dispose();
    }    

    public static float[] GetFFtData()
    {
        lineSpectrum.SpectrumProvider.GetFftData(fftBuffer, null);
        return lineSpectrum.GetSpectrumPoints(100.0f, fftBuffer);
    }

    public static float[] Update()
    {
        return GetFFtData();

    }
}
