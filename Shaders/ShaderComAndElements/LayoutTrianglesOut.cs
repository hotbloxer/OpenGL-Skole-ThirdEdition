using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class LayoutTrianglesOut : LayoutBase
    {
        string returnString;
        public enum GeometryShaderOutput { LINE_STRIP, TRIANGLE_STRIP}
        public LayoutTrianglesOut(GeometryShaderOutput type, int maxVertices) : base()
        {
            string typeInsert;
            switch (type)
            {
                case GeometryShaderOutput.LINE_STRIP:
                    typeInsert = "line_strip";
                    break;

                case GeometryShaderOutput.TRIANGLE_STRIP:
                    typeInsert = "triangle_strip";
                    break;
                default:
                    typeInsert = "line_strip";
                    break;
            }

            returnString = @$"layout ({typeInsert}, max_vertices = {maxVertices}) out;";
        }

        public override string GetLayoutString()
        {
            return returnString;
        }
    }
}
