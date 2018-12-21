namespace Mark_s_Visualizer
{
    partial class frmAudionceUnity
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
            this.pnlSpectrum = new System.Windows.Forms.Panel();
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
            this.trckbrThreshold = new System.Windows.Forms.TrackBar();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.pnlRangeButtons = new System.Windows.Forms.Panel();
            this.btnRange3 = new System.Windows.Forms.Button();
            this.btnRange2 = new System.Windows.Forms.Button();
            this.btnRange1 = new System.Windows.Forms.Button();
            this.trckbrMin = new System.Windows.Forms.TrackBar();
            this.trckbrMax = new System.Windows.Forms.TrackBar();
            this.txtMasterScaleFFT = new System.Windows.Forms.TextBox();
            this.txtTimer1Interval = new System.Windows.Forms.TextBox();
            this.lblMasterScaleFFT = new System.Windows.Forms.Label();
            this.lblRefreshRate = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.cboSongNames = new System.Windows.Forms.ComboBox();
            this.btnSaveSong = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.pnlSpectrum.SuspendLayout();
            this.pnlNewAudio.SuspendLayout();
            this.pnlAccumAudio.SuspendLayout();
            this.pnlBars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckbrThreshold)).BeginInit();
            this.pnlRangeButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckbrMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckbrMax)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(57, 439);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Black;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(999, 165);
            this.chart1.TabIndex = 16;
            this.chart1.Text = "chart1";
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Controls.Add(this.pnlNewAudio);
            this.pnlSpectrum.Controls.Add(this.pnlAccumAudio);
            this.pnlSpectrum.Controls.Add(this.pnlBars);
            this.pnlSpectrum.Location = new System.Drawing.Point(57, 12);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(999, 303);
            this.pnlSpectrum.TabIndex = 19;
            this.pnlSpectrum.SizeChanged += new System.EventHandler(this.pnlSpectrum_SizeChanged);
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
            // trckbrThreshold
            // 
            this.trckbrThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.trckbrThreshold.Location = new System.Drawing.Point(6, 12);
            this.trckbrThreshold.Maximum = 10000;
            this.trckbrThreshold.Name = "trckbrThreshold";
            this.trckbrThreshold.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trckbrThreshold.Size = new System.Drawing.Size(45, 361);
            this.trckbrThreshold.TabIndex = 17;
            this.trckbrThreshold.TickFrequency = 10;
            this.trckbrThreshold.ValueChanged += new System.EventHandler(this.trckbrThreshold_ValueChanged);
            // 
            // txtThreshold
            // 
            this.txtThreshold.BackColor = System.Drawing.Color.Black;
            this.txtThreshold.ForeColor = System.Drawing.Color.White;
            this.txtThreshold.Location = new System.Drawing.Point(6, 379);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshold.Size = new System.Drawing.Size(42, 20);
            this.txtThreshold.TabIndex = 4;
            this.txtThreshold.Text = "0";
            this.txtThreshold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtThreshold_KeyDown);
            // 
            // pnlRangeButtons
            // 
            this.pnlRangeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRangeButtons.Controls.Add(this.btnRange3);
            this.pnlRangeButtons.Controls.Add(this.btnRange2);
            this.pnlRangeButtons.Controls.Add(this.btnRange1);
            this.pnlRangeButtons.Location = new System.Drawing.Point(57, 380);
            this.pnlRangeButtons.Name = "pnlRangeButtons";
            this.pnlRangeButtons.Size = new System.Drawing.Size(235, 54);
            this.pnlRangeButtons.TabIndex = 20;
            // 
            // btnRange3
            // 
            this.btnRange3.BackColor = System.Drawing.SystemColors.Control;
            this.btnRange3.Location = new System.Drawing.Point(159, 3);
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
            this.btnRange2.Location = new System.Drawing.Point(81, 3);
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
            this.btnRange1.Location = new System.Drawing.Point(3, 3);
            this.btnRange1.Name = "btnRange1";
            this.btnRange1.Size = new System.Drawing.Size(72, 48);
            this.btnRange1.TabIndex = 0;
            this.btnRange1.Text = "Range 1";
            this.btnRange1.UseVisualStyleBackColor = false;
            this.btnRange1.Click += new System.EventHandler(this.btnRange1_Click);
            // 
            // trckbrMin
            // 
            this.trckbrMin.BackColor = System.Drawing.SystemColors.Control;
            this.trckbrMin.Location = new System.Drawing.Point(45, 310);
            this.trckbrMin.Name = "trckbrMin";
            this.trckbrMin.Size = new System.Drawing.Size(1020, 45);
            this.trckbrMin.TabIndex = 21;
            this.trckbrMin.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trckbrMin.ValueChanged += new System.EventHandler(this.trckbrMin_ValueChanged);
            // 
            // trckbrMax
            // 
            this.trckbrMax.BackColor = System.Drawing.SystemColors.Control;
            this.trckbrMax.Location = new System.Drawing.Point(45, 339);
            this.trckbrMax.Name = "trckbrMax";
            this.trckbrMax.Size = new System.Drawing.Size(1020, 45);
            this.trckbrMax.TabIndex = 22;
            this.trckbrMax.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trckbrMax.ValueChanged += new System.EventHandler(this.trckbrMax_ValueChanged);
            // 
            // txtMasterScaleFFT
            // 
            this.txtMasterScaleFFT.BackColor = System.Drawing.Color.Black;
            this.txtMasterScaleFFT.ForeColor = System.Drawing.Color.White;
            this.txtMasterScaleFFT.Location = new System.Drawing.Point(298, 380);
            this.txtMasterScaleFFT.Name = "txtMasterScaleFFT";
            this.txtMasterScaleFFT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMasterScaleFFT.Size = new System.Drawing.Size(42, 20);
            this.txtMasterScaleFFT.TabIndex = 23;
            this.txtMasterScaleFFT.Text = "0";
            this.txtMasterScaleFFT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMasterScaleFFT_KeyDown);
            // 
            // txtTimer1Interval
            // 
            this.txtTimer1Interval.BackColor = System.Drawing.Color.Black;
            this.txtTimer1Interval.ForeColor = System.Drawing.Color.White;
            this.txtTimer1Interval.Location = new System.Drawing.Point(298, 414);
            this.txtTimer1Interval.Name = "txtTimer1Interval";
            this.txtTimer1Interval.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTimer1Interval.Size = new System.Drawing.Size(42, 20);
            this.txtTimer1Interval.TabIndex = 24;
            this.txtTimer1Interval.Text = "0";
            this.txtTimer1Interval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimer1Interval_KeyDown);
            // 
            // lblMasterScaleFFT
            // 
            this.lblMasterScaleFFT.AutoSize = true;
            this.lblMasterScaleFFT.Location = new System.Drawing.Point(346, 383);
            this.lblMasterScaleFFT.Name = "lblMasterScaleFFT";
            this.lblMasterScaleFFT.Size = new System.Drawing.Size(66, 13);
            this.lblMasterScaleFFT.TabIndex = 25;
            this.lblMasterScaleFFT.Text = "FFT Y Scale";
            // 
            // lblRefreshRate
            // 
            this.lblRefreshRate.AutoSize = true;
            this.lblRefreshRate.Location = new System.Drawing.Point(346, 418);
            this.lblRefreshRate.Name = "lblRefreshRate";
            this.lblRefreshRate.Size = new System.Drawing.Size(92, 13);
            this.lblRefreshRate.TabIndex = 26;
            this.lblRefreshRate.Text = "Refresh Rate (ms)";
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(3, 402);
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
            this.cboSongNames.Location = new System.Drawing.Point(530, 378);
            this.cboSongNames.Name = "cboSongNames";
            this.cboSongNames.Size = new System.Drawing.Size(294, 21);
            this.cboSongNames.TabIndex = 28;
            this.cboSongNames.SelectionChangeCommitted += new System.EventHandler(this.cboSongNames_SelectionChangeCommitted);
            // 
            // btnSaveSong
            // 
            this.btnSaveSong.BackColor = System.Drawing.Color.Black;
            this.btnSaveSong.ForeColor = System.Drawing.Color.White;
            this.btnSaveSong.Location = new System.Drawing.Point(830, 376);
            this.btnSaveSong.Name = "btnSaveSong";
            this.btnSaveSong.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSong.TabIndex = 29;
            this.btnSaveSong.Text = "Save";
            this.btnSaveSong.UseVisualStyleBackColor = false;
            this.btnSaveSong.Click += new System.EventHandler(this.btnSaveSong_Click);
            // 
            // frmAudionceUnity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1068, 614);
            this.Controls.Add(this.btnSaveSong);
            this.Controls.Add(this.cboSongNames);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.lblRefreshRate);
            this.Controls.Add(this.lblMasterScaleFFT);
            this.Controls.Add(this.txtTimer1Interval);
            this.Controls.Add(this.txtMasterScaleFFT);
            this.Controls.Add(this.trckbrMax);
            this.Controls.Add(this.pnlRangeButtons);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.trckbrThreshold);
            this.Controls.Add(this.pnlSpectrum);
            this.Controls.Add(this.trckbrMin);
            this.Name = "frmAudionceUnity";
            this.Text = "Mark\'s Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAudionceUnity_FormClosing);
            this.Load += new System.EventHandler(this.frmAudionceUnity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.pnlSpectrum.ResumeLayout(false);
            this.pnlNewAudio.ResumeLayout(false);
            this.pnlNewAudio.PerformLayout();
            this.pnlAccumAudio.ResumeLayout(false);
            this.pnlAccumAudio.PerformLayout();
            this.pnlBars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trckbrThreshold)).EndInit();
            this.pnlRangeButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trckbrMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckbrMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel pnlSpectrum;
        private System.Windows.Forms.TrackBar trckbrThreshold;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Panel pnlRangeButtons;
        private System.Windows.Forms.Button btnRange3;
        private System.Windows.Forms.Button btnRange2;
        private System.Windows.Forms.Button btnRange1;
        private System.Windows.Forms.TrackBar trckbrMin;
        private System.Windows.Forms.TrackBar trckbrMax;
        private System.Windows.Forms.Panel pnlBars;
        private System.Windows.Forms.ProgressBar barRange3;
        private System.Windows.Forms.ProgressBar barRange2;
        private System.Windows.Forms.ProgressBar barRange1;
        private System.Windows.Forms.TextBox txtMasterScaleFFT;
        private System.Windows.Forms.TextBox txtAccumAudio3;
        private System.Windows.Forms.TextBox txtAccumAudio2;
        private System.Windows.Forms.TextBox txtAccumAudio1;
        private System.Windows.Forms.Panel pnlAccumAudio;
        private System.Windows.Forms.TextBox txtTimer1Interval;
        private System.Windows.Forms.Panel pnlNewAudio;
        private System.Windows.Forms.TextBox txtNewAudio3;
        private System.Windows.Forms.TextBox txtNewAudio2;
        private System.Windows.Forms.TextBox txtNewAudio1;
        private System.Windows.Forms.Label lblMasterScaleFFT;
        private System.Windows.Forms.Label lblRefreshRate;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.ComboBox cboSongNames;
        private System.Windows.Forms.Button btnSaveSong;
    }
}

