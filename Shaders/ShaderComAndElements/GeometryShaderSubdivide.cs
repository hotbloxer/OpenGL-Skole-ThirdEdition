using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class GeometryShaderSubdivide : ShaderElementBase
    {

    

        public GeometryShaderSubdivide() : base(ShaderType.GeometryShader)
        {

            Apply = true;
            ShaderCode = GetCode();


            functions.Add(
                @"
                void GenerateSubTriangle(vec4 a, vec4 b, vec4 c)
                { 
                    
                    
                    gl_Position = a; 
                    EmitVertex();

                    gl_Position = b; 
                    EmitVertex();

                    gl_Position = c; 
                    EmitVertex();

                    EndPrimitive(); 
                }
                "
            );

            layouts.Add(new LayoutTrianglesIn());
            layouts.Add(new LayoutTrianglesOut(LayoutTrianglesOut.GeometryShaderOutput.LINE_STRIP, 12));

        }



        private string GetCode()
        {
            return @$"       
                vec4 a = mix(gl_in[0].gl_Position, gl_in[1].gl_Position, 0.5);
                vec4 b = mix(gl_in[1].gl_Position, gl_in[2].gl_Position, 0.5);
                vec4 c = mix(gl_in[2].gl_Position, gl_in[0].gl_Position, 0.5);

                GenerateSubTriangle(gl_in[0].gl_Position, a, c); 
                GenerateSubTriangle(a, gl_in[1].gl_Position, b); 
                GenerateSubTriangle(c, b, gl_in[2].gl_Position); 
                GenerateSubTriangle(a, b, c);


            ";
        }
    }
}
