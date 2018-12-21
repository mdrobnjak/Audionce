using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioAnalysis.External;
using NAudio.Wave;

namespace AudioAnalysis
{
    public partial class NovarStrikerVisualizer : Form
    {
        Converter cvt;

        public NovarStrikerVisualizer()
        {
            InitializeComponent();
        }

        private void NovarStrikerVisualizer_Load(object sender, EventArgs e)
        {
            InitBufferAndGraphic();


            //Force high priority if needed
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            InitSoundCapture();

            InitCoordinator();

            #region just to try in a another manner
            //if (t != null)
            //    if (t.ThreadState == System.Threading.ThreadState.Running)
            //        t.Abort();
            //t = new Thread(new ThreadStart(delegate()
            //{
            //    //Thread.Sleep(1000);
            //    while (true)
            //    {
            //        // Process FFT
            //        // Force GUI update by raising paint
            //    }
            //}));
            //t.IsBackground = true;
            //t.Start();
            #endregion
            
        }

        #region Effect related variables

        Image mainBuffer;
        Graphics gMainBuffer;

        Image tempBuffer;
        Graphics gTempBuffer;

        ColorMatrix colormatrix = new ColorMatrix(new float[][]
                        {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                          //  new float[]{0, 0, 0, -0.001f, 1},
                            new float[]{-0.01f, -0.01f, -0.01f, 0, 0}
                          //  new float[]{0, 0, 0, 0, 1}
                        });
        ImageAttributes imgAttribute;

        #endregion 

        private void InitBufferAndGraphic()
        {
            //mainBuffer is a new Bitmap.
            //What is panel1?
            //What is the PixelFormat?
            mainBuffer = new Bitmap(panel1.Width, panel1.Height, PixelFormat.Format32bppArgb);
            //gMainBuffer is a new Graphics made from mainBuffer.
            gMainBuffer = Graphics.FromImage(mainBuffer);
            //CompositingQuality is set to HighSpeed.
            gMainBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            //InterpolationMode is set to Low.
            gMainBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            //tempBuffer is a new Bitmap.
            tempBuffer = new Bitmap(mainBuffer.Width, mainBuffer.Height);
            //gTempBuffer is a new Graphics made from tempBuffer.
            gTempBuffer = Graphics.FromImage(tempBuffer);
            //imgAttribute is a new ImageAttributes.
            imgAttribute = new ImageAttributes();
            //Set imgAttribute's color adjustment matrix.
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            //CompositingQuality is set to HighSpeed.
            gTempBuffer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            //InterpolationMode is set to NearestNeighbor.
            gTempBuffer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //Constants is initialized.
            //(3 SolidBrushes are initialized.)
            Constants.Init();
        }

        private const int samplingFrequency = 44100;
        Node dataList = new Node(new ComplexNumber(0, 0));
        Node endingNode;
        WaveIn waveInStream;

        #region Settings
        public int Mode
        {
            get;
            set;
        }
        public double MasterScaleFFT
        {
            get;
            set;
        }
        public double DropOffScale
        {
            get;
            set;
        }
        public int N_FFT
        {
            get;
            set;
        }
        #endregion

        private void InitSoundCapture()
        {
            //Set endingNode equal to dataList.
            //What is a node?
            endingNode = dataList;

            //Prepare a Wave input device for recording.
            waveInStream = new WaveIn();
            //Set NumberOfBuffers to 5.
            waveInStream.NumberOfBuffers = 5;
            //Set BufferMilliseconds to 10.
            waveInStream.BufferMilliseconds = 10;
            //Create a new 16 bit Wave format with sample rate = samplingFrequency and channel count = 1
            waveInStream.WaveFormat = new WaveFormat(samplingFrequency, 1);
            //Create new EventHandler for when data is available to the Wave input device.
            waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(waveInStream_DataAvailable);
            //Start recording to the Wave input device.
            waveInStream.StartRecording();

            //Set Mode to 1.
            Mode = 1;
            //Set MasterScaleFFT to 1.
            MasterScaleFFT = 1;
            //Set DropOffScale to 0.4.
            DropOffScale = 0.4;
            //Set N_FFT to 2048.
            N_FFT = 2048 / 2;

            //Init BeatDetector with an evaluateLength of 50.
            //BeatDetector.InitDetector(50);
        }

