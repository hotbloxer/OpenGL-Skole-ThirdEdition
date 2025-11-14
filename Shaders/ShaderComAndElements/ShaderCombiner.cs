using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;

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


        public bool OverrideFragmentShader ()
        {
            foreach (ShaderElementBase element in elements)
            {
                if (element is FragmentShaderOverrider) return true;
            }
            return false;
        }

        public void SetUniforms(bool includeGeometryShader)
        {
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                if (element.ShaderType == ShaderType.GeometryShader && !includeGeometryShader) continue;

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

        public string GetLayouts (ShaderType type)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply || element.ShaderType != type) continue;
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

        

        public string GetFunctions ()
        {

            StringBuilder sb = new StringBuilder();
            foreach (ShaderElementBase element in elements)
            {
                if (!element.Apply) continue;
                if (element.functions.Count > 0)
                {
                    foreach (string function in element.functions)
                    {
                        sb.Append(function);
                    }
                    
                }
                
            }
            return sb.ToString();

        }

    }
}
