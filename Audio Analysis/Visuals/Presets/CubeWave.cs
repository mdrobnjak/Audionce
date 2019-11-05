using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class CubeWave : IVFX
    {
        List<Cube> cubes = new List<Cube>();

        int size = 101;

        public CubeWave()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cubes.Add(new Cube(i, 0, -j));
                    cubes.Last().RandomizeColor();
                }
            }
            //cubes.Add(new Cube(0,0,0));
        }

        float angle = 0.0f;

        public void PreDraw()
        {
            double xTranslate = -Math.Floor((double)size / 2);
            double yTranslate = -(double)size/2;
            double zTranslate = -size * 2;

            GL.Translate(xTranslate, yTranslate, zTranslate);

            GL.Translate(-xTranslate, 0, xTranslate);

            angle += 0.1f;
            GL.Rotate(angle, 0, 1, 0);

            GL.Translate(xTranslate, 0, -xTranslate);
        }

        public void Draw()
        {
            foreach (Cube c in cubes)
            {
                GL.PushMatrix(); //Save current matrix

                GL.Translate(c.position.x, c.position.y, c.position.z); //Set origin to center of cube

                c.Draw(); //Draw cube

                GL.PopMatrix(); //Restore previously saved matrix
            }
        }

        public void PostDraw()
        {
            foreach (Cube c in cubes)
            {
                if (c.position.y > 0) c.Fall(0.5);
            }
        }

        public void Trigger1()
        {
            int centerIndex = size * size / 2;
            cubes[centerIndex].position.y = 10;
        }

        public void Trigger2()
        {
            foreach (Cube c in cubes)
            {
                c.RandomizeColor();
            }
        }
    }
}
