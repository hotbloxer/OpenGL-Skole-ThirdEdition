using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class Uniform4Element : UniformElement
    {
      
        private Matrix4 _value;
        public Matrix4 Value {get => _value; set => UpdateValue(value); }
        public Uniform4Element(string name, Matrix4 value) : base(name, "mat4")
        {
            Value = value;

        }

        private void UpdateValue(Matrix4 value)
        {
            _value = value;
        }

        public override void SetUniform(int ShaderProgramHandle)
        {
            GL.UseProgram(ShaderProgramHandle);
            int location = GL.GetUniformLocation(ShaderProgramHandle, Name);
            if (location == -1)
                throw new Exception($"Uniform '{Name}' not found.");

            GL.UniformMatrix4(location, false, ref _value);
        }
    }


    public class UniformFloatElement : UniformElement
    {

        private float _value;
        public float Value { get => _value; set => UpdateValue(value); }
        public UniformFloatElement(string name, float value) : base(name, "float")
        {

            Value = value;

        }

        private void UpdateValue(float value)
        {
            _value = value;
        }

        public override void SetUniform(int ShaderProgramHandle)
        {
            GL.UseProgram(ShaderProgramHandle);
            int location = GL.GetUniformLocation(ShaderProgramHandle, Name);
            if (location == -1)
                throw new Exception($"Uniform '{Name}' not found.");

            GL.Uniform1(location, _value);
        }

    }
}
