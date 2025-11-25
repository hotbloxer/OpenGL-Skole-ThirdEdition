using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class FragmentShaderSun : ShaderElementBase
    {
        UniformVec3Element lightPosElementUniform;
        UniformVec3Element lightColor;


        public FragmentShaderSun( ) : base(ShaderType.FragmentShader)
        {

            Sun.SunChangeEvent += UpdateLightDir;
            lightPosElementUniform = new UniformVec3Element(("lightPositionIn"), Sun.Instance.Position);
            lightColor = new UniformVec3Element(("sunLight"), Sun.Instance.LightColor);


            uniforms.Add("sunUniform", lightPosElementUniform);
            uniforms.Add("sunLight", lightColor);


        }



        public void UpdateLightDir (Vector3 dir, Vector3 lightColor)
        {
            lightPosElementUniform.UpdateValue(dir);
            this.lightColor.UpdateValue(lightColor);
        }
        

    }
}
