using ImGuiNET;
using openGL2.Objects;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class HeightMapVertexShader : ShaderElementBase, IHaveUI
    {
     
        float _heightOfMap = 200f;
        public float Height { get => _heightOfMap; }

        public ImageInformation ImageInfo { get; }
        Uniform2DSamplerElement uniformForImage;

        public HeightMapVertexShader() : base(ShaderType.VertexShader)
        {
            Apply = true;

            ShaderCode = GetCode();

            Texture t = new Texture(@"..\..\..\Textures\TextureImages\mountain_heightmap.tga", "heightMapMountain");
            uniformForImage = new Uniform2DSamplerElement("heightMap", 4, t);
            ImageInfo = uniformForImage.Texture.ImageInfo;
            AddUniform(uniformForImage); 
            
        }

        public void GetUI()
        {
            if (ImGui.Checkbox("Use Height Map", ref Apply)) {}
            if (Apply)
            {
                if (ImGui.SliderFloat("Height", ref _heightOfMap, 0, 200))
                {
                    ShaderCode = GetCode();
                }
            }
        }

        private string GetCode ()
        {
            return @$"       
                vec4 texel;
                texel = texture(heightMap, uv);
                {PositionVertexShader.Position}.y += texel.z / 255 * {(int)_heightOfMap};
                ";
        }
    }
}
