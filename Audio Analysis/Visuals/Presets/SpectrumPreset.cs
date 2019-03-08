using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class SpectrumPreset : IVFX
    {
        const double maxScale = 6.0;
        double scale = maxScale;

        Spectrum3D spectrum3D;

        public SpectrumPreset()
        {
            spectrum3D = new Spectrum3D(0,0,0);
            spectrum3D.Init(Range.Ranges[0].NumBands);
        }

        public void PreDraw()
        {
            GL.Translate(0, 0, -40.0); //Create desired perspective by drawing everything far away and centering the grid
        }

        public void Draw()
        {

            GL.PushMatrix();
            spectrum3D.SetYScale(scale);
            spectrum3D.Draw();
            GL.PopMatrix();

        }

        public void PostDraw()
        {
            if (scale >= .20) scale -= 0.20;
        }
        
        public void Trigger1()
        {

        }

        public void Trigger2()
        {

        }

        public void Trigger3(float amplitude = 0.0f)
        {
            //scale = .02 * amplitude;
        }

        public void Trigger4(int index)
        {
            if (index < 2 || index > 6) return;
            foreach (Cube c in spectrum3D.cubes)
            {
                c.SetScale(yScale: 1);
            }
            spectrum3D.cubes[index].SetScale(yScale:10);
        }
    }
}
