using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis : Form
    {
        private void btnArduino_Click(object sender, EventArgs e)
        {
            ArduinoCode.InterpretCommand(cboArduinoCommands.Text);
        }
    }

    public static class ArduinoCode
    {
        public static SerialPort port = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);

        public static bool enabled = false;

        public static string[] arduinoCommands = { "ON: 1", "OFF: 0", "Density 1: s", "Density 2: t", "Density 3: u", "Density 4: v" };

        public static int selectedRange = 0;

        public static void InterpretCommand(string arduinoCommand)
        {
            string commandChar = arduinoCommand[arduinoCommand.Length - 1].ToString();

            if (commandChar == "0")
            {
                port.Close();
                return;
            }
            else if(commandChar == "1")
            {
                port.Open();
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
                if (rangeIndex == selectedRange) Write("b");
                //else if (rangeIndex == 2) Write("m");
            }
        }

    }
}
