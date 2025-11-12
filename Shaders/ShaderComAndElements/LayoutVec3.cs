using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class LayoutVec3 : LayoutBase
    {
        public LayoutVec3(string name, int location) : base(name, "vec3", location)
        {
        }
    }
}
