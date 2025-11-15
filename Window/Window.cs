
using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using openGL2.Objects;
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
    public class Window : GameWindow
    {
        private  Figure mainFigure;
        private Figure tree;
        private Figure leaves;


        private Plane plane;

        private Shader shader;
        private Shader porkipineShader;
        private readonly Camera camera;

        Texture _albedo;
        Texture _lightMap;
        Texture _normalMap;
        Texture _specularMap;

        UI ui;

        SkyBoxShader skyBoxShader;
        MoveHeightToHeightMap heightMover;


        ShaderElementBase position;
        ShaderElementBase shitShow; 
        ShaderElementBase cellShader;        
        ShaderElementBase heightMat;

        ShaderElementBase simpleColor;



        // der er en dedikeret VAO til de forskellige VAO man får brug for

        public Window() :
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new Vector2i(1600, 900), APIVersion = new Version(3, 3) })
        {

            camera = new Camera(this);
            camera.Position = new Vector3(0.0f, 1.0f, 1.0f);
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            ImGui.CreateContext();
            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;


            position = new PositionVertexShader(); 
            shitShow = new ShitShowVertexShader();
            cellShader = new CellShaderFragmentShader();
            heightMat = new HeightMapVertexShader();
            simpleColor = new SimpleColorFragmentShader();

            ShaderElementBase wire = new GeometryShaderWire();
            ShaderElementBase subdivide = new GeometryShaderSubdivide();


            ShaderElementBase vertexForVectorGeo = new VertexShaderForGeometryVectorDirection();
            ShaderElementBase geometryHedgeHog = new GeometryShaderVectorHedgeHog();

            ShaderElementBase endVertex = new VertexShaderEnd();

            ShaderElementBase overrideFragmentShader = new FragmentShaderOverrider();

            ShaderElementBase usingTexture = new FragmentShaderElementUsingMaterial();

            ShaderElementBase GrassShader = new GeometryShaderTerrainGrassShader();


            shader = new Shader([position, shitShow, usingTexture, cellShader, heightMat, wire, subdivide, endVertex]);

            //porkipineShader = new Shader([position, shitShow, heightMat, overrideFragmentShader, simpleColor, vertexForVectorGeo, geometryHedgeHog,  endVertex, GrassShader]);
            
            Shader grassShader = new Shader([position, /*overrideFragmentShader, simpleColor,*/ usingTexture, shitShow, heightMat,  vertexForVectorGeo,  endVertex, GrassShader, geometryHedgeHog]);



            plane = new Plane();
            Cube kasse = new Cube();


            mainFigure = new([shader, grassShader], plane, plane);
          
            mainFigure.Render = true;

            ImGui.StyleColorsDark();

            ImGuiStylePtr style = ImGui.GetStyle();
            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                style.WindowRounding = 0.0f;
                style.Colors[(int)ImGuiCol.WindowBg].W = 1.0f;
            }


            GL.ClearColor(new Color4(0.3f, 0.4f, 0.2f, 1));

            

            //REFA de her skal ligge på objecktet i stedet for her
      

            GL.Enable(EnableCap.DepthTest);

            // load all default textures
            new Texture(@"..\..\..\Textures\TextureImages\rock.tga", "rock");
            new Texture(@"..\..\..\Textures\TextureImages\Rainbow.tga", "rain");
        
            Texture Chekered = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);
            new Texture(@"..\..\..\Textures\TextureImages\bulletNormal.tga", "bullet");
            new Texture(@"..\..\..\Textures\TextureImages\testTex.tga", "testTex");
            new Texture(@"..\..\..\Textures\TextureImages\brickNormal.tga", "normalBrick");
            new Texture(@"..\..\..\Textures\TextureImages\brickAlbedo.tga", "albedo");
            new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap");
            new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular");
            new Texture(@"..\..\..\Textures\TextureImages\teapot_normalmap.tga", "normal");
            Texture HeightMap= new Texture(@"..\..\..\Textures\TextureImages\teapot_displacementmap.tga", "heightMap");

            _albedo =      new Texture(@"..\..\..\Textures\TextureImages\mountain_albedomap.tga", "albedomountain");
            _lightMap =    new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap2");
            _specularMap = new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular2");
            _normalMap =   new Texture(@"..\..\..\Textures\TextureImages\mountain_normalmap.tga", "normalMountain");


            mainFigure.Material.Albedo = _albedo;
            mainFigure.Material.LightMap = _lightMap;
            mainFigure.Material.SpecularMap = _specularMap;
            mainFigure.Material.NormalTexture = _normalMap;


            skyBoxShader = new();


            
            




            kasse = new Cube();
            //Figure one = new Figure(noHeightMap, kasse);
            //Figure two = new Figure(noHeightMap, kasse);

            //float[] uvPlacements = [0.5f,0.5f, 0.75f, 0.75f];
            //one.TranslateFigure(Matrix4.CreateScale(0.01f));
            //one.TranslateFigure(Matrix4.CreateTranslation(new Vector3(uvPlacements[0], 0, uvPlacements[1])));

            //two.TranslateFigure(Matrix4.CreateScale(0.01f));
            //two.TranslateFigure(Matrix4.CreateTranslation(new Vector3(uvPlacements[2], 0, uvPlacements[3])));







            





            //VertexInformation combinedTree = new VertexInformation([tree, leafs]);
            heightMover = new((HeightMapVertexShader)heightMat);
            if (false)
            {
                ShaderElementBase[] shaderElements = [
                new PositionVertexShader(),
                new ShitShowVertexShader(),
                new FragmentShaderElementUsingMaterial(),
                new VertexShaderEnd()

            ];
                Shader noHeightMap = new Shader(shaderElements);
                VertexInformation treeVi = OBJParser.LoadOBJ("../../../Objects/OBJfiler/spruce_tree_trunk.obj");
                VertexInformation leafsVi = OBJParser.LoadOBJ("../../../Objects/OBJfiler/spruce_tree_branches.obj");

                bool renderTree = true;
                tree = new Figure([noHeightMap], treeVi);
                leaves = new Figure([noHeightMap], leafsVi);

                tree.Render = renderTree;
                leaves.Render = renderTree;

                Texture barkAlbedo = new Texture(@"../../../Textures/TextureImages/M_Bark.001_baseColor.tga", "bark color");
                Texture barkNormal = new Texture(@"../../../Textures/TextureImages/M_Bark.001_normal.tga", "bark normal");
                Texture leavsAlbedo = new Texture(@"../../../Textures/TextureImages/M_Branch.001_baseColor.tga", "leavs color");
                Texture bleavesNormal = new Texture(@"../../../Textures/TextureImages/M_Branch.001_normal.tga", "leaves normal");

                tree.Material.Albedo = barkAlbedo;
                tree.Material.NormalTexture = barkNormal;
                leaves.Material.Albedo = leavsAlbedo;
                // leaves.Material.NormalTexture = bleavesNormal;

                tree.Render = false;
                leaves.Render = false;


                int repeats = 10;
                Figure[] newtrees = new Figure[repeats * repeats];
                Figure[] newBranches = new Figure[repeats * repeats];



                float[] newUvs = new float[repeats * repeats * 2];

                float uvSpread = (float)1 / repeats;
                for (int i = 0; i < repeats; i++)
                {
                    for (int j = 0; j < repeats; j++)
                    {
                        int Index = i * repeats + j;
                        int uvIndex = Index * 2;

                        newUvs[uvIndex] = i * uvSpread;
                        newUvs[uvIndex + 1] = j * uvSpread;

                        newtrees[Index] = tree.GetDublicate();
                        newBranches[Index] = leaves.GetDublicate();
                        newtrees[Index].Render = true;
                        newBranches[Index].Render = true;

                        newtrees[Index].TranslateFigure(Matrix4.CreateScale(0.005f));
                        newtrees[Index].TranslateFigure(Matrix4.CreateTranslation(new Vector3(newUvs[uvIndex], 0, newUvs[uvIndex + 1])));

                        newBranches[Index].TranslateFigure(Matrix4.CreateScale(0.005f));
                        newBranches[Index].TranslateFigure(Matrix4.CreateTranslation(new Vector3(newUvs[uvIndex], 0, newUvs[uvIndex + 1])));
                       
                    }
                }

                heightMover.AddFiguresAndUVs(newtrees, newUvs);
                heightMover.AddFiguresAndUVs(newBranches, newUvs);
            }



            camera.UpdateView();
            ui = new();
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


            heightMover.MoveFiguresToHeight();

            ImguiImplOpenGL3.NewFrame();
            ImguiImplOpenTK4.NewFrame();
            ImGui.NewFrame();

            ImGui.DockSpaceOverViewport();

            ui.Ui(); // ui elements
            ui.RenderView(); // view of render. actually just a regular window... but transparanet
       

            ImGui.Render();
            GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
        

            ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

            if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
            {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
                Context.MakeCurrent();
            }

           
            KeyboardState input;
            MouseState mouse;
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
            GL.DeleteProgram(shader.ShaderProgramHandle);

            base.OnUnload();
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
