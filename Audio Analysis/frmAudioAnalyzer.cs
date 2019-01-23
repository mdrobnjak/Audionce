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

namespace AudioAnalyzer
{
    public partial class frmAudioAnalyzer : Form
    {
        public frmAudioAnalyzer()
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

        private void frmAudioAnalyzer_Load(object sender, EventArgs e)
        {
            AudioIn.InitSoundCapture();

            InitSpectrum();

            Range.Init(ref Ranges);

            InitGUI();

            InitSettings();

            LoadSaveData();

            //ML.Predict();

            timer1.Enabled = true;
        }

        #region Init

        Range[] Ranges;

        void InitGUI()
        {
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

        double tmpRangeAudio;

        private void Filter(int r)
        {
            if (transformedData.Count() > Ranges[r].HighCutAbsolute)
            {
                tmpRangeAudio = 0;
                for (int i = Ranges[r].LowCutAbsolute; i < Ranges[r].HighCutAbsolute; i++)
                {
                    tmpRangeAudio += transformedData[i];
                }
                Ranges[r].Audio = tmpRangeAudio;
            }
        }

        bool Gate(int r)
        {
            return Ranges[r].Audio > Ranges[r].Threshold ? true : false;
        }

        private void SetProgressBars(int r)
        {
            if (!drawBars) return;
            ((ProgressBar)pnlBars.Controls[r]).Value = 100;
        }

        void FadeProgressBars(int r)
        {
            if (!drawBars) return;
            if (((ProgressBar)pnlBars.Controls[r]).Value >= 6)
            {
                ((ProgressBar)pnlBars.Controls[r]).Value -= 6;
            }
        }

        bool drawChart = true;

        private void DrawChart()
        {
            chart1.Series[0].Points.AddY(Range.Active.Audio);
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
            trkbrMax.Maximum = trkbrMin.Maximum = Spectrum.DisplayBands;

            trkbrMin.BackColor = trkbrMax.BackColor = trkbrThreshold.BackColor
                = chart1.Series[0].Color = this.BackColor = Range.Active.Color;

            trkbrMin.Value = Range.Active.LowCutIndex;
            trkbrMax.Value = Range.Active.HighCutIndex;
            if (Range.Active.Threshold > trkbrThreshold.Maximum)
            {
                trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
            }
            trkbrThreshold.Value = (int)Range.Active.Threshold;
        }

        #region timer1_Tick()

        delegate void DrawActiveCallback();

        private void DrawActive()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (pnlSpectrum.InvokeRequired)
            {
                DrawActiveCallback d = new DrawActiveCallback(DrawActive);
                this.Invoke(d);
            }
            else
            {
                if (Spectrum.drawSpectrum) PaintSpectrum();
                if (drawChart) DrawChart();
            }
        }


        double[] transformedData;
        public void timer1_Tick(object sender, EventArgs e)
        {
            if (FFT.N_FFT != FFT.N_FFTBuffer)
            {
                FFT.N_FFT = FFT.N_FFTBuffer;
                transformedData = null;
            }

            transformedData = FFT.FFTWithProcessing(transformedData);

            for (int r = 0; r < Range.Count; r++)
            {
                Filter(r);

                ApplySubtraction(r);

                Ranges[r].AutoSettings.ApplyAutoSettings();

                if (Gate(r))
                {
                    ArduinoCode.Trigger(r);
                    SetProgressBars(r);
                }
                else
                {
                    FadeProgressBars(r);
                }

            }

            Task.Run(() => DrawActive());

            if (AutoSettings.Ranging)
            {
                AutoSettings.CollectFFTData(transformedData);
            }
            else if (AutoSettings.ReadyToProcess)
            {
                AutoSettings.ReadyToProcess = false;

                //Range1
                PrintBandAnalysis(Ranges[0].AutoSettings.DoBandAnalysis());
                Ranges[0].AutoSettings.KickSelector();

                //Range2
                Ranges[1].AutoSettings.SnareSelector();

                //Range3
                Ranges[2].AutoSettings.HatSelector();

                //SelectedRange
                trkbrThreshold.Maximum = (int)(Range.Active.Threshold * 1.33);
                UpdateControls();

                AutoSettings.Reset();
            }
        }

