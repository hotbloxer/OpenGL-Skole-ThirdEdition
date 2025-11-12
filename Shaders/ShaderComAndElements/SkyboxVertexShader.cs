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
    public class SkyboxVertexShader : ShaderElementBase
    {

        public SkyboxVertexShader() : base(ShaderType.VertexShader)
        {
         
            Apply = true;

            ShaderCode = GetCode();
            SkyBoxTexture t = new SkyBoxTexture("skybox", new string[0]);   
            AddUniform(new UniformCubeMapSamplerElement("skybox", 0, t));



        }


        private string GetCode ()
        {
            return @$"
                #version 330 core

                out vec3 TexCoords;

                uniform mat4 projection;
                uniform mat4 view;

       
                    TexCoords = {PositionVertexShader.Position}
                    gl_Position = projection * view * vec4(aPos, 1.0);
            ";
        }
    }
}
