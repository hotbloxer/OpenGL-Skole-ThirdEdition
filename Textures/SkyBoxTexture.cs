using OpenTK.Graphics.OpenGL4;


namespace openGL2.Textures
{
    public class SkyBoxTexture
    {
        int textureHandle;
        public int ID { get => textureHandle; }
        private int _shaderHandle;
        int vertexShader;
        int fragmentShader;
        public SkyBoxTexture(string name, string[] faces)
        {

            if (faces.Length != 6)
            {
                faces =
               [
                   @"..\..\..\Textures\TextureImages\right.tga",
                    @"..\..\..\Textures\TextureImages\left.tga",
                    @"..\..\..\Textures\TextureImages\bottom.tga",
                    @"..\..\..\Textures\TextureImages\top.tga",
                    @"..\..\..\Textures\TextureImages\front.tga",
                    @"..\..\..\Textures\TextureImages\back.tga",
                ];
            }

            textureHandle = loadCubemap(faces);
        }

        protected int loadCubemap(string[] faces)
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
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            return textureID;
        }
    }
}
