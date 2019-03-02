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
        public double[] color;

        const double val = 0.5;

        Scale scale;
        public Position position;
        public Angle angle = new Angle(0.0, 0.0, 0.0);

        public Cube(double xPosition, double yPosition, double zPosition)
        {
            scale = new Scale(1, 1, 1);
            position = new Position(xPosition, yPosition, zPosition);
            RandomizeColor(); //color = new double[] { 1.0, 1.0, 1.0 };
        }

        public void RandomizeColor()
        {
            color = new double[] { Rand.NextDouble(), Rand.NextDouble(), Rand.NextDouble() };
        }

        void ApplyScale()
        {
            GL.Scale(scale.x, scale.y, scale.z);
        }

        void ApplyRotation()
        {
            GL.Rotate(this.angle.x, 1.0, 0.0, 0.0);
            GL.Rotate(this.angle.y, 0.0, 1.0, 0.0);
            GL.Rotate(this.angle.z, 0.0, 0.0, 1.0);
        }

        void SpecifyVertices()
        {
            //left
            GL.Normal3(-1.0, 0.0, 0.0);
            GL.Vertex3(-val, val, val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            //right
            GL.Normal3(1.0, 0.0, 0.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(val, -val, val);

            //bottom
            GL.Normal3(0.0, -1.0, 0.0);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            //top
            GL.Normal3(0.0, 1.0, 0.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, val, val);

            //back
            GL.Normal3(0.0, 0.0, -1.0);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, val, -val);

            //front
            GL.Normal3(0.0, 0.0, 1.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(-val, -val, val);
            GL.Vertex3(-val, val, val);
        }

        public void Draw()
        {
            ApplyRotation();

            ApplyScale();

            GL.Begin(PrimitiveType.Quads);

            GL.Color3(color);

            SpecifyVertices();

            GL.End();
        }

        public void SetScale(double xScale = -9999, double yScale = -9999, double zScale = -9999)
        {
            if (xScale != -9999) this.scale.x = xScale;
            if (yScale != -9999) this.scale.y = yScale;
            if (zScale != -9999) this.scale.z = zScale;
        }

        public void AdjustYScale(double increment)
        {
            this.scale.y += increment;
        }

        public void Jitter(double amount)
        {
            this.position.x += amount;
            this.position.z += amount;
        }

        public void Pitch(double amount)
        {
            this.angle.y += amount;
        }

        public void TranslateTo()
        {
            GL.Translate(this.position.x, this.position.y, this.position.z);
        }

        public void Fall(double amount)
        {
            this.position.y -= amount;
        }


    }
}
