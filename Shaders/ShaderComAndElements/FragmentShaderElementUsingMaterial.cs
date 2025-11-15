using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    internal class FragmentShaderElementUsingMaterial : ShaderElementBase, IUseMaterial
    {
        public FragmentShaderElementUsingMaterial() : base(ShaderType.FragmentShader)
        {
        }
    }

    public interface IUseMaterial
    {

    }
}
