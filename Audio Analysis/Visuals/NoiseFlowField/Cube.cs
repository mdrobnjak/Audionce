using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class Cube
    {
        double val;
        public double xOffset, yOffset, zOffset;
        double xAngle, yAngle, zAngle;


        public Cube(double sideLength, double xOffset, double yOffset, double zOffset)
        {
            val = sideLength / 2;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.zOffset = zOffset;
        }

        public void SetLength(double sideLength)
        {
            val = sideLength / 2;
        }

        public void SpecifyVertices()
        {
            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3(-val + xOffset, val + yOffset, val + zOffset);
            GL.Vertex3(-val + xOffset, val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, val + zOffset);

            GL.Color3(1.0, 0.0, 1.0);
            GL.Vertex3(val + xOffset, val + yOffset, val + zOffset);
            GL.Vertex3(val + xOffset, val + yOffset, -val + zOffset);
            GL.Vertex3(val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(val + xOffset, -val + yOffset, val + zOffset);

            GL.Color3(0.0, 1.0, 1.0);
            GL.Vertex3(val + xOffset, -val + yOffset, val + zOffset);
            GL.Vertex3(val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, val + zOffset);

            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex3(val + xOffset, val + yOffset, val + zOffset);
            GL.Vertex3(val + xOffset, val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, val + yOffset, val + zOffset);

            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex3(val + xOffset, val + yOffset, -val + zOffset);
            GL.Vertex3(val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, -val + zOffset);
            GL.Vertex3(-val + xOffset, val + yOffset, -val + zOffset);

            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex3(val + xOffset, val + yOffset, val + zOffset);
            GL.Vertex3(val + xOffset, -val + yOffset, val + zOffset);
            GL.Vertex3(-val + xOffset, -val + yOffset, val + zOffset);
            GL.Vertex3(-val + xOffset, val + yOffset, val + zOffset);
        }


    }
}
