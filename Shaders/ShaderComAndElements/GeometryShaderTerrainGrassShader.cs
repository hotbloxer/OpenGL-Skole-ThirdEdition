using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    internal class GeometryShaderTerrainGrassShader : ShaderElementBase
    {
        public GeometryShaderTerrainGrassShader() : base(ShaderType.GeometryShader)
        {
            Apply = true;
            layouts.Add(new LayoutTrianglesIn());
            layouts.Add(new LayoutTrianglesOut(LayoutTrianglesOut.GeometryShaderOutput.TRIANGLE_STRIP, 10));

            layouts.Add(
                new CustomLayout(
                    @" 
                        in mat4 pvm[];
                        in vec3 normal[]; 
                        out vec2 uv;
                    "
                    ));


            functions.Add(@$"
                void GenerateGrass()
                {{  
                      
                        vec4 x = vec4(0.1,0,0,0);
                        vec4 y = vec4(0,0.1,0,0);
                        vec4 z = vec4(0,0,0.1,0);

                        mat4 pwmInverse = inverse(pvm[0]);
                        vec4 pos = pwmInverse * gl_in[0].gl_Position;
                        

                        gl_Position = pvm[0] * (pos + x); 
                        uv = vec2(0,0);
                        EmitVertex();

                        gl_Position = pvm[0] * (pos + z); 
                        uv = vec2(1,0);
                        EmitVertex();

                        gl_Position = pvm[0] * (pos + (x + y)); 
                        uv = vec2(1,1);
                        EmitVertex();

                        gl_Position = pvm[0] * (pos + (z + y)); 
                        uv = vec2(0,1);
                        EmitVertex();

                        EndPrimitive(); 

                }}
                "
            );

            ShaderCode = @$"
                     GenerateGrass();
                
                
            ;";



        }
    }
}
