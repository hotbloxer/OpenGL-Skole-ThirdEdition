using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders
{
    public static class ProjectedTextureShader
    {
        private static readonly int _shaderHandle;
        private static readonly int _fragmentShaderSource;
        private static readonly int _vertesSkaderSource;
        private static readonly Matrix4Uniform _orthoProjection;


        static ProjectedTextureShader()
        {

            _shaderHandle = GL.CreateProgram();



            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, SetDefaultVertexShader());

            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out var codeVert);
            if (codeVert != 1)
            {
                var infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception($"Fejl i shader ({vertexShader}).\n\n{infoLog}");
            }

            GL.AttachShader(_shaderHandle, vertexShader);


            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, SetDefaultFragmentShader());

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out var codeFrag);
            if (codeFrag != 1)
            {
                var infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception($"Fejl i shader ({fragmentShader}).\n\n{infoLog}");
            }

            GL.AttachShader(_shaderHandle, fragmentShader);

            GL.LinkProgram(_shaderHandle);

            // test for link fejl
            GL.GetProgram(_shaderHandle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_shaderHandle);
                Console.WriteLine(infoLog);
            }


            _orthoProjection = new(_shaderHandle, "ortho");


            _orthoProjection.Matrix = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, -100.0f, 100.0f);
        }


        public static void UseShader()
        {
            GL.UseProgram(_shaderHandle);
        }

        private static string SetDefaultVertexShader()
        {
            return
            @$"#version 330 core 
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec2 aUV;
            layout(location = 2) in vec3 aNormal;
            layout(location = 3) in vec3 aTangent;  
            layout(location = 4) in vec3 aBiNormal;

            uniform mat4 modelView;
            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 ortho;


            out vec2 uv;
            out vec3 vertexNormal;
            out vec3 fragPosition;


            void main() 
            {{ 
                fragPosition = vec3(model * vec4(aPosition, 1.0));
                uv = aUV;
                vertexNormal = aNormal;
                mat4 test = ortho;

                gl_Position = ortho * view  * model * vec4(aPosition, 1.0) ;
        


            }}";

            //TODO fix normal matrixen
            //mat3 normalMatrix = mat3(transpose(inverse(modelView)));  
            //normal =  aNormal * normalMatrix;
        }
        private static string SetDefaultFragmentShader()
        {
            return
            @$"#version 330 core 
            out vec4 FragColor; 
            in vec2 uv;
            in vec3 vertexNormal;
            in vec3 fragPosition;



            void main() 
            {{
            FragColor = vec4(1,1, 0.0, 1.0);

            // afslutning
            }}";
        }






    }
}
