using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using System.Xml.Linq;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class LayoutVec3 : LayoutBase
    {
        string layoutString;
        public LayoutVec3(string name, int location) : base()
        {
            layoutString = $"layout(location = {location}) in vec3 {name}; \n";
        }

        public override string GetLayoutString()
        {
            return layoutString;
        }  

        

    }
}
