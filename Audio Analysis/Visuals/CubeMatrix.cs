using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class CubeMatrix : IVFX
    {
        const double maxLength = 2;

        int cubesPerSide = 8;
        int offset = 10;
        double length = maxLength;
        double angle = 0;
        bool locked;

        List<Cube> cubes = new List<Cube>();

        public CubeMatrix()
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
                    for (int k = 0; k < cubesPerSide; k++)
                    {
                        z = k * -offset;
                        cubes.Add(new Cube(length, x, y, z));
                    }
                }
            }
        }

        public void PreDraw()
        {
            double translate = (cubesPerSide * offset / 2) - (offset / 2);
            GL.Rotate(angle, 1.0, 0.0, 0.0);
            GL.Rotate(angle, 1.0, 0.0, 1.0);
            GL.Translate(-translate, -translate, translate);
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);
                        
            foreach(Cube c in cubes)
            {
                c.SetLength(length);
                c.SpecifyVertices();
            }

            GL.End();
        }

        public void PostDraw()
        {
            length -= 0.05;
            if (!locked) angle += 0.5;
        }
        
        public void Trigger1()
        {
            length = maxLength;
            locked = false;
        }

        public void Trigger2()
        {
            locked = true;
        }
    }
}
