
using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using openGL2.Objects;
using openGL2.Objects.Scenes;
using openGL2.Objects.Terrain;
using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;


namespace openGL2.Window
{
    public class Window : GameWindow, IHaveUI
    {

        private Shader shader;
        private Shader backgroundShader;
        private Camera camera;

        UI ui;
        Sun sun = new Sun();

        SkyBoxShader skyBoxShader;

        Scene _defaultScene;
        Scene _grassScene;
        Scene _sunDisplayScene;
        Scene _vertexSelectorShader;
        KeyboardState input;
        public static MouseState mouse;

        public enum Scenes { DEFAULT, GRASS, SUNANIMATION, VERTEXSELECTOR}
        Scene CurrentScene;

        // der er en dedikeret VAO til de forskellige VAO man får brug for

        public Window() :
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900), APIVersion = new Version(3, 3) })
        {
  
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);

            camera = new(this)
            {
                Position = new(0.0f, 1.0f, 0.0f)
            };
            
            ImGui.CreateContext();
            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

            // use skybox
            skyBoxShader = new();
            UI.UseEnvironment = true;

            _defaultScene = new DefaultScene();
            _grassScene = new GrassScene();
            _sunDisplayScene = new SunDisplayScene();
            _vertexSelectorShader = new EditVerticesScene(camera);

            CurrentScene = _vertexSelectorShader;
            CurrentScene.Load();

            

            camera.UpdateView();

            ui = new();
            ui.windowUI = this;

            ImguiImplOpenTK4.Init(this);
            ImguiImplOpenGL3.Init();


            // alle winduer sættes til at være transparente og herefter sat til deres farve i UI
            // dette er fordi alt er fucked! og jeg ikke kan få render rækkefølgen til at passe ordentligt!
            ImGui.GetStyle().Colors[(int)ImGuiCol.WindowBg] = (System.Numerics.Vector4)new Vector4(0, 0, 0, 0);




        }
       
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            camera.UpdateView();
            
            if (UI.UseEnvironment)
            {
                skyBoxShader.View = camera.View;
                skyBoxShader.Projection = camera.Projection;
                skyBoxShader.Draw();
            }

            ShaderHandler.UpdateAllShaders();
            ObjectHandler.DrawAllFiguresInScene();


            CurrentScene.OnRenderFrame();

            ImguiImplOpenGL3.NewFrame();
            ImguiImplOpenTK4.NewFrame();
            ImGui.NewFrame();

            ImGui.DockSpaceOverViewport();

            ui.Ui(); // ui elements
            ui.RenderView(); // view of render. actually just a regular window... but transparanet

            Sun.Instance.UpdateSunAnimation();
            ImGui.Render();
            GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
        

            ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

            if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
            {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
                Context.MakeCurrent();
            }


            if (IsFocused)
            {
                //disse er kun nødvendige hvis winduet faktisk er i fokus
                input = KeyboardState;
                mouse = MouseState;
                camera.UpdateCamera(input, args, mouse);

            }







            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);


            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            this.Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        
        protected override void OnUnload()
        {

            //TODO implement ressource handling on close
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            //// Delete all the resources.
            //GL.DeleteBuffer(VBO);
            //GL.DeleteVertexArray(VAO);

            // TODO delete ALL SHADERS
            //GL.DeleteProgram(shader.ShaderProgramHandle);

            base.OnUnload();
        }

        
        public void ChangeSceneTo (Scenes scene)
        {
            CurrentScene.CloseScene();
            switch (scene)
            {
                case Scenes.DEFAULT:
                    CurrentScene = _defaultScene;
                    break;

                case Scenes.GRASS:
                    CurrentScene = _grassScene;
                    break;

                case Scenes.SUNANIMATION:
                    CurrentScene = _sunDisplayScene;
                    break;

                case Scenes.VERTEXSELECTOR:
                    CurrentScene = _vertexSelectorShader;
                    break;

            }

            CurrentScene.OpenScene();

        }

        public bool GetUI()
        {
            if (ImGui.Button("Default Scene")) 
            {
                ChangeSceneTo(Scenes.DEFAULT);
            }

            if (ImGui.Button("Grass Scene"))
            {
                ChangeSceneTo(Scenes.GRASS);
            }

            if (ImGui.Button("Sun Scene"))
            {
                ChangeSceneTo(Scenes.SUNANIMATION);
            }

            if (ImGui.Button("Vertex selector Scene"))
            {
                ChangeSceneTo(Scenes.VERTEXSELECTOR);
            }


            return true;
        }


        public readonly static DebugProc DebugProcCallback = Window_DebugProc;
        private static void Window_DebugProc(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr messagePtr, IntPtr userParam)
        {
            string message = Marshal.PtrToStringAnsi(messagePtr, length);

            bool showMessage = true;

            switch (source)
            {
                case DebugSource.DebugSourceApplication:
                    showMessage = false;
                    break;
                case DebugSource.DontCare:
                case DebugSource.DebugSourceApi:
                case DebugSource.DebugSourceWindowSystem:
                case DebugSource.DebugSourceShaderCompiler:
                case DebugSource.DebugSourceThirdParty:
                case DebugSource.DebugSourceOther:
                default:
                    showMessage = true;
                    break;
            }

            if (showMessage)
            {
                switch (severity)
                {
                    case DebugSeverity.DontCare:
                        Console.WriteLine($"[DontCare] [{source}] {message}");
                        break;
                    case DebugSeverity.DebugSeverityNotification:
                        //Logger?.LogDebug($"[{source}] {message}");
                        break;
                    case DebugSeverity.DebugSeverityHigh:
                        Console.Error.WriteLine($"Error: [{source}] {message}");
                        break;
                    case DebugSeverity.DebugSeverityMedium:
                        Console.WriteLine($"Warning: [{source}] {message}");
                        break;
                    case DebugSeverity.DebugSeverityLow:
                        Console.WriteLine($"Info: [{source}] {message}");
                        break;
                    default:
                        Console.WriteLine($"[default] [{source}] {message}");
                        break;
                }
            }
        }


    }
}
