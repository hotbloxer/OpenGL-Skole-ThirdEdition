using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders
{
    public class Matrix4Uniform
    {
        private Matrix4 _matrix;
        private readonly int _shaderHandle;
        private readonly string _uniformName;
        public Matrix4Uniform(int shaderHandle, string uniformName)
        {
            this._shaderHandle = shaderHandle;
            this._uniformName = uniformName;
        }

        public Matrix4 Matrix
        {
            get => _matrix;
            set
            {
                _matrix = value;
                SetUniformMatrix4(_shaderHandle, _uniformName, _matrix);
            }
        }

        private static void SetUniformMatrix4(int shadeHandler, string uniformname, Matrix4 matrix)
        {
            GL.UseProgram(shadeHandler);

            // Get the uniform location
            int location = GL.GetUniformLocation(shadeHandler, uniformname);

            if (location == -1)
                throw new Exception($"Uniform '{uniformname}' not found.");

            // Upload the matrix
            // transpose til false for at bevare kolonne orden
            GL.UniformMatrix4(location, false, ref matrix);
        }
    }

}
