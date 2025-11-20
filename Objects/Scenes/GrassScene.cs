using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects.Scenes
{
    internal class GrassScene : Scene
    {
        AnimatedGrass _animatedGrass = new AnimatedGrass();

        public override void OpenScene()
        {
            Load();
        }


        public override void Load()
        {
            ShaderElementBase pos = new PositionVertexShader();
            ShaderElementBase shitShowVertex = new ShitShowVertexShader();
            ShaderElementBase endVertex = new VertexShaderEnd();
            ShaderElementBase overrideFragmentShader = new FragmentShaderOverrider();
            ShaderElementBase simpelFrag = new SimpleColorFragmentShader();
            ShaderElementBase grassFrag = new ShaderElementFragmentGrassMaker();

            Shader terrainShader = new([pos, shitShowVertex, endVertex, overrideFragmentShader, simpelFrag]);
            Shader shaderProgram = new([pos, shitShowVertex, _animatedGrass, endVertex, overrideFragmentShader, grassFrag]);

            Plane terrainVInfo = new();
            GrassObject grassVInfo = new();

            Figure terrain = new([terrainShader], terrainVInfo, terrainVInfo);
            Figure grass = new([shaderProgram], grassVInfo);

            int xAmount = terrainVInfo.SubdivideWidth;
            int yAmount = terrainVInfo.SubdivideHeight;
            float width = (float) terrainVInfo.Width / xAmount;
            float height = (float) terrainVInfo.Height / yAmount;

            for (int x = 0; x < xAmount; x++)
            {
                for (int y = 0; y < yAmount; y++)
                {
                    Figure newGrass = grass.GetDublicate();
                    newGrass.TranslateFigure(Matrix4.CreateTranslation(new Vector3(width* x, 0, height* y)));
                }
            }
        }

        float _timeCounter;

        public override void OnRenderFrame()
        {
            _timeCounter += 0.0001f;
            _animatedGrass.UpdateTime(_timeCounter);

        }


    }
}
