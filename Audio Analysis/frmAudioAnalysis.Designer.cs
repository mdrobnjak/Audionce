namespace AudioAnalysis
{
    partial class frmAudioAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.trkbrThreshold = new System.Windows.Forms.TrackBar();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.pnlRangeButtons = new System.Windows.Forms.Panel();
            this.btnRange3 = new System.Windows.Forms.Button();
            this.btnRange2 = new System.Windows.Forms.Button();
            this.btnRange1 = new System.Windows.Forms.Button();
            this.trkbrMin = new System.Windows.Forms.TrackBar();
            this.trkbrMax = new System.Windows.Forms.TrackBar();
            this.txtTimer1Interval = new System.Windows.Forms.TextBox();
            this.lblRefreshRate = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.cboSongNames = new System.Windows.Forms.ComboBox();
            this.btnSaveSong = new System.Windows.Forms.Button();
            this.btnArduino = new System.Windows.Forms.Button();
            this.cboArduinoCommands = new System.Windows.Forms.ComboBox();
            this.btnAutoRange = new System.Windows.Forms.Button();
            this.btnSpectrumMode = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFFT = new System.Windows.Forms.TabPage();
            this.txtDropOffScale = new System.Windows.Forms.TextBox();
            this.lblDropOffScale = new System.Windows.Forms.Label();
            this.lblSpectrumScale = new System.Windows.Forms.Label();
            this.cboN_FFT = new System.Windows.Forms.ComboBox();
            this.txtSpectrumScale = new System.Windows.Forms.TextBox();
            this.lblN_FFT = new System.Windows.Forms.Label();
            this.btnCommitFFTSettings = new System.Windows.Forms.Button();
            this.tabArduino = new System.Windows.Forms.TabPage();
            this.btnTest = new System.Windows.Forms.Button();
            this.pnlSpectrum = new AudioAnalysis.Spectrum();
            this.btnDecreaseMax = new System.Windows.Forms.Button();
            this.btnIncreaseMax = new System.Windows.Forms.Button();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnMinus10Percent = new System.Windows.Forms.Button();
            this.btnPlus10Percent = new System.Windows.Forms.Button();
            this.pnlNewAudio = new System.Windows.Forms.Panel();
            this.txtNewAudio3 = new System.Windows.Forms.TextBox();
            this.txtNewAudio2 = new System.Windows.Forms.TextBox();
            this.txtNewAudio1 = new System.Windows.Forms.TextBox();
            this.pnlAccumAudio = new System.Windows.Forms.Panel();
            this.txtAccumAudio3 = new System.Windows.Forms.TextBox();
            this.txtAccumAudio2 = new System.Windows.Forms.TextBox();
            this.txtAccumAudio1 = new System.Windows.Forms.TextBox();
            this.pnlBars = new System.Windows.Forms.Panel();
            this.barRange3 = new System.Windows.Forms.ProgressBar();
            this.barRange1 = new System.Windows.Forms.ProgressBar();
            this.barRange2 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrThreshold)).BeginInit();
            this.pnlRangeButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMax)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabFFT.SuspendLayout();
            this.tabArduino.SuspendLayout();
            this.pnlSpectrum.SuspendLayout();
            this.pnlNewAudio.SuspendLayout();
            this.pnlAccumAudio.SuspendLayout();
            this.pnlBars.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(57, 440);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Black;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(999, 195);
            this.chart1.TabIndex = 16;
            this.chart1.Text = "chart1";
            // 
            // trkbrThreshold
            // 
            this.trkbrThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trkbrThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrThreshold.Location = new System.Drawing.Point(6, 12);
            this.trkbrThreshold.Maximum = 10000;
            this.trkbrThreshold.Name = "trkbrThreshold";
            this.trkbrThreshold.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkbrThreshold.Size = new System.Drawing.Size(45, 577);
            this.trkbrThreshold.TabIndex = 17;
            this.trkbrThreshold.TickFrequency = 10;
            this.trkbrThreshold.ValueChanged += new System.EventHandler(this.trkbrThreshold_ValueChanged);
            // 
            // txtThreshold
            // 
            this.txtThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtThreshold.BackColor = System.Drawing.Color.Black;
            this.txtThreshold.ForeColor = System.Drawing.Color.White;
            this.txtThreshold.Location = new System.Drawing.Point(6, 595);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshold.Size = new System.Drawing.Size(42, 20);
            this.txtThreshold.TabIndex = 4;
            this.txtThreshold.Text = "0";
            this.txtThreshold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtThreshold_KeyDown);
            // 
            // pnlRangeButtons
            // 
            this.pnlRangeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRangeButtons.Controls.Add(this.btnTest);
            this.pnlRangeButtons.Controls.Add(this.btnRange3);
            this.pnlRangeButtons.Controls.Add(this.btnRange2);
            this.pnlRangeButtons.Controls.Add(this.btnSpectrumMode);
            this.pnlRangeButtons.Controls.Add(this.btnRange1);
            this.pnlRangeButtons.Controls.Add(this.btnAutoRange);
            this.pnlRangeButtons.Controls.Add(this.btnSaveSong);
            this.pnlRangeButtons.Controls.Add(this.cboSongNames);
            this.pnlRangeButtons.Location = new System.Drawing.Point(57, 378);
            this.pnlRangeButtons.Name = "pnlRangeButtons";
            this.pnlRangeButtons.Size = new System.Drawing.Size(1000, 65);
            this.pnlRangeButtons.TabIndex = 20;
            // 
            // btnRange3
            // 
            this.btnRange3.BackColor = System.Drawing.SystemColors.Control;
            this.btnRange3.Location = new System.Drawing.Point(159, 10);
            this.btnRange3.Name = "btnRange3";
            this.btnRange3.Size = new System.Drawing.Size(72, 48);
            this.btnRange3.TabIndex = 2;
            this.btnRange3.Text = "Range 3";
            this.btnRange3.UseVisualStyleBackColor = false;
            this.btnRange3.Click += new System.EventHandler(this.btnRange3_Click);
            // 
            // btnRange2
            // 
            this.btnRange2.BackColor = System.Drawing.SystemColors.Control;
            this.btnRange2.Location = new System.Drawing.Point(81, 10);
            this.btnRange2.Name = "btnRange2";
            this.btnRange2.Size = new System.Drawing.Size(72, 48);
            this.btnRange2.TabIndex = 1;
            this.btnRange2.Text = "Range 2";
            this.btnRange2.UseVisualStyleBackColor = false;
            this.btnRange2.Click += new System.EventHandler(this.btnRange2_Click);
            // 
            // btnRange1
            // 
            this.btnRange1.BackColor = System.Drawing.SystemColors.Control;
            this.btnRange1.Location = new System.Drawing.Point(3, 10);
            this.btnRange1.Name = "btnRange1";
            this.btnRange1.Size = new System.Drawing.Size(72, 48);
            this.btnRange1.TabIndex = 0;
            this.btnRange1.Text = "Range 1";
            this.btnRange1.UseVisualStyleBackColor = false;
            this.btnRange1.Click += new System.EventHandler(this.btnRange1_Click);
            // 
            // trkbrMin
            // 
            this.trkbrMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkbrMin.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrMin.Location = new System.Drawing.Point(45, 310);
            this.trkbrMin.Name = "trkbrMin";
            this.trkbrMin.Size = new System.Drawing.Size(1020, 45);
            this.trkbrMin.TabIndex = 21;
            this.trkbrMin.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkbrMin.ValueChanged += new System.EventHandler(this.trkbrMin_ValueChanged);
            // 
            // trkbrMax
            // 
            this.trkbrMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkbrMax.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrMax.Location = new System.Drawing.Point(45, 339);
            this.trkbrMax.Name = "trkbrMax";
            this.trkbrMax.Size = new System.Drawing.Size(1020, 45);
            this.trkbrMax.TabIndex = 22;
            this.trkbrMax.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkbrMax.ValueChanged += new System.EventHandler(this.trkbrMax_ValueChanged);
            // 
            // txtTimer1Interval
            // 
            this.txtTimer1Interval.BackColor = System.Drawing.Color.Black;
            this.txtTimer1Interval.ForeColor = System.Drawing.Color.White;
            this.txtTimer1Interval.Location = new System.Drawing.Point(6, 86);
            this.txtTimer1Interval.Name = "txtTimer1Interval";
            this.txtTimer1Interval.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTimer1Interval.Size = new System.Drawing.Size(62, 20);
            this.txtTimer1Interval.TabIndex = 24;
            this.txtTimer1Interval.Text = "0";
            this.txtTimer1Interval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimer1Interval_KeyDown);
            // 
            // lblRefreshRate
            // 
            this.lblRefreshRate.AutoSize = true;
            this.lblRefreshRate.Location = new System.Drawing.Point(74, 89);
            this.lblRefreshRate.Name = "lblRefreshRate";
            this.lblRefreshRate.Size = new System.Drawing.Size(92, 13);
            this.lblRefreshRate.TabIndex = 26;
            this.lblRefreshRate.Text = "Refresh Rate (ms)";
            // 
            // lblThreshold
            // 
            this.lblThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(3, 618);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(54, 13);
            this.lblThreshold.TabIndex = 27;
            this.lblThreshold.Text = "Threshold";
            // 
            // cboSongNames
            // 
            this.cboSongNames.BackColor = System.Drawing.Color.Black;
            this.cboSongNames.ForeColor = System.Drawing.Color.White;
            this.cboSongNames.FormattingEnabled = true;
            this.cboSongNames.Location = new System.Drawing.Point(705, 5);
            this.cboSongNames.Name = "cboSongNames";
            this.cboSongNames.Size = new System.Drawing.Size(211, 21);
            this.cboSongNames.TabIndex = 28;
            this.cboSongNames.SelectionChangeCommitted += new System.EventHandler(this.cboSongNames_SelectionChangeCommitted);
            // 
            // btnSaveSong
            // 
            this.btnSaveSong.BackColor = System.Drawing.Color.Black;
            this.btnSaveSong.ForeColor = System.Drawing.Color.White;
            this.btnSaveSong.Location = new System.Drawing.Point(922, 3);
            this.btnSaveSong.Name = "btnSaveSong";
            this.btnSaveSong.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSong.TabIndex = 29;
            this.btnSaveSong.Text = "Save";
            this.btnSaveSong.UseVisualStyleBackColor = false;
            this.btnSaveSong.Click += new System.EventHandler(this.btnSaveSong_Click);
            // 
            // btnArduino
            // 
            this.btnArduino.Location = new System.Drawing.Point(3, 30);
            this.btnArduino.Name = "btnArduino";
            this.btnArduino.Size = new System.Drawing.Size(170, 39);
            this.btnArduino.TabIndex = 30;
            this.btnArduino.Text = "Execute";
            this.btnArduino.UseVisualStyleBackColor = true;
            this.btnArduino.Click += new System.EventHandler(this.btnArduino_Click);
            // 
            // cboArduinoCommands
            // 
            this.cboArduinoCommands.FormattingEnabled = true;
            this.cboArduinoCommands.Location = new System.Drawing.Point(3, 3);
            this.cboArduinoCommands.Name = "cboArduinoCommands";
            this.cboArduinoCommands.Size = new System.Drawing.Size(170, 21);
            this.cboArduinoCommands.TabIndex = 31;
            // 
            // btnAutoRange
            // 
            this.btnAutoRange.Location = new System.Drawing.Point(705, 38);
            this.btnAutoRange.Name = "btnAutoRange";
            this.btnAutoRange.Size = new System.Drawing.Size(75, 23);
            this.btnAutoRange.TabIndex = 32;
            this.btnAutoRange.Text = "Auto Range";
            this.btnAutoRange.UseVisualStyleBackColor = true;
            this.btnAutoRange.Click += new System.EventHandler(this.btnAutoRange_Click);
            // 
            // btnSpectrumMode
            // 
            this.btnSpectrumMode.Location = new System.Drawing.Point(786, 38);
            this.btnSpectrumMode.Name = "btnSpectrumMode";
            this.btnSpectrumMode.Size = new System.Drawing.Size(130, 23);
            this.btnSpectrumMode.TabIndex = 33;
            this.btnSpectrumMode.Text = "Spectrum Mode";
            this.btnSpectrumMode.UseVisualStyleBackColor = true;
            this.btnSpectrumMode.Click += new System.EventHandler(this.btnSpectrumMode_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabFFT);
            this.tabControl1.Controls.Add(this.tabArduino);
            this.tabControl1.Location = new System.Drawing.Point(1059, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(184, 623);
            this.tabControl1.TabIndex = 34;
            // 
            // tabFFT
            // 
            this.tabFFT.Controls.Add(this.txtDropOffScale);
            this.tabFFT.Controls.Add(this.lblDropOffScale);
            this.tabFFT.Controls.Add(this.lblSpectrumScale);
            this.tabFFT.Controls.Add(this.cboN_FFT);
            this.tabFFT.Controls.Add(this.txtSpectrumScale);
            this.tabFFT.Controls.Add(this.lblN_FFT);
            this.tabFFT.Controls.Add(this.btnCommitFFTSettings);
            this.tabFFT.Controls.Add(this.txtTimer1Interval);
            this.tabFFT.Controls.Add(this.lblRefreshRate);
            this.tabFFT.Location = new System.Drawing.Point(4, 22);
            this.tabFFT.Name = "tabFFT";
            this.tabFFT.Padding = new System.Windows.Forms.Padding(3);
            this.tabFFT.Size = new System.Drawing.Size(176, 597);
            this.tabFFT.TabIndex = 0;
            this.tabFFT.Text = "FFT";
            this.tabFFT.UseVisualStyleBackColor = true;
            // 
            // txtDropOffScale
            // 
            this.txtDropOffScale.BackColor = System.Drawing.Color.Black;
            this.txtDropOffScale.ForeColor = System.Drawing.Color.White;
            this.txtDropOffScale.Location = new System.Drawing.Point(6, 112);
            this.txtDropOffScale.Name = "txtDropOffScale";
            this.txtDropOffScale.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDropOffScale.Size = new System.Drawing.Size(82, 20);
            this.txtDropOffScale.TabIndex = 40;
            this.txtDropOffScale.Text = "0";
            // 
            // lblDropOffScale
            // 
            this.lblDropOffScale.AutoSize = true;
            this.lblDropOffScale.Location = new System.Drawing.Point(94, 115);
            this.lblDropOffScale.Name = "lblDropOffScale";
            this.lblDropOffScale.Size = new System.Drawing.Size(72, 13);
            this.lblDropOffScale.TabIndex = 41;
            this.lblDropOffScale.Text = "Dropoff Scale";
            // 
            // lblSpectrumScale
            // 
            this.lblSpectrumScale.AutoSize = true;
            this.lblSpectrumScale.Location = new System.Drawing.Point(84, 63);
            this.lblSpectrumScale.Name = "lblSpectrumScale";
            this.lblSpectrumScale.Size = new System.Drawing.Size(82, 13);
            this.lblSpectrumScale.TabIndex = 36;
            this.lblSpectrumScale.Text = "Spectrum Scale";
            // 
            // cboN_FFT
            // 
            this.cboN_FFT.BackColor = System.Drawing.Color.Black;
            this.cboN_FFT.ForeColor = System.Drawing.Color.White;
            this.cboN_FFT.FormattingEnabled = true;
            this.cboN_FFT.Location = new System.Drawing.Point(6, 33);
            this.cboN_FFT.Name = "cboN_FFT";
            this.cboN_FFT.Size = new System.Drawing.Size(114, 21);
            this.cboN_FFT.TabIndex = 35;
            this.cboN_FFT.Tag = "";
            // 
            // txtSpectrumScale
            // 
            this.txtSpectrumScale.BackColor = System.Drawing.Color.Black;
            this.txtSpectrumScale.ForeColor = System.Drawing.Color.White;
            this.txtSpectrumScale.Location = new System.Drawing.Point(6, 60);
            this.txtSpectrumScale.Name = "txtSpectrumScale";
            this.txtSpectrumScale.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSpectrumScale.Size = new System.Drawing.Size(72, 20);
            this.txtSpectrumScale.TabIndex = 35;
            this.txtSpectrumScale.Text = "0";
            // 
            // lblN_FFT
            // 
            this.lblN_FFT.AutoSize = true;
            this.lblN_FFT.Location = new System.Drawing.Point(126, 36);
            this.lblN_FFT.Name = "lblN_FFT";
            this.lblN_FFT.Size = new System.Drawing.Size(40, 13);
            this.lblN_FFT.TabIndex = 39;
            this.lblN_FFT.Text = "N_FFT";
            // 
            // btnCommitFFTSettings
            // 
            this.btnCommitFFTSettings.Location = new System.Drawing.Point(6, 4);
            this.btnCommitFFTSettings.Name = "btnCommitFFTSettings";
            this.btnCommitFFTSettings.Size = new System.Drawing.Size(164, 23);
            this.btnCommitFFTSettings.TabIndex = 38;
            this.btnCommitFFTSettings.Text = "Commit";
            this.btnCommitFFTSettings.UseVisualStyleBackColor = true;
            this.btnCommitFFTSettings.Click += new System.EventHandler(this.btnCommitFFTSettings_Click);
            // 
            // tabArduino
            // 
            this.tabArduino.Controls.Add(this.cboArduinoCommands);
            this.tabArduino.Controls.Add(this.btnArduino);
            this.tabArduino.Location = new System.Drawing.Point(4, 22);
            this.tabArduino.Name = "tabArduino";
            this.tabArduino.Size = new System.Drawing.Size(176, 597);
            this.tabArduino.TabIndex = 1;
            this.tabArduino.Text = "Arduino";
            this.tabArduino.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.SystemColors.Control;
            this.btnTest.Location = new System.Drawing.Point(627, 5);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(72, 56);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Controls.Add(this.btnDecreaseMax);
            this.pnlSpectrum.Controls.Add(this.btnIncreaseMax);
            this.pnlSpectrum.Controls.Add(this.btnCalibrate);
            this.pnlSpectrum.Controls.Add(this.btnMinus10Percent);
            this.pnlSpectrum.Controls.Add(this.btnPlus10Percent);
            this.pnlSpectrum.Controls.Add(this.pnlNewAudio);
            this.pnlSpectrum.Controls.Add(this.pnlAccumAudio);
            this.pnlSpectrum.Controls.Add(this.pnlBars);
            this.pnlSpectrum.Location = new System.Drawing.Point(57, 12);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(999, 303);
            this.pnlSpectrum.TabIndex = 19;
            this.pnlSpectrum.SizeChanged += new System.EventHandler(this.pnlSpectrum_SizeChanged);
            // 
            // btnDecreaseMax
            // 
            this.btnDecreaseMax.Location = new System.Drawing.Point(660, 277);
            this.btnDecreaseMax.Name = "btnDecreaseMax";
            this.btnDecreaseMax.Size = new System.Drawing.Size(52, 23);
            this.btnDecreaseMax.TabIndex = 37;
            this.btnDecreaseMax.Text = "MAX--";
            this.btnDecreaseMax.UseVisualStyleBackColor = true;
            this.btnDecreaseMax.Click += new System.EventHandler(this.btnDecreaseMax_Click);
            // 
            // btnIncreaseMax
            // 
            this.btnIncreaseMax.Location = new System.Drawing.Point(944, 277);
            this.btnIncreaseMax.Name = "btnIncreaseMax";
            this.btnIncreaseMax.Size = new System.Drawing.Size(52, 23);
            this.btnIncreaseMax.TabIndex = 36;
            this.btnIncreaseMax.Text = "MAX++";
            this.btnIncreaseMax.UseVisualStyleBackColor = true;
            this.btnIncreaseMax.Click += new System.EventHandler(this.btnIncreaseMax_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Location = new System.Drawing.Point(773, 277);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(110, 23);
            this.btnCalibrate.TabIndex = 35;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnMinus10Percent
            // 
            this.btnMinus10Percent.Location = new System.Drawing.Point(718, 277);
            this.btnMinus10Percent.Name = "btnMinus10Percent";
            this.btnMinus10Percent.Size = new System.Drawing.Size(49, 23);
            this.btnMinus10Percent.TabIndex = 34;
            this.btnMinus10Percent.Text = "-10%";
            this.btnMinus10Percent.UseVisualStyleBackColor = true;
            this.btnMinus10Percent.Click += new System.EventHandler(this.btnMinus10Percent_Click);
            // 
            // btnPlus10Percent
            // 
            this.btnPlus10Percent.Location = new System.Drawing.Point(889, 277);
            this.btnPlus10Percent.Name = "btnPlus10Percent";
            this.btnPlus10Percent.Size = new System.Drawing.Size(49, 23);
            this.btnPlus10Percent.TabIndex = 33;
            this.btnPlus10Percent.Text = "+10%";
            this.btnPlus10Percent.UseVisualStyleBackColor = true;
            this.btnPlus10Percent.Click += new System.EventHandler(this.btnPlus10Percent_Click);
            // 
            // pnlNewAudio
            // 
            this.pnlNewAudio.Controls.Add(this.txtNewAudio3);
            this.pnlNewAudio.Controls.Add(this.txtNewAudio2);
            this.pnlNewAudio.Controls.Add(this.txtNewAudio1);
            this.pnlNewAudio.Location = new System.Drawing.Point(732, 5);
            this.pnlNewAudio.Name = "pnlNewAudio";
            this.pnlNewAudio.Size = new System.Drawing.Size(74, 87);
            this.pnlNewAudio.TabIndex = 28;
            // 
            // txtNewAudio3
            // 
            this.txtNewAudio3.BackColor = System.Drawing.Color.Black;
            this.txtNewAudio3.ForeColor = System.Drawing.Color.White;
            this.txtNewAudio3.Location = new System.Drawing.Point(3, 61);
            this.txtNewAudio3.Name = "txtNewAudio3";
            this.txtNewAudio3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNewAudio3.Size = new System.Drawing.Size(67, 20);
            this.txtNewAudio3.TabIndex = 26;
            this.txtNewAudio3.Text = "0";
            // 
            // txtNewAudio2
            // 
            this.txtNewAudio2.BackColor = System.Drawing.Color.Black;
            this.txtNewAudio2.ForeColor = System.Drawing.Color.White;
            this.txtNewAudio2.Location = new System.Drawing.Point(3, 32);
            this.txtNewAudio2.Name = "txtNewAudio2";
            this.txtNewAudio2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNewAudio2.Size = new System.Drawing.Size(67, 20);
            this.txtNewAudio2.TabIndex = 25;
            this.txtNewAudio2.Text = "0";
            // 
            // txtNewAudio1
            // 
            this.txtNewAudio1.BackColor = System.Drawing.Color.Black;
            this.txtNewAudio1.ForeColor = System.Drawing.Color.White;
            this.txtNewAudio1.Location = new System.Drawing.Point(3, 3);
            this.txtNewAudio1.Name = "txtNewAudio1";
            this.txtNewAudio1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNewAudio1.Size = new System.Drawing.Size(67, 20);
            this.txtNewAudio1.TabIndex = 24;
            this.txtNewAudio1.Text = "0";
            // 
            // pnlAccumAudio
            // 
            this.pnlAccumAudio.Controls.Add(this.txtAccumAudio3);
            this.pnlAccumAudio.Controls.Add(this.txtAccumAudio2);
            this.pnlAccumAudio.Controls.Add(this.txtAccumAudio1);
            this.pnlAccumAudio.Location = new System.Drawing.Point(809, 5);
            this.pnlAccumAudio.Name = "pnlAccumAudio";
            this.pnlAccumAudio.Size = new System.Drawing.Size(74, 87);
            this.pnlAccumAudio.TabIndex = 27;
            // 
            // txtAccumAudio3
            // 
            this.txtAccumAudio3.BackColor = System.Drawing.Color.Black;
            this.txtAccumAudio3.ForeColor = System.Drawing.Color.White;
            this.txtAccumAudio3.Location = new System.Drawing.Point(3, 61);
            this.txtAccumAudio3.Name = "txtAccumAudio3";
            this.txtAccumAudio3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAccumAudio3.Size = new System.Drawing.Size(67, 20);
            this.txtAccumAudio3.TabIndex = 26;
            this.txtAccumAudio3.Text = "0";
            // 
            // txtAccumAudio2
            // 
            this.txtAccumAudio2.BackColor = System.Drawing.Color.Black;
            this.txtAccumAudio2.ForeColor = System.Drawing.Color.White;
            this.txtAccumAudio2.Location = new System.Drawing.Point(3, 32);
            this.txtAccumAudio2.Name = "txtAccumAudio2";
            this.txtAccumAudio2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAccumAudio2.Size = new System.Drawing.Size(67, 20);
            this.txtAccumAudio2.TabIndex = 25;
            this.txtAccumAudio2.Text = "0";
            // 
            // txtAccumAudio1
            // 
            this.txtAccumAudio1.BackColor = System.Drawing.Color.Black;
            this.txtAccumAudio1.ForeColor = System.Drawing.Color.White;
            this.txtAccumAudio1.Location = new System.Drawing.Point(3, 3);
            this.txtAccumAudio1.Name = "txtAccumAudio1";
            this.txtAccumAudio1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAccumAudio1.Size = new System.Drawing.Size(67, 20);
            this.txtAccumAudio1.TabIndex = 24;
            this.txtAccumAudio1.Text = "0";
            // 
            // pnlBars
            // 
            this.pnlBars.Controls.Add(this.barRange3);
            this.pnlBars.Controls.Add(this.barRange1);
            this.pnlBars.Controls.Add(this.barRange2);
            this.pnlBars.Location = new System.Drawing.Point(889, 3);
            this.pnlBars.Name = "pnlBars";
            this.pnlBars.Size = new System.Drawing.Size(107, 89);
            this.pnlBars.TabIndex = 0;
            // 
            // barRange3
            // 
            this.barRange3.Location = new System.Drawing.Point(3, 63);
            this.barRange3.Name = "barRange3";
            this.barRange3.Size = new System.Drawing.Size(100, 23);
            this.barRange3.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barRange3.TabIndex = 2;
            // 
            // barRange1
            // 
            this.barRange1.Location = new System.Drawing.Point(3, 5);
            this.barRange1.Name = "barRange1";
            this.barRange1.Size = new System.Drawing.Size(100, 23);
            this.barRange1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barRange1.TabIndex = 0;
            // 
            // barRange2
            // 
            this.barRange2.Location = new System.Drawing.Point(3, 34);
            this.barRange2.Name = "barRange2";
            this.barRange2.Size = new System.Drawing.Size(100, 23);
            this.barRange2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.barRange2.TabIndex = 1;
            // 
            // frmAudioAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1241, 635);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.trkbrMax);
            this.Controls.Add(this.pnlRangeButtons);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.trkbrThreshold);
            this.Controls.Add(this.pnlSpectrum);
            this.Controls.Add(this.trkbrMin);
            this.Name = "frmAudioAnalysis";
            this.Text = "Mark\'s Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AudioAnalysis_FormClosing);
            this.Load += new System.EventHandler(this.frmAudioAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrThreshold)).EndInit();
            this.pnlRangeButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMax)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabFFT.ResumeLayout(false);
            this.tabFFT.PerformLayout();
            this.tabArduino.ResumeLayout(false);
            this.pnlSpectrum.ResumeLayout(false);
            this.pnlNewAudio.ResumeLayout(false);
            this.pnlNewAudio.PerformLayout();
            this.pnlAccumAudio.ResumeLayout(false);
            this.pnlAccumAudio.PerformLayout();
            this.pnlBars.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TrackBar trkbrThreshold;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Panel pnlRangeButtons;
        private System.Windows.Forms.Button btnRange3;
        private System.Windows.Forms.Button btnRange2;
        private System.Windows.Forms.Button btnRange1;
        private System.Windows.Forms.TrackBar trkbrMin;
        private System.Windows.Forms.TrackBar trkbrMax;
        private System.Windows.Forms.Panel pnlBars;
        private System.Windows.Forms.ProgressBar barRange3;
        private System.Windows.Forms.ProgressBar barRange2;
        private System.Windows.Forms.ProgressBar barRange1;
        private System.Windows.Forms.TextBox txtAccumAudio3;
        private System.Windows.Forms.TextBox txtAccumAudio2;
        private System.Windows.Forms.TextBox txtAccumAudio1;
        private System.Windows.Forms.Panel pnlAccumAudio;
        private System.Windows.Forms.TextBox txtTimer1Interval;
        private System.Windows.Forms.Panel pnlNewAudio;
        private System.Windows.Forms.TextBox txtNewAudio3;
        private System.Windows.Forms.TextBox txtNewAudio2;
        private System.Windows.Forms.TextBox txtNewAudio1;
        private System.Windows.Forms.Label lblRefreshRate;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.ComboBox cboSongNames;
        private System.Windows.Forms.Button btnSaveSong;
        private System.Windows.Forms.Button btnArduino;
        private System.Windows.Forms.ComboBox cboArduinoCommands;
        private System.Windows.Forms.Button btnMinus10Percent;
        private System.Windows.Forms.Button btnPlus10Percent;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Button btnDecreaseMax;
        private System.Windows.Forms.Button btnIncreaseMax;
        private System.Windows.Forms.Button btnAutoRange;
        private System.Windows.Forms.Button btnSpectrumMode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFFT;
        private System.Windows.Forms.Label lblN_FFT;
        private System.Windows.Forms.Button btnCommitFFTSettings;
        private System.Windows.Forms.ComboBox cboN_FFT;
        private System.Windows.Forms.Label lblSpectrumScale;
        private System.Windows.Forms.TextBox txtSpectrumScale;
        private System.Windows.Forms.TabPage tabArduino;
        private System.Windows.Forms.TextBox txtDropOffScale;
        private System.Windows.Forms.Label lblDropOffScale;
        private System.Windows.Forms.Button btnTest;
        private Spectrum pnlSpectrum;
    }
}

