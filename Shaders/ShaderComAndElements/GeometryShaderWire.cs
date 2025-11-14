using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static openGL2.Shaders.ShaderComAndElements.LayoutTrianglesOut;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class GeometryShaderWire : ShaderElementBase
    {
      
        public GeometryShaderWire() : base(ShaderType.GeometryShader)
        {

            Apply = true;
            ShaderCode = GetCode();

            AddLayout(new LayoutTrianglesIn());
            AddLayout(new LayoutTrianglesOut(GeometryShaderOutput.LINE_STRIP, 5));


        }

        private string GetCode()
        {
            return @$"       
                // geometryshader
                gl_Position = gl_in[0].gl_Position; 
                EmitVertex(); 

                gl_Position = gl_in[1].gl_Position; 
                EmitVertex();
            ";
        }
    }
}