        double[] sourceData;

        private void waveInStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (sourceData == null)
                sourceData = new double[e.BytesRecorded / 2];

            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sampleL = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i + 0]);
                //  short sampleR = (short)((e.Buffer[i + 1+2] << 8) | e.Buffer[i + 2]);
                double sample32 = (sampleL) / 32722d;
                sourceData[i / 2] = sample32;// (double)(e.Buffer[i]) / 255;
            }

            AppendData(sourceData);
        }

        int distance2Node = 0;

        private void AppendData(double[] newData)
        {
            int N = 10000;
            //double[] data = new double[N];

            var prevNode = dataList;
            var shiftNode = dataList;


            for (int j = 0; j < newData.Length; j++)
            {
                endingNode.NextNode = new Node(new ComplexNumber(newData[j], 0));
                endingNode.NextNode.PrevNode = endingNode;
                endingNode = endingNode.NextNode;
                if (j == newData.Length - 1)
                    endingNode.isEndPoint = true;
                // data[thresholdCounter] = runningNode.Value;
                distance2Node++;
            }
            if (distance2Node > N)
            {
                for (int j = 0; j < newData.Length; j++)
                {
                    dataList = dataList.NextNode;
                }
                dataList.isStartPoint = true;
                dataList.PrevNode = null;
                distance2Node = distance2Node - newData.Length;
            }

        }

        private void InitCoordinator()
        {
            //Set baseScale to 4096/N_FFT.
            double baseScale = 4096d / (double)N_FFT;
            //Set Converter cvt to a new Converter
            cvt = new Converter(0, panel1.Location.Y + panel1.Height / 2, 1, baseScale);
        }

        double[] transformedData;
        bool paintInitiated = false;
        bool drawing = false;
        Font pen;
        OSD osdPanel = new OSD();

        private void NovarStrikerVisualizer_Paint(object sender, PaintEventArgs e)
        {
            //string text = DateTime.Now.ToString() + " " + sender.ToString() + " " + e.ToString();
            //System.IO.File.AppendAllText(@"D:\src\Applications\Mark's Visualizer\Form1_Paint calls", text + Environment.NewLine);
            
            transformedData = FFT(transformedData); // this is the data you were looking for
            drawing = true;
            if (!paintInitiated)
            {
                pen = new Font("Arial", 12);
                osdPanel.ImplementDrawAction(delegate (object[] pars)
                {

                    Graphics g = pars[0] as System.Drawing.Graphics;
                    OSD osd = pars[1] as OSD;
                    if (osd.Info == null)
                        return;
                    int yIndex = 0;
                    foreach (var key in osd.Info.Keys)
                    {
                        g.DrawString(string.Format("{0}:{1}", key, osdPanel.Info[key]), pen, new SolidBrush(Color.Red), new PointF(10, this.Height / 2 + yIndex * 20));
                        yIndex++;
                    }

                });
                paintInitiated = true;
            }

            DrawData(transformedData, e.Graphics, cvt);
            osdPanel.Draw(e.Graphics, osdPanel);
            e.Graphics.DrawString("Right click for setting", pen, Constants.Brushes.redBrush, 10, this.Height - 60);
            drawing = false;

            Invalidate(true);
        }

        int[] chunk_freq = { 800, 1600, 3200, 6400, 12800, 30000 };
        int[] chunk_freq_jump = { 1, 2, 4, 6, 8, 10, 16 };
        double lastDelay = 0;

        private double[] FFT(double[] lastData)
        {
            DateTime chkpoint1 = DateTime.Now;
            if (dataList == null)
                return null;
            int actualN = distance2Node + 1;

            if (actualN < N_FFT)
                return new double[0];

            bool transformed = false;
            if (lastData == null || lastData.Length == 0)
            {
                lastData = new double[N_FFT];
            }
            else
            {
                transformed = true;
            }
            ComplexNumber[] data = new ComplexNumber[N_FFT];
            var runningNode = endingNode;
            for (int i = 0; i < N_FFT; i++)
            {
                data[i] = runningNode.Value;
                if (runningNode.PrevNode == null)
                {
                }
                runningNode = runningNode.PrevNode;
            }
            var result = FFTProcessor.FFT(data);
            double N2 = result.Length / 2;
            double[] finalresult = new double[lastData.Length];
            int k = 1, transformedDataIndex = 0;
            double value = 0;

            double refFeq = 250;

            int i_ref = (int)(refFeq * N2 / 22050);
            for (int i = 0; i < N2; i += k)
            {
                value = 0;
                //k = i / i_ref;
                //k = k == 0 ? 1 : k;
                var mappedFreq = i * samplingFrequency / 2 / N2;
                for (int l = 0; l < chunk_freq.Length; l++)
                {
                    if (mappedFreq < chunk_freq[l] || l == chunk_freq.Length - 1)
                    {
                        k = chunk_freq_jump[l];//chunk_freq[l] / chunk_freq[0];
                        break;
                    }
                }

                for (int j = i; j < i + k && j < N2; j++)
                {
                    value += result[j].Magnitude;
                }


                value = value * MasterScaleFFT;
                lastData[transformedDataIndex] -= lastDelay * DropOffScale;
                if (Mode == 0)
                    finalresult[transformedDataIndex] = value;
                else
                    finalresult[transformedDataIndex] = value > lastData[transformedDataIndex] ? value : lastData[transformedDataIndex];
                transformedDataIndex++;
            }


            if (!transformed)
                Array.Resize<double>(ref finalresult, transformedDataIndex);


            DateTime chkpoint1_end = DateTime.Now;
            lastDelay = chkpoint1_end.Subtract(chkpoint1).TotalMilliseconds;
            osdPanel.AddSet("FFT delay(ms)", lastDelay.ToString());
            return finalresult;

        }

        private void DrawData(double[] data, Graphics g, Converter cvter)
        {
            var chkpoint2 = DateTime.Now;
            if (data == null || data.Length == 0 || sourceData == null)
                return;

            float ratioFreq = (float)panel1.Width / (float)data.Length; ;
            float ratioFreqTest = (float)panel1.Width / 200;
            float ratioWave = (float)panel1.Width / (float)sourceData.Length;
            float value = 0;


            gTempBuffer.DrawImage(mainBuffer, new Rectangle(0, 0, tempBuffer.Width, tempBuffer.Height), 0, 0, mainBuffer.Width, mainBuffer.Height, GraphicsUnit.Pixel);

            //BitmapFilter.GaussianBlur(mainBuffer as Bitmap, 5);

            gMainBuffer.DrawImage(tempBuffer, new Rectangle(-2, -1, mainBuffer.Width + 4, mainBuffer.Height + 3), 0, 0, tempBuffer.Width, tempBuffer.Height, GraphicsUnit.Pixel, imgAttribute);


            int sx, sy;
            for (int i = 0; i < data.Length; i++)
            {
                //REM'd by Mark
                //cvter.FromReal(i * ratioFreq, 0, out sx, out sy);
                //value = (float)(data[i] * cvt.MaxScaledY);
                //gMainBuffer.FillRectangle(Constants.Brushes.redLightBrush, i * ratioFreq, sy, ratioFreq - 1, value / 2);
            }


            for (int i = 0; i < sourceData.Length - 4; i += 4)
            {
                //REM'd by Mark
                //cvter.FromReal(i * ratioWave, 0, out sx, out sy);
                //gMainBuffer.DrawLine(new Pen(Constants.Brushes.blueBrush),
                //new PointF(i * ratioWave, sy - (int)(sourceData[i] * 150 * MasterScaleFFT) + 2), new PointF((i + 4) * ratioWave,
                //sy - (int)(sourceData[i + 4] * 150 * MasterScaleFFT) + 2));
            }

            g.DrawImage(mainBuffer, new Point(0, 0));


            for (int i = 0; i < data.Length; i++)
            {
                cvter.FromReal(i * ratioFreq, 0, out sx, out sy);
                value = (float)(data[i] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.redBrush, i * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }

            //#region BeatDetector (Don't know what will to, just code)
            //double newBass = 0, accumBass = 0;
            //bool beatDetected = BeatDetector.Scan(data, 6, 13, ref accumBass, ref newBass);
            //#endregion

            osdPanel.AddSet("Drawing delay(ms):", DateTime.Now.Subtract(chkpoint2).TotalMilliseconds.ToString());
        }

        private void NovarStrikerVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (waveInStream != null)
            {
                waveInStream.StopRecording();
                waveInStream.Dispose();
                waveInStream = null;
            }
        }
    }
}
