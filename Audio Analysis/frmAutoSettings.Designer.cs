namespace AudioAnalyzer
{
    partial class frmAutoSettings
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
            this.lblPrediction = new System.Windows.Forms.Label();
            this.txtPrediction = new System.Windows.Forms.TextBox();
            this.btnTrain = new System.Windows.Forms.Button();
            this.tabctrlBandAnalysis = new System.Windows.Forms.TabControl();
            this.btnDynamicThresholds = new System.Windows.Forms.Button();
            this.gvAutoSettings = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gvAutoSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrediction
            // 
            this.lblPrediction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrediction.AutoSize = true;
            this.lblPrediction.Location = new System.Drawing.Point(622, 549);
            this.lblPrediction.Name = "lblPrediction";
            this.lblPrediction.Size = new System.Drawing.Size(57, 13);
            this.lblPrediction.TabIndex = 74;
            this.lblPrediction.Text = "Prediction:";
            // 
            // txtPrediction
            // 
            this.txtPrediction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrediction.BackColor = System.Drawing.Color.Black;
            this.txtPrediction.ForeColor = System.Drawing.Color.White;
            this.txtPrediction.Location = new System.Drawing.Point(735, 546);
            this.txtPrediction.Name = "txtPrediction";
            this.txtPrediction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrediction.Size = new System.Drawing.Size(49, 20);
            this.txtPrediction.TabIndex = 73;
            this.txtPrediction.Text = "0";
            // 
            // btnTrain
            // 
            this.btnTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrain.Location = new System.Drawing.Point(621, 572);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(173, 32);
            this.btnTrain.TabIndex = 72;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            // 
            // tabctrlBandAnalysis
            // 
            this.tabctrlBandAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabctrlBandAnalysis.Location = new System.Drawing.Point(621, 50);
            this.tabctrlBandAnalysis.Name = "tabctrlBandAnalysis";
            this.tabctrlBandAnalysis.SelectedIndex = 0;
            this.tabctrlBandAnalysis.Size = new System.Drawing.Size(173, 490);
            this.tabctrlBandAnalysis.TabIndex = 71;
            // 
            // btnDynamicThresholds
            // 
            this.btnDynamicThresholds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDynamicThresholds.Location = new System.Drawing.Point(621, 12);
            this.btnDynamicThresholds.Name = "btnDynamicThresholds";
            this.btnDynamicThresholds.Size = new System.Drawing.Size(173, 32);
            this.btnDynamicThresholds.TabIndex = 68;
            this.btnDynamicThresholds.Text = "Dynamic Thresholds";
            this.btnDynamicThresholds.UseVisualStyleBackColor = true;
            // 
            // gvAutoSettings
            // 
            this.gvAutoSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvAutoSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAutoSettings.Location = new System.Drawing.Point(12, 12);
            this.gvAutoSettings.Name = "gvAutoSettings";
            this.gvAutoSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gvAutoSettings.Size = new System.Drawing.Size(603, 592);
            this.gvAutoSettings.TabIndex = 75;
            // 
            // frmAutoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 616);
            this.Controls.Add(this.gvAutoSettings);
            this.Controls.Add(this.lblPrediction);
            this.Controls.Add(this.txtPrediction);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.tabctrlBandAnalysis);
            this.Controls.Add(this.btnDynamicThresholds);
            this.Name = "frmAutoSettings";
            this.Text = "Auto Settings";
            ((System.ComponentModel.ISupportInitialize)(this.gvAutoSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrediction;
        private System.Windows.Forms.TextBox txtPrediction;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.TabControl tabctrlBandAnalysis;
        private System.Windows.Forms.Button btnDynamicThresholds;
        private System.Windows.Forms.DataGridView gvAutoSettings;
    }
}