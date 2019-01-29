using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AudioAnalyzer
{
    class Constants
    {
        public static float PI = 3.1415926f;


        public class Brushes
        {
            public static SolidBrush[] rangeBrushes = new SolidBrush[Range.Count];
            public static SolidBrush blackBrush;
            public static SolidBrush redBrush;
            public static SolidBrush rangeColorBrush;
        }

        public static void Init()
        {
            Brushes.blackBrush = new SolidBrush(Color.Black);
            Brushes.redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
            Brushes.rangeColorBrush = new SolidBrush(Color.Black);
        }

        public static void InitRangeBrushes(int activeIndex)
        {
            for (int i = 0; i < Range.Count; i++)
            {
                if (i == activeIndex) Brushes.rangeBrushes[i] = new SolidBrush(Range.Active.Color);
                else Brushes.rangeBrushes[i] = Brushes.blackBrush;
            }
        }
    }
}
