﻿using System;
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

            Range.Init(ref Ranges);

            InitGUI();

            InitSettings();

            LoadSaveData();

            timer1.Enabled = true;
        }

        #region Init

        Range[] Ranges;

        void InitGUI()
        {
            InitSpectrum();

            InitChart();

            InitControls();
        }

        void InitSettings()
        {
            InitFFT();

            InitPostProcessing();

            InitAutoSettings();

            InitArduinoSettings();
        }

        void LoadSaveData()
        {

            ReadConfig();
            LoadSongNames();
        }

        void InitSpectrum()
        {
            InitBufferAndGraphicForSpectrum();

            Spectrum.SyncBandsAndFreqs();

            Spectrum.InitFullSpectrum();

            InitConverter(converterScale);
        }

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

        private void InitControls()
        {
            UpdateControls();

            pnlBars.Controls.SetChildIndex(barRange1, 0);
            pnlBars.Controls.SetChildIndex(barRange2, 1);
            pnlBars.Controls.SetChildIndex(barRange3, 2);
            pnlRangeButtons.Controls.SetChildIndex(btnRange1, 0);
            pnlRangeButtons.Controls.SetChildIndex(btnRange2, 1);
            pnlRangeButtons.Controls.SetChildIndex(btnRange3, 2);

            for (int i = 0; i < Range.Count; i++)
            {
                pnlRangeButtons.Controls[i].BackColor = Ranges[i].Color;
                pnlBars.Controls[i].ForeColor = Ranges[i].Color;
            }

            cboArduinoCommands.Items.AddRange(ArduinoCode.arduinoCommands);
        }

        private void InitChart()
        {
            stripline.Interval = 0;
            stripline.IntervalOffset = 35;
            stripline.StripWidth = 5;
            stripline.BackColor = Color.Red;
            chart1.ChartAreas[0].AxisY.StripLines.Add(stripline);
        }

        void InitPostProcessing()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                cboSubtractFrom.Items.Add(i);
                cboSubtractor.Items.Add(i);
            }
            cboSubtractFrom.Text = "0";
            cboSubtractor.Text = "1";
        }

        #endregion

        #region Secondary Processing

        //private void BeatDetect(int r)
        //{
        //    if (transformedData.Count() > Ranges[r].BandHi)
        //    {
        //        Ranges[r].beatDetector.Scan(transformedData, Ranges[r].BandLo, Ranges[r].BandHi, ref accumAudios[r], ref newAudios[r]);
        //    }
        //}

        private void GetRangeAudio(int r)
        {
            if (transformedData.Count() > Ranges[r].BandHi)
            {
                double temp = 0;
                for (int i = Ranges[r].BandLo; i < Ranges[r].BandHi; i++)
                {
                    temp += transformedData[i];
                }
                Ranges[r].NewAudio = temp;
            }
        }

        private void DrawProgressBar(int r)
        {
            if (Ranges[r].NewAudio > Ranges[r].Threshold)
            {
                ArduinoCode.Trigger(r);
                if (!drawBars) return;
                ((ProgressBar)pnlBars.Controls[r]).Value = ((ProgressBar)pnlBars.Controls[r]).Maximum;
            }
            else if (drawBars)
            {
                if (((ProgressBar)pnlBars.Controls[r]).Value >= 6)
                {
                    ((ProgressBar)pnlBars.Controls[r]).Value -= 6;
                }
            }
        }

        bool drawChart = true;
        float[] Ys;

        private void DrawChart()
        {
            if (!drawChart) return;
            chart1.Series[0].Points.AddY(Range.Active.NewAudio);
            chart1.ChartAreas[0].AxisY.Maximum = GetMaxYFromLast(200);

            stripline.IntervalOffset = Range.Active.Threshold;

            //cvt.MaxScaledY = 800 / chart1.ChartAreas[0].AxisY.Maximum; //AutoScaling Spectrum Y

            chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Maximum - 250;
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

            trkbrMax.Maximum = trkbrMin.Maximum = Range.Active.NumBands;

            trkbrMin.BackColor = trkbrMax.BackColor = trkbrThreshold.BackColor
                = chart1.Series[0].Color = this.BackColor = Range.Active.Color;

            trkbrMin.Value = Range.Active.BandLo - Range.Active.NumBandsBefore;//Math.Min(rangeLows[selectedRange], trkbrMin.Maximum);
            trkbrMax.Value = Range.Active.BandHi - Range.Active.NumBandsBefore;//Math.Min(Range.Active.BandHi, trkbrMax.Maximum);
            if (Range.Active.Threshold > trkbrThreshold.Maximum)
            {
                trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
            }
            trkbrThreshold.Value = (int)Range.Active.Threshold;
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
            DrawChart();

            if (AutoSettings.Ranging)
            {
                AutoSettings.CollectFFTData(transformedData);
            }
            else if (AutoSettings.ReadyToProcess)
            {
                AutoSettings.ReadyToProcess = false;

                //Range1
                Ranges[0].AutoSettings.BassFreqSelector();

                //Range2
                Ranges[1].AutoSettings.SnareFreqSelector();

                //Range3
                Ranges[2].AutoSettings.HatFreqSelector();

                //SelectedRange
                trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
                UpdateControls();

                AutoSettings.Reset();
            }

            //execute per Range
            for (int r = 0; r < Range.Count; r++)
            {
                //BeatDetect(rangeIndex);
                GetRangeAudio(r);

                if (Ranges[r].AutoSettings.DynamicThreshold) Ranges[r].Threshold = Ranges[r].GetMaxFromLast() * Ranges[r].AutoSettings.ThresholdMultiplier;

                if (subtract)
                {
                    if (r == subtractFromIndex)
                    {
                        Ranges[r].NewAudio -= Ranges[subtractorIndex].NewAudio;
                    }
                }
                DrawProgressBar(r);
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
            Range.SetActive(0);
            UpdateControls();
        }

        private void btnRange2_Click(object sender, EventArgs e)
        {
            Range.SetActive(1);
            UpdateControls();
        }

        private void btnRange3_Click(object sender, EventArgs e)
        {
            Range.SetActive(2);
            UpdateControls();
        }

        StripLine stripline = new StripLine();

        private void trkbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            stripline.IntervalOffset = Range.Active.Threshold = trkbrThreshold.Value;

            txtThreshold.Text = trkbrThreshold.Value.ToString();
        }

        private void trkbrMin_ValueChanged(object sender, EventArgs e)
        {
            int trkbrIndex = trkbrMin.Value + Range.Active.NumBandsBefore;
            if (trkbrIndex > Range.Active.NumBandsBefore + Range.Active.NumBands || trkbrIndex < Range.Active.NumBandsBefore)
            {
                //If out of range, revert.
                trkbrMin.Value = Range.Active.BandLo;
            }
            else Range.Active.BandLo = trkbrIndex;
        }

        private void trkbrMax_ValueChanged(object sender, EventArgs e)
        {
            int trkbrIndex = trkbrMax.Value + Range.Active.NumBandsBefore;
            if (trkbrIndex > Range.Active.NumBandsBefore + Range.Active.NumBands || trkbrIndex < Range.Active.NumBandsBefore)
            {
                //If out of range, revert.
                trkbrMax.Value = Range.Active.BandHi;
            }
            else Range.Active.BandHi = trkbrMax.Value + Range.Active.NumBandsBefore;
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            trkbrThreshold.Value = (int)(trkbrThreshold.Maximum * Range.Active.AutoSettings.ThresholdMultiplier);
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

        private void btnToggleChart_Click(object sender, EventArgs e)
        {
            drawChart = !drawChart;
        }

        bool subtract = false;
        int subtractorIndex, subtractFromIndex;

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            int intVar;

            if (!int.TryParse(cboSubtractor.Text, out intVar)) return;
            subtractorIndex = intVar;
            if (!int.TryParse(cboSubtractFrom.Text, out intVar)) return;
            subtractFromIndex = intVar;

            subtract = !subtract;

            btnSubtract.BackColor = subtract ? Color.LightGreen : Color.Transparent;
        }

        bool drawBars = true;

        private void btnToggleBars_Click(object sender, EventArgs e)
        {
            drawBars = !drawBars;
        }
    }
}
