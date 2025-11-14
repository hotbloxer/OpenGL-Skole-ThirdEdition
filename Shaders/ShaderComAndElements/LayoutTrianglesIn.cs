using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class LayoutTrianglesIn : LayoutBase
    {
        public LayoutTrianglesIn() : base()
        {
        }

        public override string GetLayoutString()
        {
            return @$"layout (triangles) in; 
";
        }
    }
}
