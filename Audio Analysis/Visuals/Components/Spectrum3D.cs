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
        public List<Cube> cubes;
        //double scale = 1;

        public Scale scale;
        public Position position;
        public Position drawPosition;
        public Angle angle = new Angle(0.0, 0.0, 0.0);

        public Spectrum3D(double xPosition, double yPosition, double zPosition)
        {
            scale = new Scale(1,1,1);
            position = drawPosition = new Position(xPosition, yPosition, zPosition);
            cubes = new List<Cube>(32);
            Init();
        }

        void Init()
        {
            drawPosition.x -= ((cubes.Capacity - 1) * 1.2) / 2;

            for (int i = 0; i < cubes.Capacity; i++)
            {
                cubes.Add(new Cube(drawPosition.x + i * 1.2, position.y, position.z));
            }
        }

        
        void ApplyScale()
        {
            GL.Scale(scale.x,scale.y,scale.z);
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
                cubes[i].TranslateTo();
                cubes[i].AdjustYScale(Rand.NextDoubleNeg()*0.001); 
                cubes[i].Draw();
                GL.PopMatrix();
            }
        }
        

        public void SetYScale(double scale)
        {
            this.scale.y = scale;
        }

        public void SetXScale(double scale)
        {
            this.scale.x = scale;
        }

        public void TranslateTo()
        {
            GL.Translate(this.position.x, this.position.y, this.position.z);
        }
    }
}
