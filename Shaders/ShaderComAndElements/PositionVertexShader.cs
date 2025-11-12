using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class PositionVertexShader : ShaderElementBase
    {
        /// <summary>
        /// The value used to call position in script
        /// since aPosition is immutable
        /// </summary>
        public static readonly string Position = "newPosition";
     
        public PositionVertexShader() : base(ShaderType.VertexShader)
        {
            Apply = true;

            AddLayout(new LayoutVec3("aPosition", 0));
            
            ShaderCode = @$"       
                vec3 {Position} = aPosition;
            ";
        }

    }
}
