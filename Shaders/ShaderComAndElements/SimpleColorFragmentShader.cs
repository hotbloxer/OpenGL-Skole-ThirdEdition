using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    internal class SimpleColorFragmentShader : ShaderElementBase, IHaveUI
    {
        Vector4 fragColor = new Vector4(1, 1, 1 ,1);

        public SimpleColorFragmentShader() : base(ShaderType.FragmentShader)
        {
            Apply = true;
            layouts.Add(new CustomLayout("in vec3 geoNormal; \n"));
            layouts.Add(new CustomLayout("out vec4 FragColor; \n"));


            ShaderCode = $@"
                FragColor = vec4({fragColor.X},{fragColor.Y},{fragColor.Z},{fragColor.W} ); 
        
            ";

        }

        public void GetUI()
        {
            if (ImGui.ColorPicker4("Frag color", ref fragColor))
            {
                ShaderCode = $@"
                FragColor = vec4(
                    {fragColor.X.ToString(CultureInfo.InvariantCulture)},
                    {fragColor.Y.ToString(CultureInfo.InvariantCulture)},
                    {fragColor.Z.ToString(CultureInfo.InvariantCulture)},
                    {fragColor.W.ToString(CultureInfo.InvariantCulture)}
                );
                ";


            }
        }
    }
}
