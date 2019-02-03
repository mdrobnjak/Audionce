using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AudioAnalyzer
{
    public class Brushes
    {
        public static SolidBrush[] rangeBrushes = new SolidBrush[Range.Count];
        public static SolidBrush[] gateBrushes = new SolidBrush[Range.Count];
        public static SolidBrush blackBrush;
        public static SolidBrush redBrush;

        public static void Init()
        {
            Brushes.blackBrush = new SolidBrush(Color.Black);
            Brushes.redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
            for (int i = 0; i < Range.Count; i++)
            {
                Brushes.rangeBrushes[i] = new SolidBrush(Range.Ranges[i].Color);
            }
        }

        public static void InitGateBrushes(int activeIndex)
        {
            for (int i = 0; i < Range.Count; i++)
            {
                if (i == activeIndex) Brushes.gateBrushes[i] = new SolidBrush(Range.Active.Color);
                else Brushes.gateBrushes[i] = Brushes.blackBrush;
            }
        }
    }
}
