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

            layouts.Add(new CustomLayout($@"
                 layout(location = 1) in vec2 aUV;
                 layout(location = 2) in vec3 aNormal;
                 layout(location = 3) in vec3 aTangent;  
                 layout(location = 4) in vec3 aBiNormal;

                 uniform mat4 modelView;
                 uniform mat4 model;
                 uniform mat4 view;
                 uniform mat4 projection;
                 uniform mat4 projectionViewModel;

            "));


            layouts.Add(new CustomLayout($@"
                uniform mat4 texProjection;

                out vec2 uv;
                out vec3 vertexNormal;
                out vec3 fragPosition;
                out mat3 tbn;
                out mat4 pvm;

                out vec4 viewPosition;

            "));



            ShaderCode = @$"       
                fragPosition = vec3(model * vec4({PositionVertexShader.Position}, 1.0));
                uv = aUV;


                viewPosition = projection * view * vec4({PositionVertexShader.Position}, 1.0);

                mat4 modelview2 =   view * model;
                mat3 normalMatrix = transpose(inverse(mat3( modelview2)));
                mat4 modelViewInstance = modelView;

                pvm = projectionViewModel;

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
