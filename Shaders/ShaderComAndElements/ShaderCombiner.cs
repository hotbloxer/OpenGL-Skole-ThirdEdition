using openGL2.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class ShaderCombiner
    {
        public List<ShaderElementBase> elements = new List<ShaderElementBase>();
        private Shader _shader;

        public ShaderCombiner(Shader shader)
        {
            _shader = shader;
        }

        public void SetUniforms()
        {
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                element.SetUniforms(_shader.ShaderProgramHandle);
            }
        }

        public string GetShaderCode(OpenTK.Graphics.OpenGL4.ShaderType shaderType)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                if (element.ShaderType != shaderType) continue;
                sb.Append(element.ShaderCode);
            }
            return sb.ToString();
        }


        public string GetShaderUniforms(OpenTK.Graphics.OpenGL4.ShaderType shaderType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                if (element.ShaderType != shaderType) continue;
                sb.Append(element.Uniforms);
            }
            return sb.ToString();
        }

        public string GetLayouts ()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                sb.Append(element.Layouts);
            }
            return sb.ToString();
        }


        public void GetUI()
        {
            foreach (ShaderElementBase element in elements)
            {
                if (element is IHaveUI ui)
                {
                    ui.GetUI();
                }
            }
        }

    }
}
