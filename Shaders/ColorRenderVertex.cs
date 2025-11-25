using openGL2.Shaders.ShaderComAndElements;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders
{
    public class ColorRenderVertex : ShaderElementBase
    {
        public ColorRenderVertex() : base(ShaderType.VertexShader)
        {
            layouts.Add(new CustomLayout(@"
            layout(location = 5) in vec3 aColor;
            out vec3 vertexColor;
            "));

            ShaderCode = "vertexColor = aColor;";
            

        }
    }
}
