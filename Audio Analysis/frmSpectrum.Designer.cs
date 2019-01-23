namespace AudioAnalyzer
{
    partial class frmSpectrum
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
            this.button2 = new System.Windows.Forms.Button();
            this.pnlSpectrum = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.msiActiveRangeOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSpectrum.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(695, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 41);
            this.button2.TabIndex = 33;
            this.button2.Text = "Spectrum Mode";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Controls.Add(this.button2);
            this.pnlSpectrum.Location = new System.Drawing.Point(12, 135);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(776, 303);
            this.pnlSpectrum.TabIndex = 19;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msSettings
            // 
            this.msSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msiActiveRangeOnly});
            this.msSettings.Name = "msSettings";
            this.msSettings.Size = new System.Drawing.Size(61, 20);
            this.msSettings.Text = "Settings";
            // 
            // msiActiveRangeOnly
            // 
            this.msiActiveRangeOnly.CheckOnClick = true;
            this.msiActiveRangeOnly.Name = "msiActiveRangeOnly";
            this.msiActiveRangeOnly.Size = new System.Drawing.Size(180, 22);
            this.msiActiveRangeOnly.Text = "Active Range Only";
            this.msiActiveRangeOnly.CheckStateChanged += new System.EventHandler(this.msiActiveRangeOnly_CheckStateChanged);
            // 
            // frmSpectrum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlSpectrum);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmSpectrum";
            this.Text = "Spectrum";
            this.pnlSpectrum.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlSpectrum;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msSettings;
        private System.Windows.Forms.ToolStripMenuItem msiActiveRangeOnly;
    }
}