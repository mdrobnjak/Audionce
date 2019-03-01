using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    /// <summary>
    /// Idea 1: Add a second row of cubes that cycle during the snare. This would require a rewrite that causes the cubes to move rather than the camera.
    /// Idea 2: Randomize the placement of each new "future" tile (up, down, left, right), and have the camera follow accordingly.
    /// </summary>
    class MovingTiles : IVFX
    {
        Position cameraPos;
        List<Cube> cubes;
        Dictionary<string, int> ci;

        public MovingTiles()
        {
            ci = new Dictionary<string, int>();
            cameraPos = new Position(1.1, 0, 0);
            cubes = new List<Cube>(4);

            Init();
        }

        void Init()
        {
            ci.Add("previous", 0);
            ci.Add("current", 1);
            ci.Add("next", 2);
            ci.Add("future", 3);
            for (int i = 0; i < cubes.Capacity; i++)
            {
                cubes.Add(new Cube(i * 1.1, 0, 0));
            }
        }

        public void PreDraw()
        {
            GL.Translate(-cameraPos.x, -cameraPos.y, -4.0); //Create desired perspective by drawing everything far away and centering the grid


        }

        public void Draw()
        {
            if (moveCamera)
            {
                cameraPos.x += stepSize;

                if (--steps <= 0)
                {
                    //Move Cube
                    cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x + 1.1;
                    cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y;

                    //Rotate Keys
                    int future = ci["future"];
                    ci["future"] = ci["previous"];
                    ci["previous"] = ci["current"];
                    ci["current"] = ci["next"];
                    ci["next"] = future;

                    moveCamera = false;
                }
            }

            foreach (Cube c in cubes)
            {
                GL.PushMatrix(); //Save current matrix

                c.TranslateTo();

                c.Draw();

                GL.PopMatrix(); //Restore previously saved matrix
            }
        }

        public void PostDraw()
        {
            
        }

        bool moveCamera = false;
        int steps = 0;
        double stepSize = 0;
        public void Trigger1()
        {
            if (moveCamera) return;
            moveCamera = true;
            steps = 5;
            stepSize = (cubes[ci["next"]].position.x - cameraPos.x) / steps;
        }

        public void Trigger2()
        {

        }

        public void Trigger3(float amplitude = 0.0f)
        {
        }

        void MoveAndReassign()
        {
            //Move Camera
            cameraPos.x = cubes[ci["next"]].position.x;

            //Move Cube
            cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x + 1.1;
            cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y;

            //Rotate Keys
            int future = ci["future"];
            ci["future"] = ci["previous"];
            ci["previous"] = ci["current"];
            ci["current"] = ci["next"];
            ci["next"] = future;
        }
    }
}
