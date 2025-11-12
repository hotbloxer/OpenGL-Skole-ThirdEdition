using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class Uniform2DSamplerElement : UniformElement
    {
        public Texture Texture { get; }
        int value;
  
        public Uniform2DSamplerElement(string name, int value, Texture texture) : base(name, "sampler2D")
        {
            this.value = value;

            Texture = texture; 
        }

        public override void SetUniform(int ShaderProgramHandle)
        {
            GL.UseProgram(ShaderProgramHandle);
            GL.BindTextureUnit(value, Texture.ID);
            int location = GL.GetUniformLocation(ShaderProgramHandle, Name);
            if (location == -1)
                throw new Exception($"Uniform '{Name}' not found.");

            GL.Uniform1(location, value);
        }
    }
}
