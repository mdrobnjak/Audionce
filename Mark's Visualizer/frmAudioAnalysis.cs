using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioAnalysis.External;
using NAudio.Dsp;
using NAudio.Wave;
using System.Numerics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis : Form
    {
        public frmAudioAnalysis()
        {
            InitializeComponent();
        }

        private async void frmAudioAnalysis_Load(object sender, EventArgs e)
        {
            InitBufferAndGraphic();

            stripline.Interval = 0;
            stripline.IntervalOffset = 35;
            stripline.StripWidth = 5;
            stripline.BackColor = Color.Red;

            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline);

            //Force high priority if needed
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            InitSoundCapture();

            #region just to try in a another manner
            //if (t != null)
            //    if (t.ThreadState == System.Threading.ThreadState.Running)
            //        t.Abort();
            //t = new Thread(new ThreadStart(delegate ()
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

            InitCoordinator();

            #region InitControls

            colors[0] = Color.Pink;
            colors[1] = Color.LightBlue;
            colors[2] = Color.Gold;

            updateControlsByRange(0);

            pnlBars.Controls.SetChildIndex(barRange1, 0);
            pnlBars.Controls.SetChildIndex(barRange2, 1);
            pnlBars.Controls.SetChildIndex(barRange3, 2);
            pnlAccumAudio.Controls.SetChildIndex(txtAccumAudio1, 0);
            pnlAccumAudio.Controls.SetChildIndex(txtAccumAudio2, 1);
            pnlAccumAudio.Controls.SetChildIndex(txtAccumAudio3, 2);
            pnlNewAudio.Controls.SetChildIndex(txtNewAudio1, 0);
            pnlNewAudio.Controls.SetChildIndex(txtNewAudio2, 1);
            pnlNewAudio.Controls.SetChildIndex(txtNewAudio3, 2);
            pnlRangeButtons.Controls.SetChildIndex(btnRange1, 0);
            pnlRangeButtons.Controls.SetChildIndex(btnRange2, 1);
            pnlRangeButtons.Controls.SetChildIndex(btnRange3, 2);

            txtMasterScaleFFT.Text = MasterScaleFFT.ToString();
            txtTimer1Interval.Text = timer1.Interval.ToString();

            #endregion

            this.spectrumGraphics = pnlSpectrum.CreateGraphics();

            //Init BeatDetector with an evaluateLength of 50.
            for (int i = 0; i < beatDetectors.Count(); i++)
            {
                beatDetectors[i] = new BeatDetector();
                beatDetectors[i].InitDetector(50);
            }

            while (!spectrumCounted)
            {
                await Task.Delay(10);
            }

            //Init All Ranges besides 1 to upper end of bandwidth
            for (int i = 0; i < numRanges; i++)
            {
                if (i > 0)
                {
                    rangeLows[i] = rangeHighs[i] = trckbrMax.Maximum;
                }
                pnlRangeButtons.Controls[i].BackColor = colors[i];
                pnlBars.Controls[i].ForeColor = colors[i];
            }

            readConfig();
            initSongNames();
        }

        #region Novar Striker style spectrum

        private void InitCoordinator()
        {
            //Set baseScale to 4096/N_FFT.
            double baseScale = 4096d / (double)N_FFT;
            //Set Converter cvt to a new Converter
            cvt = new Converter(0, pnlSpectrum.Location.Y + pnlSpectrum.Height /*/ 2*/, 1, baseScale);

        }

        private void InitBufferAndGraphic()
        {
            //mainBuffer is a new Bitmap.
            //What is panel1?
            //What is the PixelFormat?
            mainBuffer = new Bitmap(pnlSpectrum.Width, pnlSpectrum.Height, PixelFormat.Format32bppArgb);
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

        bool paintInitiated = false;
        bool drawing = false;
        Font pen;
        Converter cvt;
        Graphics spectrumGraphics;

        private void PaintSpectrum()
        {
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

            DrawData(transformedData, spectrumGraphics, cvt);

            drawing = false;
        }

        private void DrawData(double[] data, Graphics g, Converter cvter)
        {
            var chkpoint2 = DateTime.Now;
            if (data == null || data.Length == 0 || sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / (float)data.Length; ;
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = (float)pnlSpectrum.Width / (float)sourceData.Length;
            float value = 0;


            gTempBuffer.DrawImage(mainBuffer, new Rectangle(0, 0, tempBuffer.Width, tempBuffer.Height), 0, 0, mainBuffer.Width, mainBuffer.Height, GraphicsUnit.Pixel);

            //BitmapFilter.GaussianBlur(mainBuffer as Bitmap, 5);

            gMainBuffer.DrawImage(tempBuffer, new Rectangle(-2, -1, mainBuffer.Width + 4, mainBuffer.Height + 3), 0, 0, tempBuffer.Width, tempBuffer.Height, GraphicsUnit.Pixel, imgAttribute);


            int sx, sy;

            g.DrawImage(mainBuffer, new Point(0, 0));

            pnlSpectrum.Refresh();
            for (int i = 0; i < data.Length; i++)
            {
                cvter.FromReal(i * ratioFreq, 0, out sx, out sy);
                value = (float)(data[i] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.redBrush, i * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }

            osdPanel.AddSet("Drawing delay(ms):", DateTime.Now.Subtract(chkpoint2).TotalMilliseconds.ToString());
        }

        #endregion

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

        private const int RATE = 44100;
        Node dataList = new Node(new ComplexNumber(0, 0));
        Node endingNode;
        WaveIn waveInStream;
        BufferedWaveProvider bwp;
        int BUFFERSIZE = (int)Math.Pow(2, 11); // must be a multiple of 2

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

            waveInStream.DeviceNumber = 0;

            //Set NumberOfBuffers to 5.
            waveInStream.NumberOfBuffers = 5;
            //Set BufferMilliseconds to 10.
            waveInStream.BufferMilliseconds = (int)((double)BUFFERSIZE / (double)RATE * 1000.0);
            //Create a new 16 bit Wave format with sample rate = samplingFrequency and channel count = 1
            waveInStream.WaveFormat = new WaveFormat(RATE, 1);
            //Create new EventHandler for when data is available to the Wave input device.
            waveInStream.DataAvailable += new EventHandler<WaveInEventArgs>(waveInStream_DataAvailable);

            bwp = new BufferedWaveProvider(waveInStream.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;
            bwp.DiscardOnBufferOverflow = true;

            //Start recording to the Wave input device.
            waveInStream.StartRecording();

            //Set Mode to 1.
            Mode = 1;
            //Set MasterScaleFFT to 1.
            MasterScaleFFT = 1;
            //Set DropOffScale to 0.4.
            DropOffScale = 10;
            //Set N_FFT to 2048.
            N_FFT = 2048;
        }

        #region File I/O
        /*
         * 0. MasterScaleFFT
         * 1. timer1.Interval
         * 2. rangeLows
         * 3. rangeHighs
         * 4. thresholds
         */

        string[] config = new string[10];

        const string configPath = @"..\..\..\Configs\";

        string currentConfig;

        private void writeConfig(string fileName = "Default")
        {
            //System.IO.File.WriteAllText(@"D:\src\Applications\Mark's Visualizer\Config.txt", "");

            config[0] = MasterScaleFFT.ToString();
            config[1] = timer1.Interval.ToString();

            config[2] = config[3] = config[4] = "";

            for (int i = 0; i < numRanges; i++)
            {
                config[2] += rangeLows[i] + ",";
                config[3] += rangeHighs[i] + ",";
                config[4] += thresholds[i] + ",";
            }
            for (int i = 2; i < 5; i++)
            {
                config[i] = config[i].TrimEnd(',');
            }

            System.IO.File.WriteAllLines(configPath + fileName + ".txt", config);
        }

        private void readConfig(string fileName = null)
        {
            if (fileName == null)
            {
                fileName = System.IO.File.ReadAllText(configPath + @"\LastConfig\LastConfig.txt");
                cboSongNames.Text = fileName;
            }

            currentConfig = fileName;

            config = System.IO.File.ReadAllLines(configPath + fileName + ".txt");

            MasterScaleFFT = Int32.Parse(config[0]);
            timer1.Interval = Int32.Parse(config[1]);
            for (int i = 0; i < numRanges; i++)
            {
                rangeLows[i] = Int32.Parse(config[2].Split(',')[i]);
                rangeHighs[i] = Int32.Parse(config[3].Split(',')[i]);
                thresholds[i] = Int32.Parse(config[4].Split(',')[i]);
            }

            btnRange1_Click(null, null);
        }

        private void initSongNames()
        {
            cboSongNames.Items.Clear();

            string songName = "";
            foreach (string songPath in Directory.GetFiles(configPath))
            {
                songName = songPath.Replace(configPath, "");
                songName = songName.Replace(".txt", "");
                cboSongNames.Items.Add(songName);
            }
        }

        #endregion

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

            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

            AppendData(sourceData);
        }

        const int numRanges = 3;

        BeatDetector[] beatDetectors = new BeatDetector[numRanges];

        int[] rangeLows = new int[numRanges];
        int[] rangeHighs = new int[numRanges];
        double[] newAudios = new double[numRanges];
        double[] accumAudios = new double[numRanges];
        double[] thresholds = new double[numRanges];
        int accumDivisor = 10;

        int selectedRange = 0;

        private void BeatDetect(int rangeIndex)
        {
            if (transformedData.Count() > rangeHighs[rangeIndex])
            {
                beatDetectors[rangeIndex].Scan(transformedData, rangeLows[rangeIndex], rangeHighs[rangeIndex], ref accumAudios[rangeIndex], ref newAudios[rangeIndex]);

                //DrawProgressBar(rangeIndex);
            }
        }

        private void DrawProgressBar(int rangeIndex)
        {
            if (newAudios[rangeIndex] > thresholds[rangeIndex])
            {
                ((ProgressBar)pnlBars.Controls[rangeIndex]).Value = ((ProgressBar)pnlBars.Controls[rangeIndex]).Maximum;
            }
            else
            {
                if (((ProgressBar)pnlBars.Controls[rangeIndex]).Value >= 3)
                {
                    ((ProgressBar)pnlBars.Controls[rangeIndex]).Value -= 3;
                }
            }
        }

        private void PrintAudiosFromBeatDetect(int rangeIndex)
        {
            if (newAudios[rangeIndex] > thresholds[rangeIndex])
            {
                pnlAccumAudio.Controls[rangeIndex].Text = ((int)accumAudios[rangeIndex] / MasterScaleFFT).ToString();
                pnlNewAudio.Controls[rangeIndex].Text = ((int)newAudios[rangeIndex] / MasterScaleFFT).ToString();
            }
        }

        private void DrawChart1(int rangeIndex)
        {
            if (newAudios[selectedRange] > thresholds[selectedRange] || true)
            {
                chart1.Series[0].Points.AddY(newAudios[selectedRange] / MasterScaleFFT);
                chart1.ChartAreas[0].AxisY.Maximum = chart1.Series[0].Points.FindMaxByValue("Y1",

                    (chart1.Series[0].Points.Count() > 200 ? (chart1.Series[0].Points.Count() - 200) : chart1.Series[0].Points.Count() - 1)

                    ).YValues[0];

                chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Maximum - 250;
            }
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

        double[] transformedData;
        OSD osdPanel = new OSD();

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
                var mappedFreq = i * RATE / 2 / N2;
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

        private void MarksVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (waveInStream != null)
            {
                waveInStream.StopRecording();
                waveInStream.Dispose();
                waveInStream = null;
            }

            writeConfig(currentConfig);
            System.IO.File.WriteAllText(configPath + @"\LastConfig\LastConfig.txt", currentConfig);
        }

        bool spectrumCounted = false;

        public void timer1_Tick(object sender, EventArgs e)
        {
            transformedData = FFT(transformedData);

            #region Calibrate Trackbars
            if (!spectrumCounted && transformedData.Count() != 0)
            {
                trckbrMin.Maximum = trckbrMax.Maximum = transformedData.Count() - 1;
                spectrumCounted = true;
            }
            #endregion

            //execute once
            PaintSpectrum();
            DrawChart1(selectedRange);

            //execute per Range
            for (int rangeIndex = 0; rangeIndex < numRanges; rangeIndex++)
            {
                BeatDetect(rangeIndex);
                // [1] subtracts from [2]
                //if (rangeIndex == 2)
                //{
                //    newAudios[rangeIndex] -= newAudios[rangeIndex - 1];
                //}
                PrintAudiosFromBeatDetect(rangeIndex);
                DrawProgressBar(rangeIndex);
            }

        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            cvt._yCenter = pnlSpectrum.Location.Y + pnlSpectrum.Height;
        }

        #region Ranges View

        Color[] colors = new Color[numRanges];

        private void updateControlsByRange(int rangeIndex)
        {
            trckbrMin.BackColor = trckbrMax.BackColor = trckbrThreshold.BackColor
                = chart1.Series[0].Color = this.BackColor = colors[selectedRange];

            trckbrMin.Value = rangeLows[selectedRange];
            trckbrMax.Value = rangeHighs[selectedRange];
            if (thresholds[selectedRange] > trckbrThreshold.Maximum)
            {
                trckbrThreshold.Maximum = (int)(thresholds[selectedRange] * 1.33);
            }
            trckbrThreshold.Value = (int)thresholds[selectedRange];
        }

        private void btnRange1_Click(object sender, EventArgs e)
        {
            selectedRange = 0;

            updateControlsByRange(selectedRange);
        }

        private void btnRange2_Click(object sender, EventArgs e)
        {
            selectedRange = 1;

            updateControlsByRange(selectedRange);

        }

        private void btnRange3_Click(object sender, EventArgs e)
        {
            selectedRange = 2;

            updateControlsByRange(selectedRange);
        }

        int intFromTextBox = 0;

        private void txtThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Int32.TryParse(txtThreshold.Text, out intFromTextBox))
                {
                    if (intFromTextBox > trckbrThreshold.Maximum) trckbrThreshold.Maximum = intFromTextBox;
                    trckbrThreshold.Value = intFromTextBox;
                }
            }
        }

        double AxisYMinimumMultiplier = .8;

        StripLine stripline = new StripLine();

        private void trckbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (trckbrThreshold.Value < chart1.ChartAreas[0].AxisY.Maximum)
            {
                trckbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            }

            thresholds[selectedRange] = trckbrThreshold.Value;

            txtThreshold.Text = trckbrThreshold.Value.ToString();

            stripline.IntervalOffset = thresholds[selectedRange];
        }

        #endregion

        private void trckbrMin_ValueChanged(object sender, EventArgs e)
        {
            rangeLows[selectedRange] = trckbrMin.Value;
        }

        private void trckbrMax_ValueChanged(object sender, EventArgs e)
        {
            rangeHighs[selectedRange] = trckbrMax.Value;
        }

        private void txtMasterScaleFFT_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (Int32.TryParse(txtMasterScaleFFT.Text, out intFromTextBox))
                {
                    MasterScaleFFT = intFromTextBox;
                }
            }
        }

        private void txtTimer1Interval_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (Int32.TryParse(txtTimer1Interval.Text, out intFromTextBox))
                {
                    timer1.Interval = intFromTextBox;
                }
            }

        }

        private void cboSongNames_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string song = cboSongNames.Items[cboSongNames.SelectedIndex].ToString();
            readConfig(song);
        }

        private void btnSaveSong_Click(object sender, EventArgs e)
        {
            writeConfig(cboSongNames.Text);
            initSongNames();
        }
    }
}
