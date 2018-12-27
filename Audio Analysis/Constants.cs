using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AudioAnalysis
{
    class Constants
    {
        public class Brushes
        {
            public static SolidBrush blackBrush;
            public static SolidBrush redBrush;
            public static SolidBrush blueBrush;
        }

        public static void Init()
        {
            Brushes.blueBrush = new SolidBrush(Color.Blue);
            Brushes.blackBrush = new SolidBrush(Color.Black);
            Brushes.redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
        }
    }
}
