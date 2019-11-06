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
        Cube[,] cubes; 

        const int sideLength = 101;
        int maxPhase = sideLength - (sideLength / 2);

        List<int> waves = new List<int>();

        const int waveDelayFrames = 1;
        int waveDelayCounter = 0;

        public CubeWave()
        {
            cubes = new Cube[sideLength,sideLength];
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    cubes[i,j] = new Cube(i, 0, -j);
                    cubes[i,j].RandomizeColor();
                }
            }
        }

        float angle = 0.0f;

        public void PreDraw()
        {
            double xTranslate = -Math.Floor((double)sideLength / 2);
            double yTranslate = -(double)sideLength/2 * 0.5;
            double zTranslate = -sideLength * .5;

            //Start drawing from this position relative to the camera...
            GL.Translate(xTranslate, yTranslate, zTranslate);

            //but first translate to the center of the structure...
            GL.Translate(-xTranslate, 0, xTranslate);

            //so we can rotate around the y axis at this (x,z) coordinate.
            angle += 0.1f;
            GL.Rotate(angle, 0, 1, 0);

            //Then, translate back to the drawing position.
            GL.Translate(xTranslate, 0, -xTranslate);

            GL.Rotate(45, 1, 0, 0);
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
            
            if (waves.Count > 0)
            {
                if(waveDelayCounter == waveDelayFrames)
                {
                    waveDelayCounter = 0;
                    //Pop up each wave in waves.
                    //Get popup indices for each phase integer.
                    PopUpTheWaves();
                }
                else
                {
                    waveDelayCounter++;
                }
            }
        }
        
        void PopUpTheWaves()
        {
            for(int i = 0; i < waves.Count; i++)
            {
                if (waves[i] == maxPhase)
                {
                    waves.RemoveAt(i);
                    i--;
                    continue;
                }
                foreach(int[] indices in getIndicesForPhase(waves[i]))
                {
                    cubes[indices[0], indices[1]].position.y = 20;
                }
                waves[i]++;
            }
        }

        public void PostDraw()
        {
            foreach (Cube c in cubes)
            {
                if (c.position.y > 0) c.Fall(0.2);
            }
        }

        List<int[]> getIndicesForPhase(int phase)
        {
            List<int[]> indices = new List<int[]>();

            int center = sideLength / 2;

            int x = center;
            int y = center + phase;
               
            //Go to center,center+phase and scroll clockwise.

            indices.Add(new int[2] { x, y });

            for (x++; x <= center + phase; x++)
            {
                indices.Add(new int[2] { x, y });
            }
            x--;
            for (y--; y >= center - phase; y--)
            {
                indices.Add(new int[2] { x, y });
            }
            y++;
            for (x--; x >= center - phase; x--)
            {
                indices.Add(new int[2] { x, y });
            }
            x++;
            for (y++; y <= center + phase; y++)
            {
                indices.Add(new int[2] { x, y });
            }
            y--;
            for (x++; x < center; x++)
            {
                indices.Add(new int[2] { x, y });
            }
            
            return indices;
        }



        public void Trigger1()
        {
            waves.Add(0);
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
