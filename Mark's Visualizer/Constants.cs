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
            public static SolidBrush redBrush;
            public static SolidBrush redLightBrush;
            public static SolidBrush blueBrush;
        }

        public static void Init()
        {
            Brushes.blueBrush = new SolidBrush(Color.Blue);
            Brushes.redBrush = new SolidBrush(Color.Black);
            Brushes.redLightBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
        }
    }
}
