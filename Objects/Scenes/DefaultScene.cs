using ImGuiNET;
using openGL2.Objects;
using openGL2.Objects.Scenes;
using openGL2.Objects.Terrain;
using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2
{
    public  class DefaultScene : Scene
    {

        private Figure mainFigure;
        private Figure tree;
        private Figure leaves;


        private Plane plane;

        private Shader shader;
        private Shader backgroundShader;


        Texture _albedo;
        Texture _lightMap;
        Texture _normalMap;
        Texture _specularMap;

        UI ui;


        MoveHeightToHeightMap heightMover;


        ShaderElementBase position;
        ShaderElementBase shitShow;
        ShaderElementBase cellShader;
        ShaderElementBase heightMat;

        ShaderElementBase simpleColor;



        public override void Load()
        {

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

            ShaderElementBase grassFrag = new ShaderElementFragmentGrassMaker();
            ShaderElementBase GrassShader = new GeometryShaderTerrainGrassShader();


            shader = new Shader([position, shitShow, usingTexture, cellShader, heightMat, wire, subdivide, vertexForVectorGeo, geometryHedgeHog, endVertex]);

            //backgroundShader = new Shader([position, shitShow, heightMat, overrideFragmentShader, simpleColor, vertexForVectorGeo, endVertex]);

            Shader grassShader = new Shader([position, overrideFragmentShader, grassFrag, shitShow, heightMat, vertexForVectorGeo, endVertex, GrassShader, geometryHedgeHog]);



            plane = new Plane();
            Cube kasse = new Cube();


            mainFigure = new([shader, grassShader], plane, plane);

            // render terrain
            mainFigure.Render = true;





            //REFA de her skal ligge på objecktet i stedet for her


            GL.Enable(EnableCap.DepthTest);

            // load all default textures
            new Texture(@"..\..\..\Textures\TextureImages\rock.tga", "rock");
            new Texture(@"..\..\..\Textures\TextureImages\Rainbow.tga", "rain");

            Texture Chekered = GeneratedTextures.GetGeneratedTexture(GeneratedTextures.GeneratedTexures.CHECKERED);
            new Texture(@"..\..\..\Textures\TextureImages\bulletNormal.tga", "bullet");
            new Texture(@"..\..\..\Textures\TextureImages\testTex.tga", "testTex");
            Texture normalBric = new Texture(@"..\..\..\Textures\TextureImages\brickNormal.tga", "normalBrick");
            Texture albedoBric = new Texture(@"..\..\..\Textures\TextureImages\brickAlbedo.tga", "albedo");
            new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap");
            Texture lightBric = new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular");
            new Texture(@"..\..\..\Textures\TextureImages\teapot_normalmap.tga", "normal");
            Texture HeightMap = new Texture(@"..\..\..\Textures\TextureImages\teapot_displacementmap.tga", "heightMap");

            _albedo = new Texture(@"..\..\..\Textures\TextureImages\mountain_albedomap.tga", "albedomountain");
            _lightMap = new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap2");
            _specularMap = new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular2");
            _normalMap = new Texture(@"..\..\..\Textures\TextureImages\mountain_normalmap.tga", "normalMountain");


            mainFigure.Material.Albedo = _albedo;
            mainFigure.Material.LightMap = _lightMap;
            mainFigure.Material.SpecularMap = _specularMap;
            mainFigure.Material.NormalTexture = _normalMap;







            kasse = new Cube();
            //Figure one = new Figure(noHeightMap, kasse);
            //Figure two = new Figure(noHeightMap, kasse);

            //float[] uvPlacements = [0.5f,0.5f, 0.75f, 0.75f];
            //one.TranslateFigure(Matrix4.CreateScale(0.01f));
            //one.TranslateFigure(Matrix4.CreateTranslation(new Vector3(uvPlacements[0], 0, uvPlacements[1])));

            //two.TranslateFigure(Matrix4.CreateScale(0.01f));
            //two.TranslateFigure(Matrix4.CreateTranslation(new Vector3(uvPlacements[2], 0, uvPlacements[3])));



            //Material brickMat = new Material();
            //brickMat.NormalTexture = normalBric;
            //brickMat.SpecularMap = lightBric;
            //brickMat.Albedo = albedoBric;

            //Shader objShader = new Shader([position, usingTexture, shitShow, endVertex]);

            //// simple obj eksempel
            //VertexInformation simpleCube = OBJParser.LoadOBJ("../../../Objects/OBJfiler/CompanionCube.obj");
            //VertexInformation funkyCube = OBJParser.LoadOBJ("../../../Objects/OBJfiler/AdvancedCube.obj");



            //Figure objEksempel = new Figure([objShader], simpleCube, true);
            //objEksempel.Material = brickMat;
            //objEksempel.Render = true;



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

                bool renderTree = false;
                tree = new Figure([noHeightMap], treeVi);
                leaves = new Figure([noHeightMap], leafsVi);



                Texture barkAlbedo = new Texture(@"../../../Textures/TextureImages/M_Bark.001_baseColor.tga", "bark color");
                Texture barkNormal = new Texture(@"../../../Textures/TextureImages/M_Bark.001_normal.tga", "bark normal");
                Texture leavsAlbedo = new Texture(@"../../../Textures/TextureImages/M_Branch.001_baseColor.tga", "leavs color");
                Texture bleavesNormal = new Texture(@"../../../Textures/TextureImages/M_Branch.001_normal.tga", "leaves normal");

                tree.Material.Albedo = barkAlbedo;
                tree.Material.NormalTexture = barkNormal;
                leaves.Material.Albedo = leavsAlbedo;
                // leaves.Material.NormalTexture = bleavesNormal;

                tree.Render = renderTree;
                leaves.Render = renderTree;

                if (true)
                {
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
            }







        }


        public override void OnRenderFrame()
        {
            heightMover.MoveFiguresToHeight();
        }

        public override void OpenScene()
        {
            Load();
        }
    }
}
