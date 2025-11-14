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
    public class ShitShowVertexShaderCopy : ShaderElementBase
    {
        /// <summary>
        /// The value used to call position in script
        /// since aPosition is immutable
        /// </summary>
   
     
        public ShitShowVertexShaderCopy() : base(ShaderType.VertexShader)
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
    

                out vec4 viewPosition;

            "));



            ShaderCode = @$"       
     
                viewPosition = projection * view * vec4({PositionVertexShader.Position}, 1.0);

               
                mat4 defaultUniformIssues = modelView;




                vertexNormal =  aNormal;
                

                mat4 test = view;
                mat4 test2 = model;
                mat4 test3 = projection;
            ";
        }

    }
}
