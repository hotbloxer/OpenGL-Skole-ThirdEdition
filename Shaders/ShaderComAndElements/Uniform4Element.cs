using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class UniformVec3Element : UniformElement
    {
      
        private Vector3 _value;
        public Vector3 Value {get => _value; set => UpdateValue(value); }
        public UniformVec3Element(string name, Vector3 value) : base(name, "vec3")
        {
            Value = value;

        }

        public void UpdateValue(Vector3 value)
        {
            _value = value;
        }

        public override void SetUniform(int ShaderProgramHandle)
        {
            GL.UseProgram(ShaderProgramHandle);
            int location = GL.GetUniformLocation(ShaderProgramHandle, Name);
            if (location == -1)
                throw new Exception($"Uniform '{Name}' not found.");

            GL.Uniform3(location, ref _value);
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

        public void UpdateValue(float value)
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
