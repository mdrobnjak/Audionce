namespace AudioAnalyzer
{
    partial class ArduinoForm
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
            this.lblArduinoMRange = new System.Windows.Forms.Label();
            this.btnArduinoMRange3 = new System.Windows.Forms.Button();
            this.btnArduinoMRange2 = new System.Windows.Forms.Button();
            this.cboPortNames = new System.Windows.Forms.ComboBox();
            this.cboArduinoCommands = new System.Windows.Forms.ComboBox();
            this.btnWriteArduino = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblArduinoMRange
            // 
            this.lblArduinoMRange.AutoSize = true;
            this.lblArduinoMRange.Location = new System.Drawing.Point(16, 165);
            this.lblArduinoMRange.Name = "lblArduinoMRange";
            this.lblArduinoMRange.Size = new System.Drawing.Size(57, 13);
            this.lblArduinoMRange.TabIndex = 41;
            this.lblArduinoMRange.Text = "\'m\' Range:";
            // 
            // btnArduinoMRange3
            // 
            this.btnArduinoMRange3.Location = new System.Drawing.Point(100, 184);
            this.btnArduinoMRange3.Name = "btnArduinoMRange3";
            this.btnArduinoMRange3.Size = new System.Drawing.Size(75, 32);
            this.btnArduinoMRange3.TabIndex = 40;
            this.btnArduinoMRange3.Text = "Range 3";
            this.btnArduinoMRange3.UseVisualStyleBackColor = true;
            this.btnArduinoMRange3.Click += new System.EventHandler(this.btnArduinoMRange3_Click);
            // 
            // btnArduinoMRange2
            // 
            this.btnArduinoMRange2.Location = new System.Drawing.Point(19, 184);
            this.btnArduinoMRange2.Name = "btnArduinoMRange2";
            this.btnArduinoMRange2.Size = new System.Drawing.Size(75, 32);
            this.btnArduinoMRange2.TabIndex = 39;
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
            this.cboPortNames.Location = new System.Drawing.Point(12, 12);
            this.cboPortNames.Name = "cboPortNames";
            this.cboPortNames.Size = new System.Drawing.Size(170, 21);
            this.cboPortNames.TabIndex = 38;
            this.cboPortNames.SelectionChangeCommitted += new System.EventHandler(this.cboPortNames_SelectionChangeCommitted);
            // 
            // cboArduinoCommands
            // 
            this.cboArduinoCommands.BackColor = System.Drawing.Color.Black;
            this.cboArduinoCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArduinoCommands.ForeColor = System.Drawing.Color.White;
            this.cboArduinoCommands.FormattingEnabled = true;
            this.cboArduinoCommands.Location = new System.Drawing.Point(12, 62);
            this.cboArduinoCommands.Name = "cboArduinoCommands";
            this.cboArduinoCommands.Size = new System.Drawing.Size(170, 21);
            this.cboArduinoCommands.TabIndex = 37;
            // 
            // btnWriteArduino
            // 
            this.btnWriteArduino.Location = new System.Drawing.Point(12, 89);
            this.btnWriteArduino.Name = "btnWriteArduino";
            this.btnWriteArduino.Size = new System.Drawing.Size(170, 39);
            this.btnWriteArduino.TabIndex = 36;
            this.btnWriteArduino.Text = "Write";
            this.btnWriteArduino.UseVisualStyleBackColor = true;
            this.btnWriteArduino.Click += new System.EventHandler(this.btnWriteArduino_Click);
            // 
            // frmArduino
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 233);
            this.Controls.Add(this.lblArduinoMRange);
            this.Controls.Add(this.btnArduinoMRange3);
            this.Controls.Add(this.btnArduinoMRange2);
            this.Controls.Add(this.cboPortNames);
            this.Controls.Add(this.cboArduinoCommands);
            this.Controls.Add(this.btnWriteArduino);
            this.Name = "frmArduino";
            this.Text = "Arduino";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblArduinoMRange;
        private System.Windows.Forms.Button btnArduinoMRange3;
        private System.Windows.Forms.Button btnArduinoMRange2;
        private System.Windows.Forms.ComboBox cboPortNames;
        private System.Windows.Forms.ComboBox cboArduinoCommands;
        private System.Windows.Forms.Button btnWriteArduino;
    }
}