using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class ArduinoForm : Form
    {      

        public ArduinoForm()
        {
            InitializeComponent();

            InitControls();
        }


        private void InitControls()
        {
            foreach (string portName in Arduino.AvailablePorts)
            {
                cboPortNames.Items.Add(portName);
                cboPortNames.Text = portName;
            }

            cboArduinoCommands.Items.AddRange(Arduino.Commands);

            btnArduinoMRange2_Click(null, null);
        }

        private void btnWriteArduino_Click(object sender, EventArgs e)
        {
            Arduino.InterpretCommand(cboArduinoCommands.Text);
        }

        private void cboPortNames_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Arduino.Port = new SerialPort(cboPortNames.Text, 57600, Parity.None, 8, StopBits.One);
        }

        private void btnArduinoMRange2_Click(object sender, EventArgs e)
        {
            Arduino.mRange = 1;
            btnArduinoMRange2.BackColor = Color.LightGreen;
            btnArduinoMRange3.BackColor = Color.Transparent;
        }

        private void btnArduinoMRange3_Click(object sender, EventArgs e)
        {
            Arduino.mRange = 2;
            btnArduinoMRange2.BackColor = Color.Transparent;
            btnArduinoMRange3.BackColor = Color.LightGreen;
        }
    }
}
