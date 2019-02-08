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
        public static double Height = 1.0;

        public static void Run()
        {
            GameWindow window = new GameWindow(500, 500);

            Visual1 vis1 = new Visual1(window);
        }
    }
}
