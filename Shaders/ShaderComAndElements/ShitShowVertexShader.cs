using ImGuiNET;
using openGL2.Objects;
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
    public class ShitShowVertexShader : ShaderElementBase
    {
        /// <summary>
        /// The value used to call position in script
        /// since aPosition is immutable
        /// </summary>
   
     
        public ShitShowVertexShader() : base(ShaderType.VertexShader)
        {
            Apply = true;
            
            ShaderCode = @$"       
                fragPosition = vec3(model * vec4({PositionVertexShader.Position}, 1.0));
                uv = aUV;

                viewPosition = projection * view * vec4({PositionVertexShader.Position}, 1.0);

                mat4 modelview2 =   view * model;
                mat3 normalMatrix = transpose(inverse(mat3( modelview2)));
                mat4 modelViewInstance = modelView;


                tbn = mat3
                  (
                     normalize(vec3(model * vec4(aTangent , 0))),
                     normalize(vec3(model * vec4(aBiNormal, 0))),
                     normalize(vec3(model * vec4(aNormal  , 0)))
                  );


                vertexNormal =  aNormal;
                

                mat4 test = view;
                mat4 test2 = model;
                mat4 test3 = projection;
            ";
        }

    }
}
