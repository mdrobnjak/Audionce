using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public static class ArduinoCode
    {
        public static SerialPort port = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);

        public static bool enable = false;

        public static void Write(string msg)
        {
            port.Write(msg);
        }

        public static void Toggle()
        {
            if (enable)
            {
                enable = false;
                port.Close();
            }
            else
            {
                port.Open();
                enable = true;
            }
        }

        public static void Trigger(int rangeIndex)
        {
            if (enable)
            {
                if (rangeIndex == 0) ArduinoCode.Write("b");
                else if (rangeIndex == 1) ArduinoCode.Write("m");
            }
        }

    }
}
