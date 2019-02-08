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
            GL.Enable(EnableCap.DepthTest);
        }

        void Resize(object o, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4.CreatePerspectiveFieldOfView((VisEnv.PI / 180) * 45.0f, window.Width / window.Height, 1.0f, 100.0f, out Matrix4 matrix);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void RenderFrame(object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Translate(0.0, 0.0, -45.0);
            GL.Rotate(VisEnv.Angle, 1.0, 0.0, 0.0);
            GL.Rotate(VisEnv.Angle, 0.0, 0.0, 1.0);

            GL.Begin(PrimitiveType.Quads);

            Cube(VisEnv.Dimension);

            GL.End();
            window.SwapBuffers();

            VisEnv.DecrementDimension();
            if(!VisEnv.Locked)VisEnv.IncrementAngle();
        }

        void Cube(double sideLength)
        {
            double val = sideLength / 2;

            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3(-val, val, val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            GL.Color3(1.0, 0.0, 1.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(val, -val, val);

            GL.Color3(0.0, 1.0, 1.0);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, val, val);

            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, val, -val);

            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(-val, -val, val);
            GL.Vertex3(-val, val, val);

        }

    }
}

