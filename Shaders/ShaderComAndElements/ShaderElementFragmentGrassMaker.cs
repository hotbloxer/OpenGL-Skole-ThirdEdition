using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class ShaderElementFragmentGrassMaker : ShaderElementBase
    {
        public ShaderElementFragmentGrassMaker() : base(ShaderType.FragmentShader)
        {
            Apply = true;
            Texture t = new Texture(@"..\..\..\Textures\TextureImages\M_Branch.001_baseColor.tga", "grasss");
            uniforms.Add("grasUni", new Uniform2DSamplerElement("grass", 0, t));

            layouts.Add(new CustomLayout(
                @"in vec2 uv;
                  out vec4 FragColor;"));

            ShaderCode = @"
                vec4 pixel = vec4(texture(grass, uv).rgba);
                if (pixel.w < 0.1) discard;
                
                FragColor = pixel;
            ";


        }
    }
}
