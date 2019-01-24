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

            Port = new SerialPort(AvailablePorts.Count > 0 ? AvailablePorts[0] : "No Serial Devices Available", 57600, Parity.None, 8, StopBits.One);
        }
    }
}
