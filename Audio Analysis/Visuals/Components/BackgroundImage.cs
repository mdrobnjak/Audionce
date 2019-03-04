using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class BackgroundImage
    {
        const double val = 0.5;
        double scale = 1;

        public Position position;
        public Angle angle = new Angle(0.0, 0.0, 0.0);

        public BackgroundImage(double xPosition, double yPosition, double zPosition)
        {
            position = new Position(xPosition, yPosition, zPosition);
        }

        void ApplyScale()
        {
            GL.Scale(scale, scale, scale);
        }

        void ApplyRotation()
        {
            GL.Rotate(this.angle.x, 1.0, 0.0, 0.0);
            GL.Rotate(this.angle.y, 0.0, 1.0, 0.0);
            GL.Rotate(this.angle.z, 0.0, 0.0, 1.0);
        }

        void SpecifyVertices()
        {
            //front
            GL.Normal3(0.0, 0.0, 1.0);
            GL.TexCoord2(0,1);
            GL.Vertex3(-val, -val, 0);
            GL.TexCoord2(1,1);
            GL.Vertex3(val, -val, 0);
            GL.TexCoord2(1,0);
            GL.Vertex3(val, val, 0);
            GL.TexCoord2(0,0);
            GL.Vertex3(-val, val, 0);
        }

        public void Draw()
        {
            ApplyRotation();

            ApplyScale();

            GL.Begin(PrimitiveType.Quads);

            GL.Color3(1.0,1.0,1.0);

            SpecifyVertices();

            GL.End();
        }

        public void SetScale(double scale)
        {
            this.scale = scale;
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
