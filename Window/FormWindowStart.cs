using openGL2.Objects;
using openGL2.Shaders;
using openGL2.Textures;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace openGL2.Window
{
    public class FormWindowStart
    {
        private Figure square;
        private Figure square2;

        private Shader shader;
        private Camera camera;

        OpenTK.GLControl.GLControl controller;

        // der er en dedikeret VAO til de forskellige VAO man får brug for

        int VAO;
        int VBO;

        public FormWindowStart(OpenTK.GLControl.GLControl controller)
        {

            this.controller = controller;
            camera = new Camera(new Vector2 (controller.Size.Width, controller.Size.Height));
        }


        public void Load()
        {
            controller.MakeCurrent();
            
            //square = new Figure(shader, Figure.FigureType.QUAD, false);
            //square2 = new Figure(shader, Figure.FigureType.CUBE, true);
            square2.TranslateFigure(Matrix4.Identity);

            GL.ClearColor(new Color4(0.3f, 0.4f, 0.7f, 1));

            VBO = square.VBOHandle;
            VAO = square.VAOHandle;

            GL.Enable(EnableCap.DepthTest);
            // texture new:
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

          

            //testTex.Create(@"..\..\..\Textures\TextureImages\Rainbow.tga", 0);
            //int tex1 = Texture.Create(@"..\..\..\Textures\TextureImages\rock.tga");
            //int tex2 = Texture.Create(@"..\..\..\Textures\TextureImages\Rainbow.tga");

            //int tex1 = Texture.Create(@"C:\Users\p-hou\source\repos\openGL2\Textures\TextureImages\LightmapTest.tga");
            //int tex2 = Texture.Create(@"C:\Users\p-hou\source\repos\openGL2\Textures\TextureImages\Rainbow.tga");

            //square.Albedo = tex1;
            //square2.Albedo = tex2;

            camera.UpdateView();
        }


        public  void  Render(PaintEventArgs e)
        {
           
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            camera.UpdateView();

            ObjectHandler.DrawAllFiguresInScene();

            
        }




        //protected override void OnResize(ResizeEventArgs e)
        //{
        //    base.OnResize(e);
        //    GL.Viewport(0, 0, Size.X, Size.Y);
        //}

        //protected override void OnUpdateFrame(FrameEventArgs args)
        //{
        //    base.OnUpdateFrame(args);
        //}

        //protected override void OnUnload()
        //{
        //    // Unbind all the resources by binding the targets to 0/null.
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    GL.BindVertexArray(0);
        //    GL.UseProgram(0);

        //    // Delete all the resources.
        //    GL.DeleteBuffer(VBO);
        //    GL.DeleteVertexArray(VAO);

        //    GL.DeleteProgram(shader.ShaderProgramHandle);

        //    base.OnUnload();
        //}

        


    }
}
