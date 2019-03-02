using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    /// <summary>
    /// Idea: Add a second row of cubes that cycle during the snare. This would require a rewrite that causes the cubes to move rather than the camera.
    /// </summary>
    class SnakingTiles : IVFX
    {
        const double maxCubeScale = 1.15;
        double cubeScale = maxCubeScale;

        enum Direction { Left, Up, Down, Right };
        Direction lastDir = Direction.Right;
        Direction cameraDir = Direction.Right;

        Position cameraPos;
        List<Cube> cubes;
        Cube background;
        Dictionary<string, int> ci;
        double distance = 1.3;

        public SnakingTiles()
        {
            ci = new Dictionary<string, int>();
            cameraPos = new Position(distance, 0, 0);

            background = new Cube(cameraPos.x,cameraPos.y,cameraPos.z-5);
            background.SetScale(xScale: 30, yScale: 30);

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
                cubes.Add(new Cube(i * distance, 0, 0));
            }
        }

        public void PreDraw()
        {
            GL.Translate(-cameraPos.x, -cameraPos.y, -3.0); //Create desired perspective by drawing everything far away and centering the grid


        }

        public void Draw()
        {
            if (moveCamera)
            {
                MoveCamera();

                if (--steps <= 0)
                {
                    //Move Cube
                    Direction nextDir = GetNextDirection();
                    switch (nextDir)
                    {
                        case Direction.Left:
                            cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x - distance;
                            cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y;
                            break;
                        case Direction.Right:
                            cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x + distance;
                            cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y;
                            break;
                        case Direction.Up:
                            cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x;
                            cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y + distance;
                            break;
                        case Direction.Down:
                            cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x;
                            cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y - distance;
                            break;
                        default:
                            break;
                    }

                    lastDir = nextDir;

                    //Rotate Keys
                    int future = ci["future"];
                    ci["future"] = ci["previous"];
                    ci["previous"] = ci["current"];
                    ci["current"] = ci["next"];
                    ci["next"] = future;
                    
                    cubes[ci["future"]].RandomizeColor();

                    moveCamera = false;
                }
            }

            foreach (Cube c in cubes)
            {
                GL.PushMatrix(); //Save current matrix

                c.SetScale(cubeScale,cubeScale,cubeScale);

                c.TranslateTo();

                c.Draw();

                GL.PopMatrix(); //Restore previously saved matrix
            }

            GL.PushMatrix();

            background.TranslateTo();

            background.Draw();

            GL.PopMatrix();
        }

        public void PostDraw()
        {
            if (cubeScale > 1) cubeScale -= 0.005;
        }

        bool moveCamera = false;
        int steps = 0;
        double stepSize = 0;
        public void Trigger1()
        {
            if (moveCamera) return;
            cameraDir = GetCameraDirection();
            moveCamera = true;
            steps = 5;
            stepSize = distance / steps;
        }


        public void Trigger2()
        {
            cubeScale = maxCubeScale;
        }

        public void Trigger3(float amplitude = 0.0f)
        {
            for(int i = 0; i < background.color.Count() - 1; i++)
            {
                background.color[i] += amplitude * Rand.NextFloatNeg() * 0.0005;
                background.color[i] %= 1;
            }
        }

        Direction GetNextDirection()
        {
            Direction nextDir = new Direction();
            while (true)
            {
                nextDir = (Direction)(Rand.NextInt() % 4);
                switch (lastDir)
                {
                    case Direction.Left:
                        if (nextDir != Direction.Right)
                            return nextDir;
                        break;
                    case Direction.Right:
                        if (nextDir != Direction.Left)
                            return nextDir;
                        break;
                    case Direction.Up:
                        if (nextDir != Direction.Down)
                            return nextDir;
                        break;
                    case Direction.Down:
                        if (nextDir != Direction.Up)
                            return nextDir;
                        break;
                    default:
                        break;

                }
            }
        }

        Direction GetCameraDirection()
        {
            double xDifference = cubes[ci["next"]].position.x - cameraPos.x;
            double yDifference = cubes[ci["next"]].position.y - cameraPos.y;

            if (xDifference > 0.01)
            {
                return Direction.Right;
            }
            else if (xDifference < -0.01)
            {
                return Direction.Left;
            }
            else if (yDifference > 0.01)
            {
                return Direction.Up;
            }
            else //(yDifference < -0.01)
            {
                return Direction.Down;
            }

        }

        void MoveCamera()
        {
            switch (cameraDir)
            {
                case Direction.Left:
                    cameraPos.x -= stepSize;
                    break;
                case Direction.Right:
                    cameraPos.x += stepSize;
                    break;
                case Direction.Up:
                    cameraPos.y += stepSize;
                    break;
                case Direction.Down:
                    cameraPos.y -= stepSize;
                    break;
                default:
                    break;
            }
            background.position.x = cameraPos.x;
            background.position.y = cameraPos.y;
        }

        //void MoveAndReassign()
        //{
        //    //Move Camera
        //    cameraPos.x = cubes[ci["next"]].position.x;

        //    //Move Cube
        //    cubes[ci["previous"]].position.x = cubes[ci["future"]].position.x + 1.1;
        //    cubes[ci["previous"]].position.y = cubes[ci["future"]].position.y;

        //    //Rotate Keys
        //    int future = ci["future"];
        //    ci["future"] = ci["previous"];
        //    ci["previous"] = ci["current"];
        //    ci["current"] = ci["next"];
        //    ci["next"] = future;
        //}
    }
}
