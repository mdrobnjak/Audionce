namespace AudioAnalyzer
{
    partial class SpectrumForm
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
            this.pnlSpectrum = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msSpectrumSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.msActiveRangeOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.msScale = new System.Windows.Forms.ToolStripMenuItem();
            this.txtmsiScale = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Location = new System.Drawing.Point(0, 0);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(821, 350);
            this.pnlSpectrum.TabIndex = 19;
            this.pnlSpectrum.SizeChanged += new System.EventHandler(this.pnlSpectrum_SizeChanged);
            this.pnlSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSpectrum_MouseDown);
            this.pnlSpectrum.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSpectrum_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msSpectrumSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(821, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msSpectrumSettings
            // 
            this.msSpectrumSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msActiveRangeOnly,
            this.msScale});
            this.msSpectrumSettings.Name = "msSpectrumSettings";
            this.msSpectrumSettings.Size = new System.Drawing.Size(70, 20);
            this.msSpectrumSettings.Text = "Spectrum";
            // 
            // msActiveRangeOnly
            // 
            this.msActiveRangeOnly.CheckOnClick = true;
            this.msActiveRangeOnly.Name = "msActiveRangeOnly";
            this.msActiveRangeOnly.Size = new System.Drawing.Size(171, 22);
            this.msActiveRangeOnly.Text = "Active Range Only";
            this.msActiveRangeOnly.CheckStateChanged += new System.EventHandler(this.msActiveRangeOnly_CheckStateChanged);
            // 
            // msScale
            // 
            this.msScale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtmsiScale});
            this.msScale.Name = "msScale";
            this.msScale.Size = new System.Drawing.Size(171, 22);
            this.msScale.Text = "Scale";
            // 
            // txtmsiScale
            // 
            this.txtmsiScale.Name = "txtmsiScale";
            this.txtmsiScale.Size = new System.Drawing.Size(100, 23);
            this.txtmsiScale.TextChanged += new System.EventHandler(this.txtmsScale_TextChanged);
            // 
            // SpectrumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 350);
            this.Controls.Add(this.pnlSpectrum);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SpectrumForm";
            this.Text = "Spectrum";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlSpectrum;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msSpectrumSettings;
        private System.Windows.Forms.ToolStripMenuItem msActiveRangeOnly;
        private System.Windows.Forms.ToolStripMenuItem msScale;
        private System.Windows.Forms.ToolStripTextBox txtmsiScale;
    }
}