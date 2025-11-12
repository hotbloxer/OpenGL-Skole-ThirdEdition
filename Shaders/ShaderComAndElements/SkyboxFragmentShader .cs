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
    public class SkyboxFragmentShader : ShaderElementBase
    {

        UniformCubeMapSamplerElement skybox;
        public static string skyboxTextureVec4 = "skyboxTexture";

        public SkyboxFragmentShader() : base(ShaderType.VertexShader)
        {
            SkyBoxTexture t = new SkyBoxTexture("skybox", new string[0]);
            skybox = new UniformCubeMapSamplerElement("skybox", 0, t);


            Apply = true;

            ShaderCode = GetCode();
            
            AddUniform(skybox);

        }


        private string GetCode ()
        {
            return @"
            #version 330 core
            out vec4 FragColor;

            in vec3 TexCoords;

            void main()
            {    
                FragColor = texture(skybox, -TexCoords);
            }
        ";
        }
    }
}
