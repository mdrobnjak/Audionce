using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class Demo : IVFX
    {
        const double maxCubeScale = 8;
        double cubeScale = maxCubeScale;

        const double maxSpectrumScale = 6.0;
        double spectrumScale = maxSpectrumScale;

        const double minArtScale = 200;
        const double maxArtScale = minArtScale * 1.1;
        double artScale = minArtScale;

        const double maxMoveSpeed = 4;
        double moveSpeed = maxMoveSpeed;

        const double maxRainScale = 5;
        double rainScale = maxCubeScale;

        const double maxRowDistance = 4;
        double rowDistance = maxRowDistance;

        List<Cube> rain;
        List<Cube> cubeRow;
        Spectrum3D spectrum3D;
        BackgroundImage albumArt;

        Vector3 gridSize;
        double distance = 1.3;
        const float cellSize = 5;

        public Demo()
        {
            spectrum3D = new Spectrum3D(0, 0, 0);
            cubeRow = new List<Cube>(40);
            albumArt = new BackgroundImage(0, 0, -400);
            rain = new List<Cube>(30);
            gridSize.Y = gridSize.Z = rain.Capacity * cellSize;

            gridSize.X = gridSize.Y * 2;

            Init();
        }

        void Init()
        {
            for (int i = 0; i < cubeRow.Capacity; i++)
            {
                cubeRow.Add(new Cube(-rowDistance, -1, -i * distance));
            }

            for (int i = 0; i < rain.Capacity; i++)
            {
                rain.Add(new Cube(
                    Rand.NextDouble() * gridSize.X - gridSize.X / 2,
                    Rand.NextDouble() * gridSize.Y - 2 * gridSize.Y / 1,
                    Rand.NextDouble() * gridSize.Z - 300.0));
            }
        }

        void InitializeColors()
        {
            for (int i = 0; i < cubeRow.Capacity; i++)
            {
                cubeRow[i].color[0] = 66 / 255.0 * (1+Rand.NextDoubleNeg() * 0.5);
                cubeRow[i].color[1] = 244 / 255.0;
                cubeRow[i].color[2] = 92 / 255.0;
            }
            for (int i = 0; i < spectrum3D.cubes.Capacity; i++)
            {
                spectrum3D.cubes[i].color[0] = 158 / 255.0;
                spectrum3D.cubes[i].color[1] = 66 / 255.0;
                spectrum3D.cubes[i].color[2] = 244 / 255.0 * (1 + Rand.NextDoubleNeg());
            }
            colorsInitialized = true;
        }

        bool colorsInitialized = false;
        public void PreDraw()
        {
            if (!colorsInitialized) InitializeColors();

            GL.Translate(0, 0, -3 * distance); //Create desired perspective by drawing everything far away and centering the grid
        }

        public void Draw()
        {
            if (cubeRow[0].position.z >= 3) ShiftToBack();

            for (int i = 0; i < cubeRow.Capacity; i++)
            {
                GL.PushMatrix(); //Save current matrix

                cubeRow[i].position.x = -rowDistance;
                cubeRow[i].position.z += distance / 3;
            
                if (i == cubeRow.Capacity - 2) cubeRow[i].SetScale(yScale: cubeScale);
                else if (i == cubeRow.Capacity - 1) cubeRow[i].SetScale(yScale: 1);

                cubeRow[i].TranslateTo();

                cubeRow[i].Draw();

                //Mirror
                GL.Translate(rowDistance*2, 0, 0);
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(cubeRow[i].color);
                cubeRow[i].SpecifyVertices();
                GL.End();

                GL.PopMatrix(); //Restore previously saved matrix
            }

            GL.PushMatrix();
            GL.Translate(0, 10, -40);
            spectrum3D.SetYScale(spectrumScale);
            spectrum3D.Draw();
            spectrum3D.TranslateTo();
            GL.PopMatrix();

            GL.PushMatrix(); //Save current matrix

            if (jitter)
            {
                albumArt.Jitter(Rand.NextDoubleNeg());
            }
            albumArt.TranslateTo();
            albumArt.SetScale(artScale);
            albumArt.Draw();

            GL.PopMatrix(); //Restore previously saved matrix

            foreach (Cube c in rain)
            {
                GL.PushMatrix(); //Save current matrix

                if (jitter) c.Jitter(Rand.NextDoubleNeg() / 4);

                c.Fall(moveSpeed);
                if (c.position.y < -80) c.position.y += gridSize.Y;

                c.TranslateTo();

                c.Pitch(Rand.NextDouble());

                c.SetScale(cubeScale, cubeScale, cubeScale);

                c.Draw();

                GL.PopMatrix(); //Restore previously saved matrix
            }
        }

        void ShiftToBack()
        {
            Cube temp = cubeRow[0];
            for (int i = 0; i < cubeRow.Capacity - 1; i++)
            {
                cubeRow[i] = cubeRow[i + 1];
            }
            cubeRow[cubeRow.Capacity - 1] = temp;
            cubeRow.Last().position.z -= (cubeRow.Capacity - 1) * distance;
        }

        public void PostDraw()
        {
            if (cubeScale >= 1.25) cubeScale -= 0.25;
            if (spectrumScale >= .20) spectrumScale -= 0.20;
            if (cubeScale < 3 * maxCubeScale / 4) jitter = false;
            if (artScale > minArtScale) artScale -= 0.01 * artScale;
            if (rainScale > 0.3) rainScale -= 0.10;
            if (moveSpeed > 0.15) moveSpeed /= 1.5;
            //if (rowDistance >= 2) rowDistance -= 0.01;
        }

        bool jitter = false;
        public void Trigger1()
        {
            cubeScale = maxCubeScale;
            artScale = maxArtScale;
            rainScale = maxRainScale;
            jitter = true;
            //rowDistance = maxRowDistance;
        }

        public void Trigger2()
        {

            moveSpeed = maxMoveSpeed;
            albumArt.position.x = 0;
            albumArt.position.y = 0;
        }

        public void Trigger3(float amplitude = 0.0f)
        {
            spectrumScale = .02 * amplitude;
        }
    }
}
