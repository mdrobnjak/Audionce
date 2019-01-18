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
            this.btnDecrementRangeBand = new System.Windows.Forms.Button();
            this.btnIncrementRangeBand = new System.Windows.Forms.Button();
            this.btnRange3 = new System.Windows.Forms.Button();
            this.btnRange2 = new System.Windows.Forms.Button();
            this.btnRange1 = new System.Windows.Forms.Button();
            this.btnSaveSong = new System.Windows.Forms.Button();
            this.cboSongNames = new System.Windows.Forms.ComboBox();
            this.trkbrMin = new System.Windows.Forms.TrackBar();
            this.trkbrMax = new System.Windows.Forms.TrackBar();
            this.txtTimer1Interval = new System.Windows.Forms.TextBox();
            this.lblRefreshRate = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.btnWriteArduino = new System.Windows.Forms.Button();
            this.cboArduinoCommands = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFFT = new System.Windows.Forms.TabPage();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.txtDropOffScale = new System.Windows.Forms.TextBox();
            this.lblDropOffScale = new System.Windows.Forms.Label();
            this.lblSpectrumScale = new System.Windows.Forms.Label();
            this.cboN_FFT = new System.Windows.Forms.ComboBox();
            this.txtSpectrumScale = new System.Windows.Forms.TextBox();
            this.lblN_FFT = new System.Windows.Forms.Label();
            this.btnCommitFFTSettings = new System.Windows.Forms.Button();
            this.tabPostProcessing = new System.Windows.Forms.TabPage();
            this.lblGetsSubtractedBy = new System.Windows.Forms.Label();
            this.cboSubtractor = new System.Windows.Forms.ComboBox();
            this.cboSubtractFrom = new System.Windows.Forms.ComboBox();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.tabAutoSettings = new System.Windows.Forms.TabPage();
            this.txtBandwidth3 = new System.Windows.Forms.TextBox();
            this.txtBandwidth2 = new System.Windows.Forms.TextBox();
            this.btnDynamicThresholds = new System.Windows.Forms.Button();
            this.txtThreshMultiplier3 = new System.Windows.Forms.TextBox();
            this.txtThreshMultiplier2 = new System.Windows.Forms.TextBox();
            this.txtThreshMultiplier1 = new System.Windows.Forms.TextBox();
            this.lblThreshMultipliers = new System.Windows.Forms.Label();
            this.txtBandwidth1 = new System.Windows.Forms.TextBox();
            this.lblBandwidths = new System.Windows.Forms.Label();
            this.txtSeconds = new System.Windows.Forms.TextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.btnCommitAutoSettings = new System.Windows.Forms.Button();
            this.tabArduino = new System.Windows.Forms.TabPage();
            this.lblArduinoMRange = new System.Windows.Forms.Label();
            this.btnArduinoMRange3 = new System.Windows.Forms.Button();
            this.btnArduinoMRange2 = new System.Windows.Forms.Button();
            this.cboPortNames = new System.Windows.Forms.ComboBox();
            this.pnlSpectrum = new AudioAnalysis.Spectrum();
            this.btnToggleBars = new System.Windows.Forms.Button();
            this.btnToggleSpectrum = new System.Windows.Forms.Button();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnSpectrumMode = new System.Windows.Forms.Button();
            this.btnAutoRange = new System.Windows.Forms.Button();
            this.pnlBars = new System.Windows.Forms.Panel();
            this.barRange3 = new System.Windows.Forms.ProgressBar();
            this.barRange1 = new System.Windows.Forms.ProgressBar();
            this.barRange2 = new System.Windows.Forms.ProgressBar();
            this.btnToggleChart = new System.Windows.Forms.Button();
            this.btnDisableAllGraphics = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrThreshold)).BeginInit();
            this.pnlRangeButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMax)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabFFT.SuspendLayout();
            this.tabPostProcessing.SuspendLayout();
            this.tabAutoSettings.SuspendLayout();
            this.tabArduino.SuspendLayout();
            this.pnlSpectrum.SuspendLayout();
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
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
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
            // 
            // pnlRangeButtons
            // 
            this.pnlRangeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRangeButtons.Controls.Add(this.btnDecrementRangeBand);
            this.pnlRangeButtons.Controls.Add(this.btnIncrementRangeBand);
            this.pnlRangeButtons.Controls.Add(this.btnRange3);
            this.pnlRangeButtons.Controls.Add(this.btnRange2);
            this.pnlRangeButtons.Controls.Add(this.btnRange1);
            this.pnlRangeButtons.Controls.Add(this.btnSaveSong);
            this.pnlRangeButtons.Controls.Add(this.cboSongNames);
            this.pnlRangeButtons.Location = new System.Drawing.Point(57, 378);
            this.pnlRangeButtons.Name = "pnlRangeButtons";
            this.pnlRangeButtons.Size = new System.Drawing.Size(1000, 65);
            this.pnlRangeButtons.TabIndex = 20;
            // 
            // btnDecrementRangeBand
            // 
            this.btnDecrementRangeBand.Location = new System.Drawing.Point(237, 37);
            this.btnDecrementRangeBand.Name = "btnDecrementRangeBand";
            this.btnDecrementRangeBand.Size = new System.Drawing.Size(72, 21);
            this.btnDecrementRangeBand.TabIndex = 31;
            this.btnDecrementRangeBand.Text = "Decrement";
            this.btnDecrementRangeBand.UseVisualStyleBackColor = true;
            this.btnDecrementRangeBand.Click += new System.EventHandler(this.btnDecrementRangeBand_Click);
            // 
            // btnIncrementRangeBand
            // 
            this.btnIncrementRangeBand.Location = new System.Drawing.Point(237, 10);
            this.btnIncrementRangeBand.Name = "btnIncrementRangeBand";
            this.btnIncrementRangeBand.Size = new System.Drawing.Size(72, 23);
            this.btnIncrementRangeBand.TabIndex = 30;
            this.btnIncrementRangeBand.Text = "Increment";
            this.btnIncrementRangeBand.UseVisualStyleBackColor = true;
            this.btnIncrementRangeBand.Click += new System.EventHandler(this.btnIncrementRangeBand_Click);
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
            // btnSaveSong
            // 
            this.btnSaveSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            // cboSongNames
            // 
            this.cboSongNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSongNames.BackColor = System.Drawing.Color.Black;
            this.cboSongNames.ForeColor = System.Drawing.Color.White;
            this.cboSongNames.FormattingEnabled = true;
            this.cboSongNames.Location = new System.Drawing.Point(705, 5);
            this.cboSongNames.Name = "cboSongNames";
            this.cboSongNames.Size = new System.Drawing.Size(211, 21);
            this.cboSongNames.TabIndex = 28;
            this.cboSongNames.SelectionChangeCommitted += new System.EventHandler(this.cboSongNames_SelectionChangeCommitted);
            // 
            // trkbrMin
            // 
            this.trkbrMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
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
            this.trkbrMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
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
            // btnWriteArduino
            // 
            this.btnWriteArduino.Location = new System.Drawing.Point(3, 80);
            this.btnWriteArduino.Name = "btnWriteArduino";
            this.btnWriteArduino.Size = new System.Drawing.Size(170, 39);
            this.btnWriteArduino.TabIndex = 30;
            this.btnWriteArduino.Text = "Write";
            this.btnWriteArduino.UseVisualStyleBackColor = true;
            this.btnWriteArduino.Click += new System.EventHandler(this.btnWriteArduino_Click);
            // 
            // cboArduinoCommands
            // 
            this.cboArduinoCommands.BackColor = System.Drawing.Color.Black;
            this.cboArduinoCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArduinoCommands.ForeColor = System.Drawing.Color.White;
            this.cboArduinoCommands.FormattingEnabled = true;
            this.cboArduinoCommands.Location = new System.Drawing.Point(3, 53);
            this.cboArduinoCommands.Name = "cboArduinoCommands";
            this.cboArduinoCommands.Size = new System.Drawing.Size(170, 21);
            this.cboArduinoCommands.TabIndex = 31;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabFFT);
            this.tabControl1.Controls.Add(this.tabPostProcessing);
            this.tabControl1.Controls.Add(this.tabAutoSettings);
            this.tabControl1.Controls.Add(this.tabArduino);
            this.tabControl1.Location = new System.Drawing.Point(1059, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(184, 623);
            this.tabControl1.TabIndex = 34;
            // 
            // tabFFT
            // 
            this.tabFFT.Controls.Add(this.txtMode);
            this.tabFFT.Controls.Add(this.lblMode);
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
            // txtMode
            // 
            this.txtMode.BackColor = System.Drawing.Color.Black;
            this.txtMode.ForeColor = System.Drawing.Color.White;
            this.txtMode.Location = new System.Drawing.Point(6, 138);
            this.txtMode.Name = "txtMode";
            this.txtMode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMode.Size = new System.Drawing.Size(120, 20);
            this.txtMode.TabIndex = 42;
            this.txtMode.Text = "0";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(132, 141);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(34, 13);
            this.lblMode.TabIndex = 43;
            this.lblMode.Text = "Mode";
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
            this.cboN_FFT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            // tabPostProcessing
            // 
            this.tabPostProcessing.Controls.Add(this.lblGetsSubtractedBy);
            this.tabPostProcessing.Controls.Add(this.cboSubtractor);
            this.tabPostProcessing.Controls.Add(this.cboSubtractFrom);
            this.tabPostProcessing.Controls.Add(this.btnSubtract);
            this.tabPostProcessing.Location = new System.Drawing.Point(4, 22);
            this.tabPostProcessing.Name = "tabPostProcessing";
            this.tabPostProcessing.Size = new System.Drawing.Size(176, 597);
            this.tabPostProcessing.TabIndex = 3;
            this.tabPostProcessing.Text = "Post";
            this.tabPostProcessing.UseVisualStyleBackColor = true;
            // 
            // lblGetsSubtractedBy
            // 
            this.lblGetsSubtractedBy.AutoSize = true;
            this.lblGetsSubtractedBy.Location = new System.Drawing.Point(38, 29);
            this.lblGetsSubtractedBy.Name = "lblGetsSubtractedBy";
            this.lblGetsSubtractedBy.Size = new System.Drawing.Size(97, 13);
            this.lblGetsSubtractedBy.TabIndex = 38;
            this.lblGetsSubtractedBy.Text = "gets subtracted by:";
            // 
            // cboSubtractor
            // 
            this.cboSubtractor.BackColor = System.Drawing.Color.Black;
            this.cboSubtractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubtractor.ForeColor = System.Drawing.Color.White;
            this.cboSubtractor.FormattingEnabled = true;
            this.cboSubtractor.Location = new System.Drawing.Point(3, 49);
            this.cboSubtractor.Name = "cboSubtractor";
            this.cboSubtractor.Size = new System.Drawing.Size(170, 21);
            this.cboSubtractor.TabIndex = 37;
            this.cboSubtractor.Tag = "";
            // 
            // cboSubtractFrom
            // 
            this.cboSubtractFrom.BackColor = System.Drawing.Color.Black;
            this.cboSubtractFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubtractFrom.ForeColor = System.Drawing.Color.White;
            this.cboSubtractFrom.FormattingEnabled = true;
            this.cboSubtractFrom.Location = new System.Drawing.Point(3, 3);
            this.cboSubtractFrom.Name = "cboSubtractFrom";
            this.cboSubtractFrom.Size = new System.Drawing.Size(170, 21);
            this.cboSubtractFrom.TabIndex = 36;
            this.cboSubtractFrom.Tag = "";
            // 
            // btnSubtract
            // 
            this.btnSubtract.Location = new System.Drawing.Point(3, 76);
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.Size = new System.Drawing.Size(170, 42);
            this.btnSubtract.TabIndex = 0;
            this.btnSubtract.Text = "Subtract";
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.btnSubtract_Click);
            // 
            // tabAutoSettings
            // 
            this.tabAutoSettings.Controls.Add(this.txtBandwidth3);
            this.tabAutoSettings.Controls.Add(this.txtBandwidth2);
            this.tabAutoSettings.Controls.Add(this.btnDynamicThresholds);
            this.tabAutoSettings.Controls.Add(this.txtThreshMultiplier3);
            this.tabAutoSettings.Controls.Add(this.txtThreshMultiplier2);
            this.tabAutoSettings.Controls.Add(this.txtThreshMultiplier1);
            this.tabAutoSettings.Controls.Add(this.lblThreshMultipliers);
            this.tabAutoSettings.Controls.Add(this.txtBandwidth1);
            this.tabAutoSettings.Controls.Add(this.lblBandwidths);
            this.tabAutoSettings.Controls.Add(this.txtSeconds);
            this.tabAutoSettings.Controls.Add(this.lblSeconds);
            this.tabAutoSettings.Controls.Add(this.btnCommitAutoSettings);
            this.tabAutoSettings.Location = new System.Drawing.Point(4, 22);
            this.tabAutoSettings.Name = "tabAutoSettings";
            this.tabAutoSettings.Size = new System.Drawing.Size(176, 597);
            this.tabAutoSettings.TabIndex = 2;
            this.tabAutoSettings.Text = "Auto";
            this.tabAutoSettings.UseVisualStyleBackColor = true;
            // 
            // txtBandwidth3
            // 
            this.txtBandwidth3.BackColor = System.Drawing.Color.Black;
            this.txtBandwidth3.ForeColor = System.Drawing.Color.White;
            this.txtBandwidth3.Location = new System.Drawing.Point(117, 86);
            this.txtBandwidth3.Name = "txtBandwidth3";
            this.txtBandwidth3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBandwidth3.Size = new System.Drawing.Size(49, 20);
            this.txtBandwidth3.TabIndex = 53;
            this.txtBandwidth3.Text = "0";
            // 
            // txtBandwidth2
            // 
            this.txtBandwidth2.BackColor = System.Drawing.Color.Black;
            this.txtBandwidth2.ForeColor = System.Drawing.Color.White;
            this.txtBandwidth2.Location = new System.Drawing.Point(62, 86);
            this.txtBandwidth2.Name = "txtBandwidth2";
            this.txtBandwidth2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBandwidth2.Size = new System.Drawing.Size(49, 20);
            this.txtBandwidth2.TabIndex = 52;
            this.txtBandwidth2.Text = "0";
            // 
            // btnDynamicThresholds
            // 
            this.btnDynamicThresholds.Location = new System.Drawing.Point(3, 165);
            this.btnDynamicThresholds.Name = "btnDynamicThresholds";
            this.btnDynamicThresholds.Size = new System.Drawing.Size(163, 32);
            this.btnDynamicThresholds.TabIndex = 50;
            this.btnDynamicThresholds.Text = "Dynamic Thresholds";
            this.btnDynamicThresholds.UseVisualStyleBackColor = true;
            this.btnDynamicThresholds.Click += new System.EventHandler(this.btnDynamicThresholds_Click);
            // 
            // txtThreshMultiplier3
            // 
            this.txtThreshMultiplier3.BackColor = System.Drawing.Color.Black;
            this.txtThreshMultiplier3.ForeColor = System.Drawing.Color.White;
            this.txtThreshMultiplier3.Location = new System.Drawing.Point(117, 139);
            this.txtThreshMultiplier3.Name = "txtThreshMultiplier3";
            this.txtThreshMultiplier3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshMultiplier3.Size = new System.Drawing.Size(49, 20);
            this.txtThreshMultiplier3.TabIndex = 49;
            this.txtThreshMultiplier3.Text = "0";
            // 
            // txtThreshMultiplier2
            // 
            this.txtThreshMultiplier2.BackColor = System.Drawing.Color.Black;
            this.txtThreshMultiplier2.ForeColor = System.Drawing.Color.White;
            this.txtThreshMultiplier2.Location = new System.Drawing.Point(62, 139);
            this.txtThreshMultiplier2.Name = "txtThreshMultiplier2";
            this.txtThreshMultiplier2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshMultiplier2.Size = new System.Drawing.Size(49, 20);
            this.txtThreshMultiplier2.TabIndex = 48;
            this.txtThreshMultiplier2.Text = "0";
            // 
            // txtThreshMultiplier1
            // 
            this.txtThreshMultiplier1.BackColor = System.Drawing.Color.Black;
            this.txtThreshMultiplier1.ForeColor = System.Drawing.Color.White;
            this.txtThreshMultiplier1.Location = new System.Drawing.Point(6, 139);
            this.txtThreshMultiplier1.Name = "txtThreshMultiplier1";
            this.txtThreshMultiplier1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshMultiplier1.Size = new System.Drawing.Size(50, 20);
            this.txtThreshMultiplier1.TabIndex = 46;
            this.txtThreshMultiplier1.Text = "0";
            // 
            // lblThreshMultipliers
            // 
            this.lblThreshMultipliers.AutoSize = true;
            this.lblThreshMultipliers.Location = new System.Drawing.Point(31, 120);
            this.lblThreshMultipliers.Name = "lblThreshMultipliers";
            this.lblThreshMultipliers.Size = new System.Drawing.Size(103, 13);
            this.lblThreshMultipliers.TabIndex = 47;
            this.lblThreshMultipliers.Text = "Threshold Multipliers";
            // 
            // txtBandwidth1
            // 
            this.txtBandwidth1.BackColor = System.Drawing.Color.Black;
            this.txtBandwidth1.ForeColor = System.Drawing.Color.White;
            this.txtBandwidth1.Location = new System.Drawing.Point(6, 86);
            this.txtBandwidth1.Name = "txtBandwidth1";
            this.txtBandwidth1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBandwidth1.Size = new System.Drawing.Size(50, 20);
            this.txtBandwidth1.TabIndex = 44;
            this.txtBandwidth1.Text = "0";
            // 
            // lblBandwidths
            // 
            this.lblBandwidths.AutoSize = true;
            this.lblBandwidths.Location = new System.Drawing.Point(40, 66);
            this.lblBandwidths.Name = "lblBandwidths";
            this.lblBandwidths.Size = new System.Drawing.Size(92, 13);
            this.lblBandwidths.TabIndex = 45;
            this.lblBandwidths.Text = "Bandwidths (Bars)";
            // 
            // txtSeconds
            // 
            this.txtSeconds.BackColor = System.Drawing.Color.Black;
            this.txtSeconds.ForeColor = System.Drawing.Color.White;
            this.txtSeconds.Location = new System.Drawing.Point(6, 33);
            this.txtSeconds.Name = "txtSeconds";
            this.txtSeconds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSeconds.Size = new System.Drawing.Size(62, 20);
            this.txtSeconds.TabIndex = 42;
            this.txtSeconds.Text = "0";
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(74, 36);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(96, 13);
            this.lblSeconds.TabIndex = 43;
            this.lblSeconds.Text = "Seconds to Collect";
            // 
            // btnCommitAutoSettings
            // 
            this.btnCommitAutoSettings.Location = new System.Drawing.Point(6, 4);
            this.btnCommitAutoSettings.Name = "btnCommitAutoSettings";
            this.btnCommitAutoSettings.Size = new System.Drawing.Size(164, 23);
            this.btnCommitAutoSettings.TabIndex = 39;
            this.btnCommitAutoSettings.Text = "Commit";
            this.btnCommitAutoSettings.UseVisualStyleBackColor = true;
            this.btnCommitAutoSettings.Click += new System.EventHandler(this.btnCommitAutoSettings_Click);
            // 
            // tabArduino
            // 
            this.tabArduino.Controls.Add(this.btnDisableAllGraphics);
            this.tabArduino.Controls.Add(this.lblArduinoMRange);
            this.tabArduino.Controls.Add(this.btnArduinoMRange3);
            this.tabArduino.Controls.Add(this.btnArduinoMRange2);
            this.tabArduino.Controls.Add(this.cboPortNames);
            this.tabArduino.Controls.Add(this.cboArduinoCommands);
            this.tabArduino.Controls.Add(this.btnWriteArduino);
            this.tabArduino.Location = new System.Drawing.Point(4, 22);
            this.tabArduino.Name = "tabArduino";
            this.tabArduino.Size = new System.Drawing.Size(176, 597);
            this.tabArduino.TabIndex = 1;
            this.tabArduino.Text = "Arduino";
            this.tabArduino.UseVisualStyleBackColor = true;
            // 
            // lblArduinoMRange
            // 
            this.lblArduinoMRange.AutoSize = true;
            this.lblArduinoMRange.Location = new System.Drawing.Point(7, 156);
            this.lblArduinoMRange.Name = "lblArduinoMRange";
            this.lblArduinoMRange.Size = new System.Drawing.Size(57, 13);
            this.lblArduinoMRange.TabIndex = 35;
            this.lblArduinoMRange.Text = "\'m\' Range:";
            // 
            // btnArduinoMRange3
            // 
            this.btnArduinoMRange3.Location = new System.Drawing.Point(91, 175);
            this.btnArduinoMRange3.Name = "btnArduinoMRange3";
            this.btnArduinoMRange3.Size = new System.Drawing.Size(75, 32);
            this.btnArduinoMRange3.TabIndex = 34;
            this.btnArduinoMRange3.Text = "Range 3";
            this.btnArduinoMRange3.UseVisualStyleBackColor = true;
            this.btnArduinoMRange3.Click += new System.EventHandler(this.btnArduinoMRange3_Click);
            // 
            // btnArduinoMRange2
            // 
            this.btnArduinoMRange2.Location = new System.Drawing.Point(10, 175);
            this.btnArduinoMRange2.Name = "btnArduinoMRange2";
            this.btnArduinoMRange2.Size = new System.Drawing.Size(75, 32);
            this.btnArduinoMRange2.TabIndex = 33;
            this.btnArduinoMRange2.Text = "Range 2";
            this.btnArduinoMRange2.UseVisualStyleBackColor = true;
            this.btnArduinoMRange2.Click += new System.EventHandler(this.btnArduinoMRange2_Click);
            // 
            // cboPortNames
            // 
            this.cboPortNames.BackColor = System.Drawing.Color.Black;
            this.cboPortNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPortNames.ForeColor = System.Drawing.Color.White;
            this.cboPortNames.FormattingEnabled = true;
            this.cboPortNames.Location = new System.Drawing.Point(3, 3);
            this.cboPortNames.Name = "cboPortNames";
            this.cboPortNames.Size = new System.Drawing.Size(170, 21);
            this.cboPortNames.TabIndex = 32;
            this.cboPortNames.SelectionChangeCommitted += new System.EventHandler(this.cboPortNames_SelectionChangeCommitted);
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Controls.Add(this.btnToggleBars);
            this.pnlSpectrum.Controls.Add(this.btnToggleSpectrum);
            this.pnlSpectrum.Controls.Add(this.btnCalibrate);
            this.pnlSpectrum.Controls.Add(this.btnSpectrumMode);
            this.pnlSpectrum.Controls.Add(this.btnAutoRange);
            this.pnlSpectrum.Controls.Add(this.pnlBars);
            this.pnlSpectrum.Location = new System.Drawing.Point(57, 12);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(999, 303);
            this.pnlSpectrum.TabIndex = 19;
            this.pnlSpectrum.SizeChanged += new System.EventHandler(this.pnlSpectrum_SizeChanged);
            // 
            // btnToggleBars
            // 
            this.btnToggleBars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleBars.Location = new System.Drawing.Point(885, 98);
            this.btnToggleBars.Name = "btnToggleBars";
            this.btnToggleBars.Size = new System.Drawing.Size(111, 23);
            this.btnToggleBars.TabIndex = 37;
            this.btnToggleBars.Text = "Toggle Bars";
            this.btnToggleBars.UseVisualStyleBackColor = true;
            this.btnToggleBars.Click += new System.EventHandler(this.btnToggleBars_Click);
            // 
            // btnToggleSpectrum
            // 
            this.btnToggleSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleSpectrum.Location = new System.Drawing.Point(617, 3);
            this.btnToggleSpectrum.Name = "btnToggleSpectrum";
            this.btnToggleSpectrum.Size = new System.Drawing.Size(130, 23);
            this.btnToggleSpectrum.TabIndex = 36;
            this.btnToggleSpectrum.Text = "Toggle Spectrum";
            this.btnToggleSpectrum.UseVisualStyleBackColor = true;
            this.btnToggleSpectrum.Click += new System.EventHandler(this.btnToggleSpectrum_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalibrate.Location = new System.Drawing.Point(753, 3);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(130, 23);
            this.btnCalibrate.TabIndex = 35;
            this.btnCalibrate.Text = "Calibrate Threshold";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // btnSpectrumMode
            // 
            this.btnSpectrumMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSpectrumMode.Location = new System.Drawing.Point(617, 32);
            this.btnSpectrumMode.Name = "btnSpectrumMode";
            this.btnSpectrumMode.Size = new System.Drawing.Size(130, 23);
            this.btnSpectrumMode.TabIndex = 33;
            this.btnSpectrumMode.Text = "Spectrum Mode";
            this.btnSpectrumMode.UseVisualStyleBackColor = true;
            this.btnSpectrumMode.Click += new System.EventHandler(this.btnSpectrumMode_Click);
            // 
            // btnAutoRange
            // 
            this.btnAutoRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoRange.Location = new System.Drawing.Point(753, 32);
            this.btnAutoRange.Name = "btnAutoRange";
            this.btnAutoRange.Size = new System.Drawing.Size(130, 23);
            this.btnAutoRange.TabIndex = 32;
            this.btnAutoRange.Text = "Auto Range";
            this.btnAutoRange.UseVisualStyleBackColor = true;
            this.btnAutoRange.Click += new System.EventHandler(this.btnAutoRange_Click);
            // 
            // pnlBars
            // 
            this.pnlBars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            // btnToggleChart
            // 
            this.btnToggleChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleChart.Location = new System.Drawing.Point(923, 449);
            this.btnToggleChart.Name = "btnToggleChart";
            this.btnToggleChart.Size = new System.Drawing.Size(130, 23);
            this.btnToggleChart.TabIndex = 37;
            this.btnToggleChart.Text = "Toggle Chart";
            this.btnToggleChart.UseVisualStyleBackColor = true;
            this.btnToggleChart.Click += new System.EventHandler(this.btnToggleChart_Click);
            // 
            // btnDisableAllGraphics
            // 
            this.btnDisableAllGraphics.Location = new System.Drawing.Point(3, 242);
            this.btnDisableAllGraphics.Name = "btnDisableAllGraphics";
            this.btnDisableAllGraphics.Size = new System.Drawing.Size(170, 39);
            this.btnDisableAllGraphics.TabIndex = 36;
            this.btnDisableAllGraphics.Text = "Disable All Graphics";
            this.btnDisableAllGraphics.UseVisualStyleBackColor = true;
            this.btnDisableAllGraphics.Click += new System.EventHandler(this.btnDisableAllGraphics_Click);
            // 
            // frmAudioAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1241, 635);
            this.Controls.Add(this.btnToggleChart);
            this.Controls.Add(this.pnlSpectrum);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.trkbrMax);
            this.Controls.Add(this.pnlRangeButtons);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.trkbrThreshold);
            this.Controls.Add(this.trkbrMin);
            this.Name = "frmAudioAnalysis";
            this.Text = "Audio Analysis";
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
            this.tabPostProcessing.ResumeLayout(false);
            this.tabPostProcessing.PerformLayout();
            this.tabAutoSettings.ResumeLayout(false);
            this.tabAutoSettings.PerformLayout();
            this.tabArduino.ResumeLayout(false);
            this.tabArduino.PerformLayout();
            this.pnlSpectrum.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtTimer1Interval;
        private System.Windows.Forms.Label lblRefreshRate;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.ComboBox cboSongNames;
        private System.Windows.Forms.Button btnSaveSong;
        private System.Windows.Forms.Button btnWriteArduino;
        private System.Windows.Forms.ComboBox cboArduinoCommands;
        private System.Windows.Forms.Button btnCalibrate;
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
        private AudioAnalysis.Spectrum pnlSpectrum;
        private System.Windows.Forms.TabPage tabAutoSettings;
        private System.Windows.Forms.Button btnCommitAutoSettings;
        private System.Windows.Forms.Label lblBandwidths;
        private System.Windows.Forms.TextBox txtSeconds;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.TextBox txtThreshMultiplier1;
        private System.Windows.Forms.Label lblThreshMultipliers;
        private System.Windows.Forms.Button btnIncrementRangeBand;
        private System.Windows.Forms.Button btnDecrementRangeBand;
        private System.Windows.Forms.Button btnToggleSpectrum;
        private System.Windows.Forms.ComboBox cboPortNames;
        private System.Windows.Forms.TextBox txtMode;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblArduinoMRange;
        private System.Windows.Forms.Button btnArduinoMRange3;
        private System.Windows.Forms.Button btnArduinoMRange2;
        private System.Windows.Forms.Button btnToggleChart;
        private System.Windows.Forms.TabPage tabPostProcessing;
        private System.Windows.Forms.Label lblGetsSubtractedBy;
        private System.Windows.Forms.ComboBox cboSubtractor;
        private System.Windows.Forms.ComboBox cboSubtractFrom;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btnToggleBars;
        private System.Windows.Forms.TextBox txtThreshMultiplier3;
        private System.Windows.Forms.TextBox txtThreshMultiplier2;
        private System.Windows.Forms.Button btnDynamicThresholds;
        private System.Windows.Forms.TextBox txtBandwidth3;
        private System.Windows.Forms.TextBox txtBandwidth2;
        private System.Windows.Forms.TextBox txtBandwidth1;
        private System.Windows.Forms.Button btnDisableAllGraphics;
    }
}