        void ApplySubtraction(int r)
        {
            if (subtract)
            {
                if (r == subtractFromIndex)
                {
                    Ranges[r].Audio -= Ranges[subtractorIndex].Audio;
                }
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
            Range.Active.LowCutIndex = trkbrMin.Value;
        }

        private void trkbrMax_ValueChanged(object sender, EventArgs e)
        {
            Range.Active.HighCutIndex = trkbrMax.Value;
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            trkbrThreshold.Value = (int)(trkbrThreshold.Maximum * Range.Active.AutoSettings.ThresholdMultiplier);
            //trkbrThreshold_ValueChanged(null, null);
        }
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

            if (subtract)
            {
                subtract = false;
                btnSubtract.BackColor = Color.Transparent;
            }
            else
            {
                subtract = true;
                btnSubtract.BackColor = Color.LightGreen;
            }
        }

        bool drawBars = true;

        private void btnToggleBars_Click(object sender, EventArgs e)
        {
            drawBars = !drawBars;
        }
        #endregion

        Dictionary<string, List<double>> AlgorithmDatas;

        float[] ToArray(Dictionary<string, List<double>> algDatas)
        {
            float[] arr = new float[100];

            int a = 0;

            foreach (string key in algDatas.Keys)
            {
                for (int i = 0; i < 20; i++)
                {
                    arr[i + a] = (float)algDatas[key][i];
                }
                a += 20;
            }

            return arr;
        }

        //Band Analysis:        
        void PrintBandAnalysis(Dictionary<string, List<double>> AlgorithmDatas)
        {
            int i = 0;

            if (tabctrlBandAnalysis.TabPages.Count < 1)
            {
                foreach (string algName in AlgorithmDatas.Keys)
                {
                    tabctrlBandAnalysis.TabPages.Add(new TabPage(algName));
                    var txtAlgData = new TextBox();
                    txtAlgData.ScrollBars = ScrollBars.Both;
                    txtAlgData.Multiline = true;
                    txtAlgData.Dock = DockStyle.Fill;
                    tabctrlBandAnalysis.TabPages[i].Controls.Add(txtAlgData);
                    i++;
                }
            }

            i = 0;

            csvRow = "";

            foreach (string algName in AlgorithmDatas.Keys)
            {
                string dataToPrint = "";
                double max = 0;
                for (int j = 0; j < AlgorithmDatas[algName].Count; j++)
                {
                    dataToPrint += "[" + j + "]: " + ((int)AlgorithmDatas[algName][j]).ToString() + "\r\n";
                    if (AlgorithmDatas[algName][j] > max) max = AlgorithmDatas[algName][j];
                }

                for (int j = 0; j < AlgorithmDatas[algName].Count; j++)
                {
                    csvRow += (AlgorithmDatas[algName][j] / max).ToString() + ",";
                }

                tabctrlBandAnalysis.TabPages[i].Controls[0].Text = dataToPrint;

                i++;
            }

            csvRow = csvRow.Remove(csvRow.Length - 1, 1);
            csvRow += Environment.NewLine;

            this.AlgorithmDatas = new Dictionary<string, List<double>>(AlgorithmDatas);

            txtPrediction.Text = ML.PredictRealTime(ML.mlContext, Array.ConvertAll(csvRow.Split(','), float.Parse)).ToString();
        }

        string csvRow;

        private void btnTrain_Click(object sender, EventArgs e)
        {
            AppendCSV();
        }

        void AppendCSV()
        {
            csvRow = csvRow.Insert(0, trkbrMin.Value.ToString() + ",");
            File.AppendAllText(ML._trainDataPath, csvRow);
        }
    }
}
