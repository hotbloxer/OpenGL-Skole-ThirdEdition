using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects.Scenes
{
    public class SunDisplayScene : Scene
    {
        ShaderElementBase position = new PositionVertexShader();
        ShaderElementBase shitShow = new ShitShowVertexShader();
        ShaderElementBase heightMat = new HeightMapVertexShader();
        ShaderElementBase endVertex = new VertexShaderEnd();
        ShaderElementBase usingTexture = new FragmentShaderElementUsingMaterial();

        Texture _albedo      = new Texture(@"..\..\..\Textures\TextureImages\mountain_albedomap.tga", "albedomountain");
        Texture _lightMap = new Texture(@"..\..\..\Textures\TextureImages\LightmapTest.tga", "lightMap2");
        Texture _specularMap = new Texture(@"..\..\..\Textures\TextureImages\brickLight.tga", "specular2");
        Texture _normalMap   = new Texture(@"..\..\..\Textures\TextureImages\mountain_normalmap.tga", "normalMountain");



        public SunDisplayScene()
        {

        }

        public override void Load()
        {
            Plane terrain = new Plane();
            Shader terrainShader = new Shader([position, shitShow, heightMat, usingTexture, endVertex,]);

            Figure mountain = new([terrainShader], terrain, terrain);
            mountain.Material.Albedo = _albedo;
            mountain.Material.LightMap = _lightMap;
            mountain.Material.SpecularMap = _specularMap;
            mountain.Material.NormalTexture = _normalMap;
        }

        public override void OnRenderFrame()
        {
          
        }

        public override void OpenScene()
        {
            Load();
        }
    }
}
