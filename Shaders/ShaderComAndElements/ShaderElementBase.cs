
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
        public List<LayoutBase> layouts = new(); 
        public Dictionary<string, UniformElement> uniforms = new();
        public List<string> functions = new();
        public string Uniforms { get => UniformToString(uniforms); }
        public string Layouts { get => LayoutToString(layouts); }
        public string ShaderCode = "";
        public readonly ShaderType ShaderType;
        public bool Apply = true;
        public readonly uint id;
        private static uint idCounter = 0;

        public ShaderElementBase(ShaderType shaderType)
        {
            id = idCounter++;
            ShaderType = shaderType;
        }

        public void AddUniform (UniformElement uniform)
        {
            uniforms.Add(uniform.Name, uniform);
        }

        public void AddLayout (LayoutBase layout)
        {
            layouts.Add(layout);
        }


        public void SetUniforms(int shaderHandle)
        {
            foreach (UniformElement element in uniforms.Values)
            {
                if (Apply)
                {
                    element.SetUniform(shaderHandle);

                }
                
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

        private string LayoutToString(List<LayoutBase> layoutList)
        {
            if (layoutList.Count < 1) return "";

            StringBuilder sb = new StringBuilder();
            foreach (LayoutBase layout in layoutList)
            {
                sb.Append(layout.GetLayoutString());
          
            }
            return sb.ToString();
        }
    }
}
