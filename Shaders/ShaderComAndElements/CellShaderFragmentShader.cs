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
    public class CellShaderFragmentShader : ShaderElementBase, IHaveUI 
    {
        public CellShaderFragmentShader() :base(ShaderType.FragmentShader)
        {
            Apply = false;

            ShaderCode = @"       
                normal = vertexNormal;
                vec4 color;
                float intensity = dot(lightDir, normal);
                if (intensity > 0.95)
                color = vec4(1.0,0.5,0.5,1.0);
                else if (intensity > 0.5)
                color = vec4(0.6,0.3,0.3,1.0);
                else
                if (intensity > 0.25)
                color = vec4(0.4,0.2,0.2,1.0);
                else
                color = vec4(0.2,0.1,0.1,1.0);
                FragColor = color;";
        }

        public bool GetUI()
        {
            if (ImGui.Checkbox("Use Cell Shading", ref Apply)) { return true; }
            return false;

        }
    }
}
