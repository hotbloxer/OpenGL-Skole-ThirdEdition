using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class VertexShaderForGeometryVectorDirection : ShaderElementBase
    {
        
        public VertexShaderForGeometryVectorDirection() : base(ShaderType.VertexShader)
        {
            Apply = true;

            layouts.Clear();

            layouts.Add(new CustomLayout("out vec3 normalForGeoDir; \n"));

            //TODO spørg søren om hvad den her skal gøre?
            ShaderCode = $@"   
                //mat3 nMat = mat3(transpose(inverse(projection * modelView)));
                //normal = normalize(vec3(vec4(nMat * aNormal, 0.0)));
                  normalForGeoDir =  aNormal;
                ";


        }
    }
}
