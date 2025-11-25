using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class ColorRenderFrag : ShaderElementBase
    {

        


        public ColorRenderFrag() : base(ShaderType.FragmentShader)
        {

            layouts.Add(new CustomLayout("in vec3 vertexColor; \n"));

            layouts.Add(new CustomLayout("out vec4 FragColor; \n"));

            ShaderCode = @"
            FragColor = vec4(vertexColor, 1.0);

            ";


        }
    }
}
