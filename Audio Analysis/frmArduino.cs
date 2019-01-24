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
    public partial class frmArduino : Form
    {
        static List<string> AvailablePorts = new List<string>();

        public static SerialPort ArduinoPort;

        public static bool enabled = false;

        public static string[] arduinoCommands = { "ON: 1", "OFF: 0" };

        public static int bRange = 0, mRange;

        public frmArduino()
        {
            InitializeComponent();

            InitControls();
        }

        public static void InitArduinoPort()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                AvailablePorts.Add(portName);
            }

            ArduinoPort = new SerialPort(AvailablePorts.Count > 0 ? AvailablePorts[0] : "No Serial Devices Available", 57600, Parity.None, 8, StopBits.One);
        }

        private void InitControls()
        {
            foreach (string portName in AvailablePorts)
            {
                cboPortNames.Items.Add(portName);
                cboPortNames.Text = portName;
            }

            cboArduinoCommands.Items.AddRange(arduinoCommands);

            btnArduinoMRange2_Click(null, null);
        }

        private void btnWriteArduino_Click(object sender, EventArgs e)
        {
            InterpretCommand(cboArduinoCommands.Text);
        }

        private void cboPortNames_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ArduinoPort = new SerialPort(cboPortNames.Text, 57600, Parity.None, 8, StopBits.One);
        }

        private void btnArduinoMRange2_Click(object sender, EventArgs e)
        {
            mRange = 1;
            btnArduinoMRange2.BackColor = Color.LightGreen;
            btnArduinoMRange3.BackColor = Color.Transparent;
        }

        private void btnArduinoMRange3_Click(object sender, EventArgs e)
        {
            mRange = 2;
            btnArduinoMRange2.BackColor = Color.Transparent;
            btnArduinoMRange3.BackColor = Color.LightGreen;
        }

        public static void InterpretCommand(string arduinoCommand)
        {
            string commandChar = arduinoCommand[arduinoCommand.Length - 1].ToString();

            if (commandChar == "0")
            {
                if (ArduinoPort.IsOpen)
                {
                    ArduinoPort.Close();
                }
                return;
            }
            else if (commandChar == "1")
            {
                if (!ArduinoPort.IsOpen)
                {
                    ArduinoPort.Open();
                }
                return;
            }

            Write(commandChar);
        }

        public static void Write(string msg)
        {
            ArduinoPort.Write(msg);
        }

        public static void Toggle()
        {
            if (ArduinoPort.IsOpen)
            {
                ArduinoPort.Close();
            }
            else
            {
                ArduinoPort.Open();
            }
        }

        public static void Trigger(int rangeIndex)
        {
            if (ArduinoPort.IsOpen)
            {
                if (rangeIndex == bRange) Write("b");
                else if (rangeIndex == mRange) Write("m");
            }
        }
    }
}
