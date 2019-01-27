namespace AudioAnalyzer
{
    partial class GateForm
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
            this.pnlGate = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlGate
            // 
            this.pnlGate.BackColor = System.Drawing.SystemColors.Control;
            this.pnlGate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGate.Location = new System.Drawing.Point(0, 0);
            this.pnlGate.Name = "pnlGate";
            this.pnlGate.Size = new System.Drawing.Size(184, 212);
            this.pnlGate.TabIndex = 20;
            this.pnlGate.SizeChanged += new System.EventHandler(this.pnlGate_SizeChanged);
            // 
            // GateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 212);
            this.Controls.Add(this.pnlGate);
            this.MaximumSize = new System.Drawing.Size(200, 250);
            this.Name = "GateForm";
            this.Text = "Gate";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlGate;
    }
}