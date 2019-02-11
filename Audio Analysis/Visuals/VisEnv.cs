﻿using System;
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
            Visuals.Preset = new NoiseFlowField();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);

            GL.Enable(EnableCap.DepthTest);
            
            base.OnLoad(e);

        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, base.Width, base.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4.CreatePerspectiveFieldOfView((Visuals.PI / 180) * 45.0f, base.Width / base.Height, 1.0f, 1000.0f, out Matrix4 matrix);
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
