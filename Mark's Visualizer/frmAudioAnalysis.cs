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

            InitSpectrumSettings(0,300,5000,20000);

            InitChart1();

            InitCoordinator();

            InitControls();

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
        }

        #region Init Methods

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

        Graphics spectrumGraphics;

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

            this.spectrumGraphics = pnlSpectrum.CreateGraphics();
        }

        int hzPerBand = AudioIn.RATE / FFTProcessor.N_FFT;
        int[] bandsPerRange, bandsBefore;

        private void InitSpectrumSettings()
        {
            bandsPerRange = new int[]
            {
                transformedData.Length, transformedData.Length, transformedData.Length
            };
            bandsBefore = new int[]
            {
                0,0,0
            };
        }

        private void InitSpectrumSettings(int freqMin, int freq1, int freq2, int freqMax)
        {
            bandsPerRange = new int[]
            {
                BandsPerRange(freq1 - freqMin), BandsPerRange(freq2 - freq1), BandsPerRange(freqMax - freq2)
            };

            bandsBefore = new int[]
            {
                0,bandsPerRange[0],bandsPerRange[0]+bandsPerRange[1]
            };
        }

        private int BandsPerRange(int bandwidthInHz)
        {
            return bandwidthInHz / hzPerBand;
        }
        
        Color[] colors = new Color[numRanges];

        private void InitControls()
        {
            updateControlsByRange();

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

        private void InitCoordinator()
        {
            //Set baseScale to 4096/N_FFT.
            double baseScale = 1.0;//4096d / (double)FFTProcessor.N_FFT;
            //Set Converter cvt to a new Converter
            cvt = new Converter(0, pnlSpectrum.Location.Y + pnlSpectrum.Height /*/ 2*/, 1, baseScale);
        }

        #endregion

        #region Draw Spectrum
        bool paintInitiated = false;
        bool drawing = false;
        Font pen;
        OSD osdPanel = new OSD();

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

            float ratioFreq = (float)pnlSpectrum.Width / bandsPerRange[selectedRange];
            float ratioFreqTest = (float)pnlSpectrum.Width / 200;
            float ratioWave = (float)pnlSpectrum.Width / (float)AudioIn.sourceData.Length;
            float value = 0;


            gTempBuffer.DrawImage(mainBuffer, new Rectangle(0, 0, tempBuffer.Width, tempBuffer.Height), 0, 0, mainBuffer.Width, mainBuffer.Height, GraphicsUnit.Pixel);

            //BitmapFilter.GaussianBlur(mainBuffer as Bitmap, 5);

            gMainBuffer.DrawImage(tempBuffer, new Rectangle(-2, -1, mainBuffer.Width + 4, mainBuffer.Height + 3), 0, 0, tempBuffer.Width, tempBuffer.Height, GraphicsUnit.Pixel, imgAttribute);


            int sx, sy;

            g.DrawImage(mainBuffer, new Point(0, 0));

            pnlSpectrum.Refresh();

            #region Fill Rectangles
            int bandIndexRelative = 0;
            int bandIndexAbsolute = bandsBefore[selectedRange];

            for (; bandIndexRelative < trkbrMin.Value; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            for (; bandIndexRelative < trkbrMax.Value; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.redBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            for (; bandIndexRelative < bandsPerRange[selectedRange] && bandIndexRelative < FFTProcessor.N_Spectrum - 1; bandIndexRelative++, bandIndexAbsolute++)
            {
                cvter.FromReal(bandIndexRelative * ratioFreq, 0, out sx, out sy);
                value = (float)(data[bandIndexAbsolute] * cvt.MaxScaledY);
                g.FillRectangle(Constants.Brushes.blackBrush, bandIndexRelative * ratioFreq, sy - value / 2, ratioFreq - 1, value / 2);
            }
            #endregion

            osdPanel.AddSet("Drawing delay(ms):", DateTime.Now.Subtract(chkpoint2).TotalMilliseconds.ToString());
        }

        #endregion 
        
        #region File I/O

        string[] config = new string[10];
        /*
         * 0. 1
         * 1. timer1.Interval
         * 2. rangeLows
         * 3. rangeHighs
         * 4. thresholds
         */
        const string configPath = @"..\..\..\Configs\";
        string currentConfig;

        private void WriteConfig(string fileName = "Default")
        {
            config[0] = "1";
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

        private void ReadConfig(string fileName = null)
        {
            if (fileName == null)
            {
                fileName = System.IO.File.ReadAllText(configPath + @"\LastConfig\LastConfig.txt");
                cboSongNames.Text = fileName;
            }

            currentConfig = fileName;

            config = System.IO.File.ReadAllLines(configPath + fileName + ".txt");

            //Int32.Parse(config[0]);
            timer1.Interval = Int32.Parse(config[1]);
            for (int i = 0; i < numRanges; i++)
            {
                rangeLows[i] = Int32.Parse(config[2].Split(',')[i]);
                rangeHighs[i] = Int32.Parse(config[3].Split(',')[i]);
                thresholds[i] = Int32.Parse(config[4].Split(',')[i]);
            }

            btnRange1_Click(null, null);
        }

        private void LoadSongNames()
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
                chart1.ChartAreas[0].AxisY.Maximum = chart1.Series[0].Points.FindMaxByValue("Y1",

                    (chart1.Series[0].Points.Count() > 200 ? (chart1.Series[0].Points.Count() - 200) : chart1.Series[0].Points.Count() - 1)

                    ).YValues[0];

                //cvt.MaxScaledY = 800 / chart1.ChartAreas[0].AxisY.Maximum; //AutoScaling Spectrum Y

                chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Maximum - 250;
            }
        }

        double[] transformedData;

        public void timer1_Tick(object sender, EventArgs e)
        {

            transformedData = FFTProcessor.FFTWithProcessing(transformedData);

            //execute once
            PaintSpectrum();
            DrawChart1(selectedRange);

            if(AutoSettings.autoRanging)
            {
                AutoSettings.CollectFFTData(0, transformedData.Length, transformedData);
            }
            else if(AutoSettings.readyToProcess)
            {
                //Range1
                trkbrMin.Value = AutoSettings.MostDynamic(0, BandsPerRange(300));
                trkbrMax.Value = trkbrMin.Value + 1;
                trkbrThreshold.Maximum = (int)(AutoSettings.AutoThreshold()*1.33);
                trkbrThreshold.Value = AutoSettings.AutoThreshold();
                //Range2
                rangeLows[1] = AutoSettings.MostDynamic(BandsPerRange(1000), BandsPerRange(10000));
                rangeHighs[1] = rangeLows[1] + 1;
                thresholds[1] = AutoSettings.AutoThreshold();
                //Range3
                rangeLows[2] = AutoSettings.MostDynamic(BandsPerRange(10000), BandsPerRange(20000));
                rangeHighs[2] = rangeLows[2] + 1;
                thresholds[2] = AutoSettings.AutoThreshold();

                AutoSettings.Reset();
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

        private void MarksVisualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIn.Dispose();

            WriteConfig(currentConfig);
            System.IO.File.WriteAllText(configPath + @"\LastConfig\LastConfig.txt", currentConfig);
        }

        private void pnlSpectrum_SizeChanged(object sender, EventArgs e)
        {
            cvt._yCenter = pnlSpectrum.Location.Y + pnlSpectrum.Height;
        }

        private void updateControlsByRange()
        {

            trkbrMax.Maximum = trkbrMin.Maximum = bandsPerRange[selectedRange];

            trkbrMin.BackColor = trkbrMax.BackColor = trkbrThreshold.BackColor
                = chart1.Series[0].Color = this.BackColor = colors[selectedRange];

            trkbrMin.Value = rangeLows[selectedRange];
            trkbrMax.Value = rangeHighs[selectedRange];
            if (thresholds[selectedRange] > trkbrThreshold.Maximum)
            {
                trkbrThreshold.Maximum = (int)(thresholds[selectedRange] * 1.33);
            }
            trkbrThreshold.Value = (int)thresholds[selectedRange];
        }

        private void btnRange1_Click(object sender, EventArgs e)
        {
            selectedRange = 0;

            updateControlsByRange();
        }

        private void btnRange2_Click(object sender, EventArgs e)
        {
            selectedRange = 1;

            updateControlsByRange();

        }

        private void btnRange3_Click(object sender, EventArgs e)
        {
            selectedRange = 2;

            updateControlsByRange();
        }

        int intFromTextBox = 0;

        private void txtThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Int32.TryParse(txtThreshold.Text, out intFromTextBox))
                {
                    if (intFromTextBox > trkbrThreshold.Maximum) trkbrThreshold.Maximum = intFromTextBox;
                    trkbrThreshold.Value = intFromTextBox;
                }
            }
        }

        StripLine stripline = new StripLine();

        private void trkbrThreshold_ValueChanged(object sender, EventArgs e)
        {
            stripline.IntervalOffset = thresholds[selectedRange] = trkbrThreshold.Value;

            txtThreshold.Text = trkbrThreshold.Value.ToString();
        }

        private void trkbrMin_ValueChanged(object sender, EventArgs e)
        {
            rangeLows[selectedRange] = trkbrMin.Value;
        }

        private void trkbrMax_ValueChanged(object sender, EventArgs e)
        {
            rangeHighs[selectedRange] = trkbrMax.Value;
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
            ReadConfig(song);
        }

        private void btnSaveSong_Click(object sender, EventArgs e)
        {
            WriteConfig(cboSongNames.Text);
            LoadSongNames();
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
            trkbrThreshold.Value = (int)thresholds[selectedRange];
            trkbrThreshold_ValueChanged(null, null);
        }

        private void btnMinus10Percent_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numRanges; i++)
            {
                thresholds[i] -= thresholds[i] * 0.1;
            }
            trkbrThreshold.Value = (int)thresholds[selectedRange];
            trkbrThreshold_ValueChanged(null, null);
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum = (int)chart1.ChartAreas[0].AxisY.Maximum;
            trkbrThreshold_ValueChanged(null, null);

        }

        private void btnIncreaseMax_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum += (int)((double)trkbrThreshold.Maximum * 0.1);
            trkbrThreshold_ValueChanged(null, null);
        }

        private void btnAutoRange_Click(object sender, EventArgs e)
        {
            AutoSettings.BeginAutoRange();
        }

        private void btnFullSpectrum_Click(object sender, EventArgs e)
        {
            InitSpectrumSettings();
            updateControlsByRange();
        }

        private void btnDecreaseMax_Click(object sender, EventArgs e)
        {
            trkbrThreshold.Maximum -= (int)((double)trkbrThreshold.Maximum * 0.1);
            trkbrThreshold_ValueChanged(null, null);
        }
    }
}
