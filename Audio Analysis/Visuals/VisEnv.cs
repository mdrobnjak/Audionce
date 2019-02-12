using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace AudioAnalyzer
{
    public class VisEnv : GameWindow
    {
        public VisEnv(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);

            DoLighting(1.0f);

            base.OnLoad(e);
        }

        public static void DoLighting(float brightness)
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            float[] lightPosition = { 0, 0, 100 };
            float[] lightColor = { 1.0f, 1.0f, 1.0f };
            for (int i = 0; i < 3; i++)
            {
                lightColor[i] *= brightness;
            }
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightColor);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightColor);
            GL.Enable(EnableCap.Light0);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, base.Width, base.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4.CreatePerspectiveFieldOfView((Visuals.PI / 180) * 45.0f, (float)base.Width / base.Height, 1.0f, 1000.0f, out Matrix4 matrix);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Visuals.Preset.PreDraw();

            Visuals.Preset.Draw();

            Context.SwapBuffers();

            Visuals.Preset.PostDraw();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            base.OnUnload(e);
        }
    }
}

