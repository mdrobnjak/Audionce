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

            using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;


            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            #region Alternate Methods
            //Force high priority if needed
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

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
        }

        private void frmAudioAnalysis_Load(object sender, EventArgs e)
        {

            AudioIn.InitSoundCapture();

            InitBufferAndGraphicForSpectrum();

            Spectrum.SyncBandsAndFreqs();
            Spectrum.InitFullSpectrum();

            InitChart1();

            InitConverter(converterScale);

            InitControls();

            InitTabFFT();

            InitAutoSettings();

            #region Init BeatDetectors
            //Init BeatDetectors with an evaluateLength of 50.
            for (int i = 0; i < beatDetectors.Count(); i++)
            {
                beatDetectors[i] = new BeatDetector();
                beatDetectors[i].InitDetector(50);
            }
            #endregion

            #region Load Save Data
            ReadConfig();
            LoadSongNames();
            #endregion

            timer1.Enabled = true;
        }

        #region Init

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

        Graphics gSpectrum;

        private void InitBufferAndGraphicForSpectrum()
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

            this.gSpectrum = pnlSpectrum.CreateGraphics();
        }

        Color[] colors = new Color[numRanges];

        private void InitControls()
        {
            UpdateControls();

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

            colors[0] = Color.Pink;
            colors[1] = Color.LightBlue;
            colors[2] = Color.Gold;
            for (int i = 0; i < numRanges; i++)
            {
                pnlRangeButtons.Controls[i].BackColor = colors[i];
                pnlBars.Controls[i].ForeColor = colors[i];
            }

            cboArduinoCommands.Items.AddRange(ArduinoCode.arduinoCommands);
        }

        private void InitChart1()
        {
            stripline.Interval = 0;
            stripline.IntervalOffset = 35;
            stripline.StripWidth = 5;
            stripline.BackColor = Color.Red;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline);
        }

        Converter cvt;
        int converterScale = 8;

        private void InitConverter(int yMult)
        {
            double maxScaledY = (4096d / FFT.N_FFT) * yMult;
            cvt = new Converter(0, pnlSpectrum.Location.Y + pnlSpectrum.Height, 1, maxScaledY);
        }

        #endregion        
        
        #region Secondary Processing

        const int numRanges = 3;
        int selectedRange = 0;

        BeatDetector[] beatDetectors = new BeatDetector[numRanges];

        int[] rangeLows = new int[numRanges];
        int[] rangeHighs = new int[numRanges];
        double[] newAudios = new double[numRanges];
        double[] accumAudios = new double[numRanges];
        double[] thresholds = new double[numRanges];

        private void BeatDetect(int rangeIndex)
        {
            if (transformedData.Count() > rangeHighs[rangeIndex])
            {
                beatDetectors[rangeIndex].Scan(transformedData, rangeLows[rangeIndex], rangeHighs[rangeIndex], ref accumAudios[rangeIndex], ref newAudios[rangeIndex]);
            }
        }

        private void DrawProgressBar(int rangeIndex)
        {
            if (newAudios[rangeIndex] > thresholds[rangeIndex])
            {
                ((ProgressBar)pnlBars.Controls[rangeIndex]).Value = ((ProgressBar)pnlBars.Controls[rangeIndex]).Maximum;
                ArduinoCode.Trigger(rangeIndex);
            }
            else
            {
                if (((ProgressBar)pnlBars.Controls[rangeIndex]).Value >= 6)
                {
                    ((ProgressBar)pnlBars.Controls[rangeIndex]).Value -= 6;
                }
            }
        }

        private void PrintAudiosFromBeatDetect(int rangeIndex)
        {
            if (newAudios[rangeIndex] > thresholds[rangeIndex])
            {
                pnlAccumAudio.Controls[rangeIndex].Text = accumAudios[rangeIndex].ToString();
                pnlNewAudio.Controls[rangeIndex].Text = newAudios[rangeIndex].ToString();
            }
        }

        private void DrawChart1(int rangeIndex)
        {
            if (newAudios[selectedRange] > thresholds[selectedRange] || true)
            {
                chart1.Series[0].Points.AddY(newAudios[selectedRange]);
                chart1.ChartAreas[0].AxisY.Maximum = GetMaxYFromLast(200);

                //cvt.MaxScaledY = 800 / chart1.ChartAreas[0].AxisY.Maximum; //AutoScaling Spectrum Y

                chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Maximum - 250;
            }
        }

        private double GetMaxYFromLast(int numPoints)
        {
            return chart1.Series[0].Points.FindMaxByValue("Y1",

                    (chart1.Series[0].Points.Count() > numPoints ? (chart1.Series[0].Points.Count() - numPoints) : chart1.Series[0].Points.Count() - 1)

                    ).YValues[0];
        }

        #endregion

        private void UpdateControls()
        {

            trkbrMax.Maximum = trkbrMin.Maximum = Spectrum.bandsPerRange[selectedRange];

            trkbrMin.BackColor = trkbrMax.BackColor = trkbrThreshold.BackColor
                = chart1.Series[0].Color = this.BackColor = colors[selectedRange];

            trkbrMin.Value = Math.Min(rangeLows[selectedRange], trkbrMin.Maximum);
            trkbrMax.Value = Math.Min(rangeHighs[selectedRange], trkbrMax.Maximum);
            if (thresholds[selectedRange] > trkbrThreshold.Maximum)
            {
                trkbrThreshold.Maximum = (int)(thresholds[selectedRange] * 1.33);
            }
            trkbrThreshold.Value = (int)thresholds[selectedRange];
        }

        #region timer1_Tick()

        double[] transformedData;
        public void timer1_Tick(object sender, EventArgs e)
        {
            if (FFT.N_FFT != FFT.N_FFTBuffer)
            {
                FFT.N_FFT = FFT.N_FFTBuffer;
                transformedData = null;
            }

            transformedData = FFT.FFTWithProcessing(transformedData);

            //execute once
            PaintSpectrum();
            DrawChart1(selectedRange);

            if (AutoSet.ranging)
            {
                AutoSet.CollectFFTData(0, transformedData.Length, transformedData);
            }
            else if (AutoSet.readyToProcess)
            {
                //Range1
                rangeLows[0] = AutoSet.CenterFreqSelector(Spectrum.GetBandForFreq(fmin), Spectrum.GetBandForFreq(f1)) - AutoSet.bandwidth / 2;
                rangeLows[0] = Math.Max(rangeLows[0],0);
                rangeHighs[0] = rangeLows[0] + 1 + AutoSet.bandwidth / 2;
                thresholds[0] = AutoSet.Threshold();
                //Range2
                rangeLows[1] = AutoSet.CenterFreqSelector(Spectrum.GetBandForFreq(f1), Spectrum.GetBandForFreq(f2)) - AutoSet.bandwidth / 2;
                rangeHighs[1] = rangeLows[1] + 1 + AutoSet.bandwidth / 2;
                thresholds[1] = AutoSet.Threshold();
                //Range3
                rangeLows[2] = AutoSet.CenterFreqSelector(Spectrum.GetBandForFreq(f2), Spectrum.GetBandForFreq(fmax)) - AutoSet.bandwidth / 2;
                rangeHighs[2] = rangeLows[2] + 1 + AutoSet.bandwidth / 2;
                thresholds[2] = AutoSet.Threshold();
                //SelectedRange
                trkbrThreshold.Maximum = (int)(thresholds[selectedRange] * 1.33);
                UpdateControls();

                AutoSet.Reset();
            }

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
        #endregion

        #region Event Handlers

        private void AudioAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIn.Dispose();

            WriteConfig(currentConfig);
            System.IO.File.WriteAllText(configPath + @"\LastConfig\LastConfig.txt", currentConfig);
        }
        
        private void btnRange1_Click(object sender, EventArgs e)
        {
            selectedRange = 0;
            ArduinoCode.selectedRange = 0;
            UpdateControls();
        }

        private void btnRange2_Click(object sender, EventArgs e)
        {
            selectedRange = 1;
            ArduinoCode.selectedRange = 1;
            UpdateControls();

        }

        private void btnRange3_Click(object sender, EventArgs e)
        {
            selectedRange = 2;
            ArduinoCode.selectedRange = 2;
            UpdateControls();
        }

        StripLine stripline = new StripLine();

        private void trkbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            stripline.IntervalOffset = thresholds[selectedRange] = trkbrThreshold.Value;

            txtThreshold.Text = trkbrThreshold.Value.ToString();
        }

        private void trkbrMin_ValueChanged(object sender, EventArgs e)
        {
            rangeLows[selectedRange] = trkbrMin.Value + Spectrum.bandsBefore[selectedRange];
        }

        private void trkbrMax_ValueChanged(object sender, EventArgs e)
        {
            rangeHighs[selectedRange] = trkbrMax.Value + Spectrum.bandsBefore[selectedRange];
        }
        
        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            trkbrThreshold.Value = AutoSet.Threshold(trkbrThreshold.Maximum);
            //trkbrThreshold_ValueChanged(null, null);
        }

        #endregion

        private void btnIncrementRangeBand_Click(object sender, EventArgs e)
        {
            if (trkbrMax.Value == trkbrMax.Maximum) return;
            trkbrMin.Value++;
            trkbrMax.Value++;
        }

        private void btnDecrementRangeBand_Click(object sender, EventArgs e)
        {
            if (trkbrMin.Value == trkbrMin.Minimum) return;
            trkbrMin.Value--;
            trkbrMax.Value--;
        }
    }
}
