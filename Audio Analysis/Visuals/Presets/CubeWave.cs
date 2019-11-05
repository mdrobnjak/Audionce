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
        List<double> heightBuffer = new List<double>();

        public CubeWave()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
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
            GL.Translate(-2,-2.5,-8);

            angle += 0.1f;
            GL.Rotate(angle, 0, 1, 0);
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
                if (c.position.y > 0) c.Fall(0.05);
            }

            for(int i = 0; i < heightBuffer.Count; i++)
            {
                heightBuffer[i] -= 0.1;
            }
        }

        public void Trigger1()
        {
            cubes[12].position.y = 2;
            heightBuffer.Add(3);
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
