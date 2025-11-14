using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class VertexShaderEnd : ShaderElementBase
    {
        public VertexShaderEnd() : base(ShaderType.VertexShader)
        {
            Apply = true;
            ShaderCode = $@"
                gl_Position = projectionViewModel * vec4({PositionVertexShader.Position}, 1.0) ;";


        }
    }
}
