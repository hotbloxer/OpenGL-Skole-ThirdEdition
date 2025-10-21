using ImGuiNET;
using openGL2.Objects;
using openGL2.Shaders;
using openGL2.Textures;
using ImGuiNET;
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
using Vortice.Direct3D11;
using Xunit;



namespace openGL2.Window
{
    public class Window : GameWindow
    {
        private readonly Figure cube;
        private readonly Figure square;

        private readonly Shader shader;
      
        private readonly Camera camera;


        // der er en dedikeret VAO til de forskellige VAO man får brug for

        int VAO;
        int VBO;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) :
            base(gameWindowSettings, nativeWindowSettings)
        {
            shader = new Shader();
            cube = new Figure(shader, Figure.FigureType.CUBE, false);

            square = new Figure(shader, Figure.FigureType.QUAD, true)
            {
                Render = true
            };

            cube.MoveFigure();
            camera = new Camera(this);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(new Color4(0.3f, 0.4f, 0.2f, 1));

            //REFA de her skal ligge på objecktet i stedet for her
            VBO = cube.GetVBOHandle;
            VAO = cube.GetVAOHandle;

            GL.Enable(EnableCap.DepthTest);

            // load all default textures
            new Texture(@"..\..\..\Textures\TextureImages\rock.tga", "rock");
            new Texture(@"..\..\..\Textures\TextureImages\Rainbow.tga", "rain");
            Texture.GetWhite();
            Texture Chekered = Texture.GetCheckered();
            new Texture(@"..\..\..\Textures\TextureImages\bulletNormal.tga", "bullet");
            new Texture(@"..\..\..\Textures\TextureImages\testTex.tga", "testTex");

            // TODO fix conflicting names and textures
            //cube.Albedo = new Texture(@"..\..\..\Textures\TextureImages\brickAlbedo.tga", "albedo");
            //square.Albedo = cube.Albedo;
            //cube.LightMap = new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap");
            //square.LightMap = cube.LightMap;
            //cube.SpecularMap = new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular");
            //square.SpecularMap = cube.SpecularMap;
            square.NormalTexture = new Texture(@"..\..\..\Textures\TextureImages\brickNormal.tga", "normal");
            cube.NormalTexture = square.NormalTexture;

            cube.Albedo =            Texture. GetWhite();
            square.Albedo =          Texture. GetWhite();
            cube.LightMap =          Texture. GetWhite();
            square.LightMap =        Texture. GetWhite();
            cube.SpecularMap =       Texture. GetWhite();
            square.SpecularMap =     Texture. GetWhite();
        

            camera.UpdateView();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
   
            //TODO spørg søren om de behøver deklerers hver gang
            KeyboardState input;
            MouseState mouse;
            if (IsFocused) 
            {
                // disse er kun nødvendige hvis winduet faktisk er i fokus
                input = KeyboardState;
                mouse = MouseState;
                camera.UpdateCamera(input, args, mouse);
            }   

            camera.UpdateView();

            ObjectHandler.DrawAllFiguresInScene();

            this.Context.SwapBuffers();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }


        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);

            // TODO delete ALL SHADERS
            GL.DeleteProgram(shader.ShaderProgramHandle);

            base.OnUnload();
        }
    }
}
