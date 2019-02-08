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
    public class Visual1
    {
        GameWindow window;

        public Visual1(GameWindow window)
        {
            this.window = window;

            Start();
        }

        public void Start()
        {
            window.Load += Loaded;
            window.Resize += Resize;
            window.RenderFrame += RenderFrame;
            window.Run(1.0 / 60);
        }

        void Loaded(object o, EventArgs e)
        {
            GL.ClearColor(Color.Black);
        }

        void Resize(object o, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-50.0, 50.0, -50.0, 50.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void RenderFrame(object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(PrimitiveType.Quads);

            //for()
            //{

            //}

            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex2(30.0, -30.0 + VisEnv.Height);
            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex2(-30.0, -30.0 + VisEnv.Height);
            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex2(-30.0, -30.0);
            GL.Color3(1.0, 1.0, 1.0);
            GL.Vertex2(30.0, -30.0);

            GL.End();
            window.SwapBuffers();

            VisEnv.Height -= 0.5;
        }

    }
}

