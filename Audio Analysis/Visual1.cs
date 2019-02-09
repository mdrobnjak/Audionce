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
            double translate = (cubesPerSide * offset / 2) - (offset / 2);

            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.Translate(-40.0, -40.0, 40.0);
            GL.Rotate(VisEnv.Angle, 1.0, 0.0, 0.0);
            GL.Rotate(VisEnv.Angle, 0.0, 0.0, 1.0);
            GL.Translate(-translate, -translate, translate);
            //GL.Translate(40.0, 40.0, 40.0);

            GL.Begin(PrimitiveType.Quads);

            CubeMatrix();

            //Cube(VisEnv.Dimension, 10, 10, -10);
            //Cube(VisEnv.Dimension, -10, 10, -10);
            //Cube(VisEnv.Dimension, -10, -10, -10);
            //Cube(VisEnv.Dimension, 10, -10, -10);

            //Cube(VisEnv.Dimension, 10, 10, 10);
            //Cube(VisEnv.Dimension, -10, 10, 10);
            //Cube(VisEnv.Dimension, -10, -10, 10);
            //Cube(VisEnv.Dimension, 10, -10, 10);

            GL.End();
            window.SwapBuffers();

            VisEnv.DecrementDimension();
            if(!VisEnv.Locked)VisEnv.IncrementAngle();
        }

        int cubesPerSide = 8;
        int offset = 10;


        void CubeMatrix()
        {
            int x = 0;
            int y = 0;
            int z = 0;

            for (int i = 0; i < cubesPerSide; i++)
            {
                x = i * offset;
                for (int j = 0; j < cubesPerSide; j++)
                {
                    y = j * offset;
                    for(int k = 0; k < cubesPerSide; k++)
                    {
                        z = k * -offset;
                        Cube(VisEnv.Dimension, x, y, z);
                    }
                } 
            }
        }

        void Cube(double sideLength, double xOffset = 0, double yOffset = 0, double zOffset = 0)
        {
            double val = sideLength / 2;

            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3(-val+xOffset, val+yOffset, val + zOffset);
            GL.Vertex3(-val+xOffset, val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, val + zOffset);

            GL.Color3(1.0, 0.0, 1.0);
            GL.Vertex3(val+xOffset, val+yOffset, val + zOffset);
            GL.Vertex3(val+xOffset, val+yOffset, -val + zOffset);
            GL.Vertex3(val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(val+xOffset, -val+yOffset, val + zOffset);

            GL.Color3(0.0, 1.0, 1.0);
            GL.Vertex3(val+xOffset, -val+yOffset, val + zOffset);
            GL.Vertex3(val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, val + zOffset);

            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex3(val+xOffset, val+yOffset, val + zOffset);
            GL.Vertex3(val+xOffset, val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, val+yOffset, val + zOffset);

            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex3(val+xOffset, val+yOffset, -val + zOffset);
            GL.Vertex3(val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, -val + zOffset);
            GL.Vertex3(-val+xOffset, val+yOffset, -val + zOffset);

            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex3(val+xOffset, val+yOffset, val + zOffset);
            GL.Vertex3(val+xOffset, -val+yOffset, val + zOffset);
            GL.Vertex3(-val+xOffset, -val+yOffset, val + zOffset);
            GL.Vertex3(-val+xOffset, val+yOffset, val + zOffset);

        }

    }
}

