
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public abstract class ShaderElementBase
    {
        public Dictionary<string, LayoutBase> layouts = new(); 
        public Dictionary<string, UniformElement> uniforms = new();
        public string Uniforms { get => UniformToString(uniforms); }
        public string Layouts { get => LayoutToString(layouts); }
        public string ShaderCode = "";
        public readonly ShaderType ShaderType;
        public bool Apply;

        public ShaderElementBase(ShaderType shaderType)
        {
            ShaderType = shaderType;
        }

        public void AddUniform (UniformElement uniform)
        {
            uniforms.Add(uniform.Name, uniform);
        }

        public void AddLayout (LayoutBase layout)
        {
            layouts.Add(layout.Name, layout);
        }


        public void SetUniforms(int shaderHandle)
        {
            foreach (UniformElement element in uniforms.Values)
            {
                element.SetUniform(shaderHandle);
            }
        }

        private string UniformToString(Dictionary<string, UniformElement> uniformDic)
        {
            if (uniformDic.Count < 1) return "";

            StringBuilder sb = new StringBuilder();
            foreach (UniformElement element in uniformDic.Values)
            {
                sb.Append("uniform ");
                sb.Append(element.UniformType + " ");
                sb.Append(element.Name + ";\n");
            }
            return sb.ToString();
        }

        private string LayoutToString(Dictionary<string, LayoutBase> layoutDic)
        {
            if (layoutDic.Count < 1) return "";

            StringBuilder sb = new StringBuilder();
            foreach (LayoutBase layout in layoutDic.Values)
            {
                sb.Append(layout.GetLayoutString());
          
            }
            return sb.ToString();
        }
    }
}
