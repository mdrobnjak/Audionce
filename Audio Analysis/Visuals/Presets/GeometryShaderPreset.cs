using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class GeometryShaderPreset : IVFX
    {
        const double maxScale = 6.0;
        double scale = maxScale;

        Spectrum3D spectrum3D;

        public GeometryShaderPreset()
        {
            spectrum3D = new Spectrum3D(0, 0, 0);
        }        

        public void PreDraw()
        {
            GL.Translate(0, 0, -400.0); //Create desired perspective by drawing everything far away and centering the grid
        }

        public void Draw()
        {

            GL.PushMatrix();
                        
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
            scale = .02 * amplitude;
        }
    }
}
