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
        Random r = new Random();

        const double maxmaxScale = 3;

        int cubesPerSide = 8;
        int spacePerCube = 10;
        double scale = maxmaxScale;
        double angle = 0;
        bool locked;

        List<Cube2> cubes = new List<Cube2>();

        public CubeMatrix()
        {
            int x = 0;
            int y = 0;
            int z = 0;

            for (int i = 0; i < cubesPerSide; i++)
            {
                x = i * spacePerCube;
                for (int j = 0; j < cubesPerSide; j++)
                {
                    y = j * spacePerCube;
                    for (int k = 0; k < cubesPerSide; k++)
                    {
                        z = k * -spacePerCube;
                        cubes.Add(new Cube2(x, y, z));
                    }
                }
            }
        }

        public void PreDraw()
        {
            double translate = (cubesPerSide * spacePerCube / 2) - (spacePerCube / 2);
            GL.Rotate(angle, 1.0, 0.0, 0.0);
            GL.Rotate(angle, 1.0, 0.0, 1.0);
            GL.Translate(-translate, -translate, translate);
        }

        public void Draw()
        {
            foreach (Cube2 c in cubes)
            {
                GL.PushMatrix(); //Save current matrix
                
                GL.Translate(c.position.x, c.position.y, c.position.z); //Set origin to center of cube

                c.angle.y += r.NextDouble(); //Adjust Angle
                c.angle.z += r.NextDouble(); //Adjust Angle

                //Execute Rotation
                GL.Rotate(c.angle.x, 1.0, 0.0, 0.0);
                GL.Rotate(c.angle.y, 0.0, 1.0, 0.0);
                GL.Rotate(c.angle.z, 0.0, 0.0, 1.0);

                GL.Scale(scale, scale, scale); //Set scale of cube

                c.Draw(); //Draw cube

                GL.PopMatrix(); //Restore previously saved matrix
            }
        }

        public void PostDraw()
        {
            scale -= 0.05;
            if (!locked) angle += 0.5;
        }
        
        public void Trigger1()
        {
            scale = maxmaxScale;
            locked = false;
        }

        public void Trigger2()
        {
            locked = true;
        }
    }
}
