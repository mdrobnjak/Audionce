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
        float[] vertices =
        {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
        };

        uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        int ElementBufferObject;
        int VertexBufferObject;
        int VertexArrayObject;
        Shader shader;

        double time = 0.0;
        Matrix4 view, projection;

        public VisEnv(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Visuals.Preset = new CubeMatrix();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            string[] path = Environment.CurrentDirectory.Split('\\');
            Array.Resize(ref path, path.Count() - 3);
            string directory = string.Join(@"\", path) + @"\Audio Analysis\Visuals\";

            shader = new Shader(directory+"shader.vert", directory+"shader.frag");
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView((Visuals.PI / 180) * 45.0f, Width / Height, 0.1f, 100.0f);

            base.OnLoad(e);

        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, base.Width, base.Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();
            
            GL.BindVertexArray(VertexArrayObject);

            Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            
            base.OnRenderFrame(e);
            
            //GL.LoadIdentity();


            //Visuals.Preset.PreDraw();

            //Visuals.Preset.Draw();

            //window.SwapBuffers();

            //Visuals.Preset.PostDraw();
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
            
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            shader.Dispose();
            base.OnUnload(e);
        }
    }
}

