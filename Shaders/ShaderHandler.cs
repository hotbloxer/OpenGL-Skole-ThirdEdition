using System.Collections.Generic;

namespace openGL2.Shaders
{
    public static class ShaderHandler
    {
        private static Dictionary<string, Shader> _shaders = new ();
        private static int runningNo = 0;
     

        public static void UpdateShaderScripts()
        {
            foreach (Shader shader in _shaders.Values)
            {
                shader.IsUpToDate = false;
            }
        }

        public static void AddShader (Shader shader, string name = "")
        {
            runningNo ++;
            if (name == "")
            {
                name = "shader_" + runningNo;
            }
            _shaders.Add(name, shader);
        }

        public static void RemoveShader (Shader shaderToRemove)
        {
            for (int i = 0; i < _shaders.Values.Count; i++)
            {
                Shader shader = _shaders.Values.ElementAt(i);
                
                if (shaderToRemove == shader)
                {
                    _shaders.Remove(_shaders.Keys.ElementAt(i));
                    return;
                }
            }
        }

        public static Dictionary<string, Shader> GetShaders ()
        {
            return _shaders;
        }

        public static void UpdateAllShaders()
        {
            foreach (Shader shader in _shaders.Values)
            {
                shader.UseShader();
            }
        }
    }
}