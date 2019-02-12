﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    public static class Visuals
    {
        public const float PI = (float)Math.PI;

        public static IVFX Preset = new CubeMatrix();

        public static void Run()
        {
            using (VisEnv vis = new VisEnv(500, 500, "Visuals"))
            {
                vis.Run(60.0);
            }

        }
    }
}
