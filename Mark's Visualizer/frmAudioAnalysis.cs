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

            AudioIn.InitSoundCapture();

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

            for (int i = 0; i < numRanges; i++)
            {
                if (i > 0)
                {
                    rangeLows[i] = rangeHighs[i] = trckbrMax.Maximum;
                }
                pnlRangeButtons.Controls[i].BackColor = colors[i];
                pnlBars.Controls[i].ForeColor = colors[i];
            }

            txtMasterScaleFFT.Text = AudioIn.MasterScaleFFT.ToString();
            txtTimer1Interval.Text = timer1.Interval.ToString();

            cboArduinoCommands.Items.AddRange(ArduinoCode.arduinoCommands);
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

            readConfig();
            initSongNames();
        }

        #region Novar Striker style spectrum

        private void InitCoordinator()
        {
            //Set baseScale to 4096/N_FFT.
            double baseScale = 4096d / (double)FFTProcessor.N_FFT;
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
            if (data == null || data.Length == 0 || AudioIn.sourceData == null)
                return;

            float ratioFreq = (float)pnlSpectrum.Width / (float)data.Length; ;
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = (float)pnlSpectrum.Width / (float)AudioIn.sourceData.Length;
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

            config[0] = AudioIn.MasterScaleFFT.ToString();
            config[1] = timer1.Interval.ToString();

            config[2] = config[3] = config[4] = "";

            for (int i = 0; i < numRanges; i++)
            {
                config[2] += rangeLows[i] + ",";
                config[3] += rangeHighs[i] + ",";
                config[4] += (int)thresholds[i] + ",";
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

            AudioIn.MasterScaleFFT = Int32.Parse(config[0]);
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
                ArduinoCode.Trigger(rangeIndex);
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
                pnlAccumAudio.Controls[rangeIndex].Text = ((int)accumAudios[rangeIndex] / AudioIn.MasterScaleFFT).ToString();
                pnlNewAudio.Controls[rangeIndex].Text = ((int)newAudios[rangeIndex] / AudioIn.MasterScaleFFT).ToString();
            }
        }

        private void DrawChart1(int rangeIndex)
        {
            if (newAudios[selectedRange] > thresholds[selectedRange] || true)
            {
                chart1.Series[0].Points.AddY(newAudios[selectedRange] / AudioIn.MasterScaleFFT);
                chart1.ChartAreas[0].AxisY.Maximum = chart1.Series[0].Points.FindMaxByValue("Y1",

                    (chart1.Series[0].Points.Count() > 200 ? (chart1.Series[0].Points.Count() - 200) : chart1.Series[0].Points.Count() - 1)

                    ).YValues[0];
                chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Maximum - 250;
            }
        }





        double[] transformedData;
        OSD osdPanel = new OSD();




        private void MarksVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIn.Dispose();

            writeConfig(currentConfig);
            System.IO.File.WriteAllText(configPath + @"\LastConfig\LastConfig.txt", currentConfig);

        }

        bool spectrumCounted = false;

        public void timer1_Tick(object sender, EventArgs e)
        {
            transformedData = FFTProcessor.FFT(transformedData);

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
            stripline.IntervalOffset = thresholds[selectedRange] = trckbrThreshold.Value;

            txtThreshold.Text = trckbrThreshold.Value.ToString();
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
                    AudioIn.MasterScaleFFT = intFromTextBox;
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

        private void btnArduino_Click(object sender, EventArgs e)
        {
            ArduinoCode.InterpretCommand(cboArduinoCommands.Text);
        }

        private void btnPlus10Percent_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numRanges; i++)
            {
                thresholds[i] += thresholds[i] * 0.1;
            }
            trckbrThreshold.Value = (int)thresholds[selectedRange];
            trckbrThreshold_ValueChanged(null, null);
        }

        private void btnMinus10Percent_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numRanges; i++)
            {
                thresholds[i] -= thresholds[i] * 0.1;
            }
            trckbrThreshold.Value = (int)thresholds[selectedRange];
            trckbrThreshold_ValueChanged(null, null);
        }

        private void btnCalibrateTrackbar_Click(object sender, EventArgs e)
        {
            trckbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            trckbrThreshold_ValueChanged(null, null);
        }

        private void btnIncreaseMax_Click(object sender, EventArgs e)
        {
            trckbrThreshold.Maximum += (int)((double)trckbrThreshold.Maximum * 0.1) ;
            trckbrThreshold_ValueChanged(null, null);
        }

        private void btnDecreaseMax_Click(object sender, EventArgs e)
        {
            trckbrThreshold.Maximum -= (int)((double)trckbrThreshold.Maximum * 0.1);
            trckbrThreshold_ValueChanged(null, null);
        }
    }
}
