using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace openGL2.Shaders
{
    public class SkyBoxShader
    {
        private Matrix4Uniform _view;
        private Matrix4Uniform _projection;

        public Matrix4 Projection { get => _projection.Matrix;  set => _projection.Matrix = value; }
        public Matrix4 View { get => _view.Matrix;  
            set {
                // remove translation part of the view matrix
                _view.Matrix = new Matrix4(new Matrix3(value));

            } }
        int textureHandle;

        public SkyBoxShader()
        {
            


            string[] faces =
            [
                @"..\..\..\Textures\TextureImages\right.tga",
                @"..\..\..\Textures\TextureImages\left.tga",
                @"..\..\..\Textures\TextureImages\bottom.tga",
                @"..\..\..\Textures\TextureImages\top.tga",
                @"..\..\..\Textures\TextureImages\front.tga",
                @"..\..\..\Textures\TextureImages\back.tga",
            ];
            textureHandle = loadCubemap(faces);
        
            CreateShaderProgram();
            CreateCube();

            _view = new Matrix4Uniform(_shaderHandle, "view");
            _projection = new Matrix4Uniform(_shaderHandle, "projection");

        }

        int loadCubemap(string[] faces)
        {
            
            
            GL.CreateTextures(TextureTarget.TextureCubeMap, 1, out int textureID);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

            int width, height, nrChannels;
            for (int i = 0; i < faces.Length; i++)
            {
                if (!ImageParser.ParseImage(faces[i], ImageParser.ImageType.TGA, out ImageInformation imageInfo, out byte[] pixels))
                {
                    throw new Exception("check this texture");
                }

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, 
                    PixelInternalFormat.Rgb, 
                    imageInfo.width, 
                    imageInfo.height, 
                    0, 
                    PixelFormat.Rgb, 
                    PixelType.UnsignedByte, 
                    pixels);
                
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            return textureID;
        }

        int vboHandle;
        int vaoHandle;
        public void CreateCube ()
        {
            vboHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, skyboxVertices.Length * sizeof(float), skyboxVertices, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        private int _shaderHandle;
        int vertexShader;
        int fragmentShader;
        private void CreateShaderProgram()
        {
            _shaderHandle = GL.CreateProgram();

             vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VertexShaderSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out var code);
            if (code != 1)
            {
                var infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception($"Fejl i shader ({vertexShader}).\n\n{infoLog}");
            }
            GL.AttachShader(_shaderHandle, vertexShader);


            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, FragmentShaderSource);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out code);
            if (code != 1)
            {
                var infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception($"Fejl i shader ({fragmentShader}).\n\n{infoLog}");
            }
            GL.AttachShader(_shaderHandle, fragmentShader);

            GL.LinkProgram(_shaderHandle);

        }

        public void Draw()
        {
            GL.DepthMask(false);

            GL.UseProgram(_shaderHandle);

            int textureUnit = 0;
            GL.BindVertexArray(vaoHandle);
            //GL.ActiveTexture(0);
            //GL.BindTexture(TextureTarget.TextureCubeMap, textureHandle);

            
            GL.BindTextureUnit(textureUnit, textureHandle);

            int location = GL.GetUniformLocation(_shaderHandle, "skybox");
            if (location == -1)
                throw new Exception($"Uniform 'skybox' not found.");

            GL.Uniform1(location, textureUnit);

            GL.DrawArrays(PrimitiveType.Triangles, 0, skyboxVertices.Length / 3);

            GL.DepthMask(true);
        }

        string VertexShaderSource = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;

            out vec3 TexCoords;

            uniform mat4 projection;
            uniform mat4 view;

            void main()
            {
                TexCoords = aPos;
                gl_Position = projection * view * vec4(aPos, 1.0);
                
            } 
        ";


        string FragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;

            in vec3 TexCoords;

            uniform samplerCube  skybox;

            void main()
            {    
               // FragColor = textureCube(skybox, TexCoords);
               //vec4 pixel = vec4(0.0, 0.0, 0.0, 1.0);
                   // FragColor = mix(pixel, textureCube(skybox, -TexCoords), 1);

               // vec4 tempPixel = vec4(texture(skybox, uv).rgb, 1.0);
                FragColor = texture(skybox, -TexCoords);
                
            }
        ";




        float[] skyboxVertices = [
            
            -0.5f, -0.5f, -0.5f, // Front face
            0.5f, -0.5f, -0.5f,
            0.5f,  0.5f, -0.5f,
            0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f, // Back face
            0.5f, -0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f, // Left face
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

            0.5f,  0.5f,  0.5f, // Right face
            0.5f,  0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f, // Bottom face
            0.5f, -0.5f, -0.5f,
            0.5f, -0.5f,  0.5f,
            0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f, // Top face
            0.5f,  0.5f, -0.5f,
            0.5f,  0.5f,  0.5f,
            0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
        ];


    }
}
