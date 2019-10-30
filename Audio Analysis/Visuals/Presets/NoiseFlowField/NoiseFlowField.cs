using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class NoiseFlowField : IVFX
    {
        const float maxBrightness = 1.0f;
        float brightness = maxBrightness;

        const double maxCubeScale = 3.0;
        double cubeScale = maxCubeScale;

        const double minArtScale = 200;
        const double maxArtScale = minArtScale * 1.1;
        double artScale = minArtScale;
        
        double spectrumYScale = 6.0;
        const double minSpectrumXScale = 0.5;
        double spectrumXscale = minSpectrumXScale;

        const int numCells = 8;
        const float cellSize = 5;

        const double maxMoveSpeed = 4;
        double moveSpeed = maxMoveSpeed;

        FastNoise fastNoise;
        Vector3 gridSize;
        float increment;
        Vector3 offset, offsetSpeed;
        Vector3[,,] flowfieldDirection;

        DateTime lastDraw;
        float deltaTime;
        int numCubes = 30;
        List<Cube> cubes;
        BackgroundImage albumArt;
        Spectrum3D spectrum3D;

        public NoiseFlowField()
        {
            gridSize.X = gridSize.Y = gridSize.Z = numCells * cellSize;

            cubes = new List<Cube>(numCubes);
            albumArt = new BackgroundImage(gridSize.X / 2, gridSize.Y / 2, -gridSize.Z-120);
            spectrum3D = new Spectrum3D(0, 0, 40);
            spectrum3D.Init();

            for (int i = 0; i < numCubes; i++)
            {
                cubes.Add(new Cube(
                    Rand.NextDouble() * gridSize.X,
                    Rand.NextDouble() * gridSize.Y,
                    Rand.NextDouble() * gridSize.Z));
            }
        }

        void CalculateFlowFieldDirections()
        {
            float deltaTime = (float)((DateTime.Now - lastDraw).TotalMilliseconds * 1000);

            offset = new Vector3(offset.X + (offsetSpeed.X * deltaTime), offset.Y + (offsetSpeed.Y * deltaTime), offset.Z + (offsetSpeed.Z * deltaTime));

            float noise;
            float xOff = 0f;
            for (int x = 0; x < gridSize.X; x++)
            {
                float yOff = 0f;

                for (int y = 0; y < gridSize.Y; y++)
                {
                    float zOff = 0f;

                    for (int z = 0; z < gridSize.Z; z++)
                    {
                        noise = fastNoise.GetSimplex(xOff + offset.X, yOff + offset.Y, zOff + offset.Z) + 1;
                        Vector3 noiseDirection = new Vector3((float)Math.Cos(noise * Math.PI), (float)Math.Sin(noise * Math.PI), (float)Math.Cos(noise * Math.PI));
                        flowfieldDirection[x, y, z] = Vector3.Normalize(noiseDirection);
                        zOff += increment;
                    }

                    yOff += increment;
                }

                xOff += increment;
            }
        }

        void CubeBehavior()
        {
            foreach (Cube c in cubes)
            {
                ////check edges - x
                //if (c.transform.position.x > this.transform.position.x + (gridSize.X * cellSize))
                //{
                //    c.transform.position = new Vector3(this.transform.position.x, c.transform.position.y, c.transform.position.z);
                //}
                //if (c.transform.position.x < this.transform.position.x)
                //{
                //    c.transform.position = new Vector3(this.transform.position.x + (gridSize.x * cellSize), c.transform.position.y, c.transform.position.z);
                //}
                //// y
                //if (c.transform.position.y > this.transform.position.y + (gridSize.Y * cellSize))
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y, c.transform.position.z);
                //}
                //if (c.transform.position.y < this.transform.position.y)
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y + (gridSize.y * cellSize), c.transform.position.z);
                //}
                //// z
                //if (c.transform.position.z > this.transform.position.z + (gridSize.Z * cellSize))
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z);
                //}
                //if (c.transform.position.z < this.transform.position.z)
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z + (gridSize.z * cellSize));
                //}

                Vector3 particlePos = new Vector3(
                    (float)Math.Floor((c.position.x) / cellSize),
                    (float)Math.Floor((c.position.y) / cellSize),
                    (float)Math.Floor((c.position.z) / cellSize)
                    );

                //c.ApplyRotation(flowfieldDirection[particlePos.X, particlePos.Y, particlePos.Z], particleRotateSpeed);
                //c.moveSpeed = particleMoveSpeed;
                //p.transform.localScale = new Vector3(particleScale,particleScale,particleScale);
            }
        }


        public void PreDraw()
        {
            //VisEnv.DoLighting2(brightness, 0.2f);

            GL.Translate(-gridSize.X / 2, -gridSize.Y / 2, -80.0); //Create desired perspective by drawing everything far away and centering the grid
        }



        public void Draw()
        {
            GL.PushMatrix(); //Save current matrix

            if (jitter)
            {
                albumArt.Jitter(Rand.NextDoubleNeg());
            }
            albumArt.TranslateTo();
            albumArt.SetScale(artScale);
            albumArt.Draw();

            GL.PopMatrix(); //Restore previously saved matrix

            foreach (Cube c in cubes)
            {
                GL.PushMatrix(); //Save current matrix

                if (jitter) c.Jitter(Rand.NextDoubleNeg() / 4);

                c.Fall(moveSpeed);
                if (c.position.y < 0) c.position.y += gridSize.Y;

                c.TranslateTo();

                c.Pitch(Rand.NextDouble());

                c.SetScale(cubeScale, cubeScale, cubeScale);

                c.Draw();

                GL.PopMatrix(); //Restore previously saved matrix
            }

            GL.PushMatrix();
            GL.Translate(gridSize.X / 2, gridSize.Y / 2, 0);
            spectrum3D.SetXScale(spectrumXscale);
            spectrum3D.SetYScale(spectrumYScale);
            spectrum3D.Draw();
            spectrum3D.TranslateTo();
            GL.PopMatrix();
        }

        public void PostDraw()
        {
            lastDraw = DateTime.Now;
            if (moveSpeed > 0.15) moveSpeed /= 1.5;
            if (cubeScale > 0.3) cubeScale -= 0.10;
            if (brightness > 0.5f) brightness -= 0.05f / 3;
            if (cubeScale < 3 * maxCubeScale / 4) jitter = false;
            if (artScale > minArtScale) artScale -= 0.01 * artScale;
            if (spectrumYScale >= .20) spectrumYScale -= 0.20;
            if (spectrumXscale <= 1.5) spectrumXscale += 0.01;
        }

        bool jitter = false;
        public void Trigger1()
        {
            //cubeScale = maxCubeScale;
            artScale = maxArtScale;
            jitter = true;
            brightness = maxBrightness;
            spectrumXscale = minSpectrumXScale;
            moveSpeed = maxMoveSpeed;
        }

        public void Trigger2()
        {
            //moveSpeed = maxMoveSpeed;
            cubeScale = maxCubeScale;

            albumArt.position.x = gridSize.X / 2;
            albumArt.position.y = gridSize.Y / 2;
        }

        public void Trigger3(float amplitude = 0.0f)
        {
            spectrumYScale = .01 * amplitude;
        }

        public void Trigger4(int index, float amplitude = 0.0f)
        {

        }
    }
}

