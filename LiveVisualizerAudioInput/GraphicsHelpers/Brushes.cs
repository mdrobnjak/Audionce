using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LiveVisualizerAudioInput
{
    public class Brushes
    {
        public static SolidBrush[] rangeBrushes = new SolidBrush[Range.Count];
        public static SolidBrush[] rangeBrushesDark = new SolidBrush[Range.Count];        

        public static SolidBrush[] gateBrushes = new SolidBrush[Range.Count];
        public static SolidBrush blackBrush;
        public static SolidBrush redBrush;
        public static SolidBrush blueBrush;
        public static SolidBrush whiteBrush;

        public static void Init()
        {
            blackBrush = new SolidBrush(Color.Black);
            redBrush = new SolidBrush(Color.FromArgb(255, Color.Red));
            blueBrush = new SolidBrush(Color.FromArgb(255, Color.Blue));
            whiteBrush = new SolidBrush(Color.White);
            for (int i = 0; i < Range.Count; i++)
            {
                rangeBrushes[i] = new SolidBrush(Range.Ranges[i].Color);
            }
            for (int i = 0; i < Range.Count; i++)
            {
                rangeBrushesDark[i] = new SolidBrush(Range.Ranges[i].DarkColor);
            }
        }

        public static void InitGateBrushes(int activeIndex)
        {
            for (int i = 0; i < Range.Count; i++)
            {
                if (i == activeIndex) gateBrushes[i] = redBrush;
                else gateBrushes[i] = blackBrush;
            }
        }
    }
}
