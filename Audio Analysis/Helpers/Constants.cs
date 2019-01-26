using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AudioAnalyzer
{
    class Constants
    {
        public class Brushes
        {
            public static SolidBrush blackBrush;
            public static SolidBrush redBrush;
        }

        public static void Init()
        {
            Brushes.blackBrush = new SolidBrush(Color.Black);
            Brushes.redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
        }
    }
}
