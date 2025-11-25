using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Tools;
using openGL2.Window;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects.Scenes
{
    public class EditVerticesScene : Scene
    {

        VetexSelector vertexSelector;
        Figure tree;
        //Figure leaves;
        Camera cam;
        private static MouseState mouse;


     

        public EditVerticesScene(Camera cam)
        {
            this.cam = cam;
           
        }

        public override void Load()
        {
            ShaderElementBase pos = new PositionVertexShader();
            ShaderElementBase shitShowVertex = new ShitShowVertexShader();
            ShaderElementBase endVertex = new VertexShaderEnd();
            ShaderElementBase overrideFragmentShader = new FragmentShaderOverrider();
            ShaderElementBase simpelFrag = new SimpleColorFragmentShader();
            ShaderElementBase colorRender = new ColorRenderFrag();
            ShaderElementBase colorRenderVert = new ColorRenderVertex();
            ShaderElementBase geo = new GeometryShaderWire();


            Shader shaderProgram = new([pos, shitShowVertex, colorRenderVert, endVertex, geo, overrideFragmentShader, colorRender]);

            VertexInformation treeVi = OBJParser.LoadOBJ("../../../Objects/OBJfiler/spruce_tree_trunk.obj");
            //VertexInformation leafsVi = OBJParser.LoadOBJ("../../../Objects/OBJfiler/spruce_tree_branches.obj");

            bool renderTree = false;
            tree = new Figure([shaderProgram], treeVi);
            //leaves = new Figure([shaderProgram], leafsVi);

            vertexSelector = new();

        }




     
        int[] currentSelection = new int [4];
        public override void OnRenderFrame()
        {
            mouse = Window.Window.mouse;

            if (mouse == null) return;
            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                currentSelection[0] = (int)mouse.Position.X;
                currentSelection[1] = (int)mouse.Position.Y;
            }
                
            if (mouse.IsButtonReleased(MouseButton.Left))
            {
                currentSelection[2] = (int)mouse.Position.X;
                currentSelection[3] = (int)mouse.Position.Y;
                ChangeColors(currentSelection[0], currentSelection[1], currentSelection[2], currentSelection[3]);
            }
        }

        public override void OpenScene()
        {
            Load();
            

        }



        private void ChangeColors(int startx, int starty, int endx, int endy)
        {
            Matrix4 pmv = tree.ModelSpace * cam.View * cam.Projection;

            float[] posInModel = tree._vertexInformation.Positions;
            float[] colorsInModels = tree._vertexInformation.Colors;
            for (int i = 0; i < posInModel.Length; i += 3)
            {
                int[] test = vertexSelector.ProjectVertexToScreen(
                    [0, 0, 1600, 900],
                    [posInModel[i], posInModel[i + 1], posInModel[i + 2]],
                    pmv);



                if (test[0] > startx && test[0] < endx &&
                    test[1] > starty && test[1] < endy
                    )
                {
                    colorsInModels[i] = 0.1f;
                    colorsInModels[i + 1] = 0.1f;
                    colorsInModels[i + 2] = 0.1f;
                }
            }

            tree._vertexInformation.Colors = colorsInModels;
            tree.UpdateVBO();

        }


    }
}
