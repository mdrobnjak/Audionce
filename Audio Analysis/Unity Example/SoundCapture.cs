using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;
using System;

public static class SoundCapture
{
    // Use this for initialization

    public static int numBars = 4096;

    public static int minFreq = 1;
    public static int maxFreq = 22050;
    public static int barSpacing = 0;
    public static bool logScale = false;
    public static bool isAverage = false;

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
        fftSize = FftSize.Fft4096;

        // Actual fft data
        fftBuffer = new float[(int)fftSize];

        // These are the actual classes that give you spectrum data
        // The specific vars of lineSpectrum are changed below in the editor so most of these aren't that important here
        spectrumProvider = new BasicSpectrumProvider(capture.WaveFormat.Channels,
                    capture.WaveFormat.SampleRate, fftSize);

        lineSpectrum = new LineSpectrum(fftSize)
        {
            SpectrumProvider = spectrumProvider,
            UseAverage = false,
            BarCount = numBars,
            BarSpacing = 2,
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

    public static void OnApplicationQuit()
    {
        capture.Stop();
        capture.Dispose();
    }


    public static float[] barData = new float[numBars];

    public static float[] GetFFtData()
    {
        lock (barData)
        {
            lineSpectrum.BarCount = numBars;
            if (numBars != barData.Length)
            {
                barData = new float[numBars];
            }
        }

        if (spectrumProvider.IsNewDataAvailable || true)
        {
            lineSpectrum.MinimumFrequency = minFreq;
            lineSpectrum.MaximumFrequency = maxFreq;
            lineSpectrum.IsXLogScale = logScale;
            lineSpectrum.BarSpacing = barSpacing;
            lineSpectrum.SpectrumProvider.GetFftData(fftBuffer,null);
            return lineSpectrum.GetSpectrumPoints(100.0f, fftBuffer);
        }
        else
        {
            return new float[numBars];
        }
    }

    public static float[] Update()
    {

        int numBars = barData.Length;

        float[] resData = GetFFtData();

        return resData;

        if (resData == null)
        {
            return null;
        }

        lock (barData)
        {
            for (int i = 0; i < numBars && i < resData.Length; i++)
            {
                // Make the data between 0.0 and 1.0
                barData[i] = resData[i] / 100.0f;
            }

            for (int i = 0; i < numBars && i < resData.Length; i++)
            {
                if (lineSpectrum.UseAverage)
                {
                    // Scale the data because for some reason bass is always loud and treble is soft
                    barData[i] = (float)(barData[i] + highScaleAverage * Math.Sqrt(i / (numBars + 0.0f)) * barData[i]);
                }
                else
                {
                    barData[i] = (float)(barData[i] + highScaleNotAverage * Math.Sqrt(i / (numBars + 0.0f)) * barData[i]);
                }
            }
        }

    }
}
