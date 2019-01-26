using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class Arduino
    {
        public static List<string> AvailablePorts = new List<string>();

        public static SerialPort Port;

        public static string[] Commands = { "ON: 1", "OFF: 0" };

        public static int bRange = 0, mRange;

        public static void InitPort()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                AvailablePorts.Add(portName);
            }

            Port = new SerialPort(AvailablePorts.Count > 0 ? AvailablePorts.Last() : "No Serial Devices Available", 57600, Parity.None, 8, StopBits.One);
        }


        public static void InterpretCommand(string arduinoCommand)
        {
            string commandChar = arduinoCommand[arduinoCommand.Length - 1].ToString();

            if (commandChar == "0")
            {
                if (Port.IsOpen)
                {
                    Port.Close();
                }
                return;
            }
            else if (commandChar == "1")
            {
                if (!Port.IsOpen)
                {
                    Port.Open();
                }
                return;
            }

            Write(commandChar);
        }

        public static void Write(string msg)
        {
            Port.Write(msg);
        }

        public static void Trigger(int rangeIndex)
        {
            if (Port.IsOpen)
            {
                if (rangeIndex == bRange) Write("b");
                else if (rangeIndex == mRange) Write("m");
            }
        }
    }
}
