using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class Spectrum3D
    {
        List<Cube> cubes;
        double scale = 10;

        public Position position;
        public Angle angle = new Angle(0.0, 0.0, 0.0);

        public Spectrum3D(double xPosition, double yPosition, double zPosition)
        {
            position = new Position(xPosition, yPosition, zPosition);
            cubes = new List<Cube>(8);
            Init();
        }

        void Init()
        {
            for(int i = 0; i < cubes.Capacity; i++)
            {
                cubes.Add(new Cube(position.x + i, position.y, position.z));
            }
            this.position.x -= ((cubes.Capacity - 1) * scale * 1.1)/ 2;
        }

        void ApplyScale()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].position.x = i * scale * 1.1;
            }
        }

        void ApplyRotation()
        {
            GL.Rotate(this.angle.x, 1.0, 0.0, 0.0);
            GL.Rotate(this.angle.y, 0.0, 1.0, 0.0);
            GL.Rotate(this.angle.z, 0.0, 0.0, 1.0);
        }

        void SpecifyVertices()
        {
        }

        public void Draw()
        {
            ApplyRotation();

            ApplyScale();
            
            for(int i = 0; i < cubes.Count; i++)
            {
                GL.PushMatrix();
                //cubes[i].position.x = i * scale * 1.1;
                cubes[i].TranslateTo();
                cubes[i].SetScale(scale);
                cubes[i].Draw();
                GL.PopMatrix();
            }
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
