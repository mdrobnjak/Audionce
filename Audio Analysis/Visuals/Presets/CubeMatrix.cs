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

        List<Cube> cubes = new List<Cube>();

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
                        cubes.Add(new Cube(x, y, z));
                        cubes.Last().RandomizeColor();
                    }
                }
            }
        }

        public void PreDraw()
        {
            double translate = (cubesPerSide * spacePerCube / 2) - (spacePerCube / 2);
            GL.Rotate(angle, 0.5, 0.0, 0.0);
            GL.Rotate(angle, 0.5, 0.0, 0.5);
            GL.Translate(-translate, -translate, translate);
        }

        public void Draw()
        {
            foreach (Cube c in cubes)
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
            if(scale > 0.05)scale -= 0.05;
            if (!locked) angle += 0.2;
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

        public void Trigger3(float amplitude = 0.0f)
        {

        }
    }
}
