using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        static int texture;

        public static Color4 clearColor;
        
        protected override void OnLoad(EventArgs e)
        {
            clearColor = new Color4(0,0,0,0);

            GL.ClearColor(clearColor);

            GL.Enable(EnableCap.DepthTest);

            DoLighting2(1.0f * 1, 0.5f);

            DoTexture();

            base.OnLoad(e);
        }

        void DoTexture()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            #region LoadImage
            if (FileIO.Path == null) FileIO.InitPath();
            Bitmap bmp = new Bitmap(FileIO.Path + "SynriseSoulwaxRemix.bmp");
            //PictureAnalysis.GetMostUsedColor(bmp);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData texData = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);            
            #endregion
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, texData.Width, texData.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, texData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            bmp.UnlockBits(texData);
        }

        //public static void DoLighting(float diffuseBrightness, float ambientBrightness)
        //{
        //    GL.Enable(EnableCap.Lighting);
        //    GL.Enable(EnableCap.ColorMaterial);
        //    float[] lightPosition = { 0, 0, 100 };
        //    float[] diffuseColor = { 1.0f, 1.0f, 1.0f };
        //    for (int i = 0; i < 3; i++)
        //    {
        //        diffuseColor[i] *= diffuseBrightness;
        //    }
        //    float[] ambientColor = { 1.0f, 1.0f, 1.0f };
        //    for (int i = 0; i < 3; i++)
        //    {
        //        ambientColor[i] *= ambientBrightness;
        //    }
        //    GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
        //    GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseColor);
        //    GL.Light(LightName.Light0, LightParameter.Ambient, ambientColor);
        //    GL.Enable(EnableCap.Light0);
        //}

        public static void DoLighting2(float diffuseBrightness, float ambientBrightness)
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            float[] lightPosition = { 0, 0, 100 };
            float[] diffuseColor = { 1.0f, 1.0f, 1.0f };
            for (int i = 0; i < 3; i++)
            {
                diffuseColor[i] *= diffuseBrightness;
            }
            float[] ambientColor = { 1.0f, 1.0f, 1.0f };
            for (int i = 0; i < 3; i++)
            {
                ambientColor[i] *= ambientBrightness;
            }
            GL.Light(LightName.Light1, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light1, LightParameter.Diffuse, diffuseColor);
            GL.Light(LightName.Light1, LightParameter.Ambient, ambientColor);
            GL.Enable(EnableCap.Light1);
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

            if (input.IsKeyDown(Key.Space))
            {
                Visuals.Preset.Trigger1();
                Arduino.Trigger(0);
            }

            if (input.IsKeyDown(Key.Keypad0))
            {
                Visuals.Preset.Trigger2();
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

