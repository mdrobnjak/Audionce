using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class frmAudioAnalyzer : Form
    {
        private void InitArduinoSettings()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                cboPortNames.Items.Add(portName);
                cboPortNames.Text = portName;
            }

            ArduinoCode.port = new SerialPort(cboPortNames.Text != "" ? cboPortNames.Text : "No Serial Devices Available", 57600, Parity.None, 8, StopBits.One);

            btnArduinoMRange2_Click(null,null);
        }

        private void btnWriteArduino_Click(object sender, EventArgs e)
        {
            ArduinoCode.InterpretCommand(cboArduinoCommands.Text);
        }

        private void cboPortNames_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ArduinoCode.port = new SerialPort(cboPortNames.Text, 57600, Parity.None, 8, StopBits.One);
        }

        private void btnArduinoMRange2_Click(object sender, EventArgs e)
        {
            ArduinoCode.mRange = 1;
            btnArduinoMRange2.BackColor = Color.LightGreen;
            btnArduinoMRange3.BackColor = Color.Transparent;
        }

        private void btnArduinoMRange3_Click(object sender, EventArgs e)
        {
            ArduinoCode.mRange = 2;
            btnArduinoMRange2.BackColor = Color.Transparent;
            btnArduinoMRange3.BackColor = Color.LightGreen;
        }

        private void btnDisableAllGraphics_Click(object sender, EventArgs e)
        {
            if(drawBars)
            {
                drawBars = false;
                Spectrum.drawSpectrum = false;
                drawChart = false;
                btnDisableAllGraphics.BackColor = Color.LightGreen;
            }
            else
            {
                drawBars = true;
                Spectrum.drawSpectrum = true;
                drawChart = true;
                btnDisableAllGraphics.BackColor = Color.Transparent;
            }
            gSpectrum.Clear(Color.White);
            chart1.Series[0].Points.Clear();
        }
    }

    public static class ArduinoCode
    {
        public static SerialPort port;

        public static bool enabled = false;

        public static string[] arduinoCommands = { "ON: 1", "OFF: 0"};

        public static int bRange = 0, mRange;
        
        public static void InterpretCommand(string arduinoCommand)
        {
            string commandChar = arduinoCommand[arduinoCommand.Length - 1].ToString();

            if (commandChar == "0")
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
                return;
            }
            else if(commandChar == "1")
            {
                if (!port.IsOpen)
                {
                    port.Open();
                }
                return;
            }

            Write(commandChar);
        }

        public static void Write(string msg)
        {
            port.Write(msg);
        }

        public static void Toggle()
        {
            if (port.IsOpen)
            {
                port.Close();
            }
            else
            {
                port.Open();
            }
        }

        public static void Trigger(int rangeIndex)
        {
            if (port.IsOpen)
            {
                if (rangeIndex == bRange) Write("b");
                else if (rangeIndex == mRange) Write("m");
            }
        }

    }
}
