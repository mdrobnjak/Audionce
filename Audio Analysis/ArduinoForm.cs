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
            InterpretCommand(cboArduinoCommands.Text);
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

        public static void InterpretCommand(string arduinoCommand)
        {
            string commandChar = arduinoCommand[arduinoCommand.Length - 1].ToString();

            if (commandChar == "0")
            {
                if (Arduino.Port.IsOpen)
                {
                    Arduino.Port.Close();
                }
                return;
            }
            else if (commandChar == "1")
            {
                if (!Arduino.Port.IsOpen)
                {
                    Arduino.Port.Open();
                }
                return;
            }

            Write(commandChar);
        }

        public static void Write(string msg)
        {
            Arduino.Port.Write(msg);
        }

        public static void Toggle()
        {
            if (Arduino.Port.IsOpen)
            {
                Arduino.Port.Close();
            }
            else
            {
                Arduino.Port.Open();
            }
        }

        public static void Trigger(int rangeIndex)
        {
            if (Arduino.Port.IsOpen)
            {
                if (rangeIndex == Arduino.bRange) Write("b");
                else if (rangeIndex == Arduino.mRange) Write("m");
            }
        }
    }
}
