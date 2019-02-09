using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    public static class VisEnv
    {
        public const float PI = (float)Math.PI;

        public static double Dimension { get; private set; }

        public static double Angle { get; private set; }

        public static bool Locked;

        public static void SetDimensionToMax()
        {
            Dimension = 2;
        }

        public static void DecrementDimension()
        {
            Dimension -= 0.05;
        }

        public static void IncrementAngle()
        {
            Angle += 1;
        }

        public static void Run()
        {
            GameWindow window = new GameWindow(500, 500);

            SetDimensionToMax();

            Visual1 vis1 = new Visual1(window);
        }
    }
}
