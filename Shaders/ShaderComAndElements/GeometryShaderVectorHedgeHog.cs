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
    public class GeometryShaderVectorHedgeHog : ShaderElementBase, IHaveUI
    {

        UniformFloatElement value;
        float normSize = 0.3f;
        public GeometryShaderVectorHedgeHog() : base(ShaderType.GeometryShader)
        {


            Apply = true;
            layouts.Add(new LayoutTrianglesIn());
            layouts.Add(new LayoutTrianglesOut(LayoutTrianglesOut.GeometryShaderOutput.LINE_STRIP, 6));

            layouts.Add(
                new CustomLayout(
                    @" 
                        in vec3 normal[]; 
                        out vec3 geoNormal;"
                    ));

            value = new UniformFloatElement("normSize", normSize);

            uniforms.Add("normSize", value);

            functions.Add(
                @$"
                void GenerateLine(int i)
                {{  
                    gl_Position = gl_in[i].gl_Position; 
                    geoNormal = normal[i];
                    EmitVertex();

                    gl_Position = (gl_in[i].gl_Position + vec4(normal[i], 0.0) * normSize); 
                    geoNormal = normal[i];
                    EmitVertex();

                    EndPrimitive(); }}
                "
            );

            ShaderCode = @$"
                GenerateLine(0);
                GenerateLine(1); 
                GenerateLine(2);
            ";


        }

        public bool GetUI()
        {
            if (ImGui.SliderFloat("Normal size", ref normSize, 0, 5) )
            {
                value.Value = normSize;
                return true;
            }
            return false;
   
        }
    }
}
