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
            this.msSpectrum = new System.Windows.Forms.MenuStrip();
            this.msSpectrumSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.msActiveRangeOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.msScale = new System.Windows.Forms.ToolStripMenuItem();
            this.txtmsiScale = new System.Windows.Forms.ToolStripTextBox();
            this.msSpectrum.SuspendLayout();
            this.SuspendLayout();
            // 
            // msSpectrum
            // 
            this.msSpectrum.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msSpectrum.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msSpectrumSettings});
            this.msSpectrum.Location = new System.Drawing.Point(0, 0);
            this.msSpectrum.Name = "msSpectrum";
            this.msSpectrum.Size = new System.Drawing.Size(821, 24);
            this.msSpectrum.TabIndex = 20;
            this.msSpectrum.Text = "menuStrip1";
            this.msSpectrum.Visible = false;
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
            this.Controls.Add(this.msSpectrum);
            this.Name = "SpectrumForm";
            this.Text = "Spectrum";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SpectrumForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SpectrumForm_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SpectrumForm_MouseUp);
            this.msSpectrum.ResumeLayout(false);
            this.msSpectrum.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip msSpectrum;
        private System.Windows.Forms.ToolStripMenuItem msSpectrumSettings;
        private System.Windows.Forms.ToolStripMenuItem msActiveRangeOnly;
        private System.Windows.Forms.ToolStripMenuItem msScale;
        private System.Windows.Forms.ToolStripTextBox txtmsiScale;
    }
}