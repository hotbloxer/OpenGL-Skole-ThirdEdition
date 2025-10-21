using openGL2.Shaders;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openGL2.Textures
{



    public class Texture : IDisposable
    {
        public static Dictionary<string, Texture> AllTextures = [];

        int _textureID; 
        public int ID { get => _textureID;}

        string _textureName;
        public string Name { get => _textureName;}

        private TextureFilterTypes _filterType;
        public TextureFilterTypes FilterType { get => _filterType; set => SetFilterFromEnum(value); }

        private static string _checkeredKey = "Checkered";

        /// <summary>
        /// denne bruges til at loade TGA texturer med
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="name"></param>
        public Texture (string filePath, string name)
        {
            _textureName = name;
            _textureID = Create(filePath);

            AllTextures.Add(_textureName, this);
        }

        /// <summary>
        /// use this to instantiate raw bytes
        /// </summary>
        /// <param name=""></param>
        public Texture(ImageInformation imageInfo, byte[] pixels, string name)
        {
            _textureName = name;
            _textureID = LoadTextureToGPU(imageInfo, pixels);
            AllTextures.Add(_textureName, this);
        }

        enum GeneratedTexures { CHECKERED, WHITE}

        /// <summary>
        /// blank test texture
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <param name="pixels"></param>
        /// <param name="name"></param>
        private Texture(GeneratedTexures texture)
        {
            ImageInformation i = new();
            byte[] p = [];
            

            switch (texture)
            {
                case GeneratedTexures.CHECKERED:
                    GenerateChekceredTexture(out i, out  p);
                    _textureName = _checkeredKey;
                    break;

                case GeneratedTexures.WHITE:
                    GenerateWhiteTexture(out i, out  p);
                    _textureName = _whiteKey;
                    break;
            }
            
             
            _textureID = LoadTextureToGPU(i, p);
            AllTextures.Add(_textureName, this);
            


        }

        public static Texture GetCheckered ()
        {
            
            if (AllTextures.ContainsKey(_checkeredKey)) return AllTextures[_checkeredKey];

            return new Texture(GeneratedTexures.CHECKERED);
        }

        static string _whiteKey = "white";
        public static Texture GetWhite ()
        {
            if (AllTextures.ContainsKey(_whiteKey)) return AllTextures[_whiteKey];

            return new Texture(GeneratedTexures.WHITE);
        }



        private static int Create(string filePath)
        {
            if (!ImageParser.ParseImage(filePath, ImageParser.ImageType.TGA, out ImageInformation imageInfo, out byte[] pixels))
            {
                throw new Exception("check this texture");
            }
            // der bindes ikke til nogen texture unit her, det skal gøres i shaderen pr object
            return LoadTextureToGPU(imageInfo, pixels);
        }


        private static int LoadTextureToGPU (ImageInformation imageInfo, byte[] pixels, bool repeatTiling = true)
        {
            GL.GenTextures(1, out int _textureID);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            SetTiling(_textureID);
            SetFilter(_textureID);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, imageInfo.width, imageInfo.height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return _textureID;
        }

        private static void GenerateChekceredTexture(out ImageInformation imageInformation, out byte[] pixels)
        {
            imageInformation = new ImageInformation();
            imageInformation.height = 8;
            imageInformation.width = 8;
            imageInformation.alpha = false;

            byte O = 0;
            byte I = 255;

            pixels =
                [
                O,O,O,O,O,O,O,O,O,O,O,O,I,I,I,I,I,I,I,I,I,I,I,I,
                O,O,O,O,O,O,O,O,O,O,O,O,I,I,I,I,I,I,I,I,I,I,I,I,
                O,O,O,O,O,O,O,O,O,O,O,O,I,I,I,I,I,I,I,I,I,I,I,I,
                O,O,O,O,O,O,O,O,O,O,O,O,I,I,I,I,I,I,I,I,I,I,I,I,
                I,I,I,I,I,I,I,I,I,I,I,I,O,O,O,O,O,O,O,O,O,O,O,O,
                I,I,I,I,I,I,I,I,I,I,I,I,O,O,O,O,O,O,O,O,O,O,O,O,
                I,I,I,I,I,I,I,I,I,I,I,I,O,O,O,O,O,O,O,O,O,O,O,O,
                I,I,I,I,I,I,I,I,I,I,I,I,O,O,O,O,O,O,O,O,O,O,O,O,
                ];
        }

        private static void GenerateWhiteTexture(out ImageInformation imageInformation, out byte[] pixels)
        {
            imageInformation = new ImageInformation();
            imageInformation.height = 4;
            imageInformation.width = 4;
            imageInformation.alpha = false;

            pixels =
                [
                255,255,255,255,255,255,255,255,255,255,255,255,
                255,255,255,255,255,255,255,255,255,255,255,255,
                255,255,255,255,255,255,255,255,255,255,255,255,
                255,255,255,255,255,255,255,255,255,255,255,255,
                ];
        }


        private static void SetTiling(int textureId, TextureWrapMode wrapMode = TextureWrapMode.Repeat)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        }

        private static void SetFilter(int textureId, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
        }

        public enum TextureFilterTypes { NEAREST, LINEAR, BILINEAR, TRILINEAR}
        private void SetFilterFromEnum (TextureFilterTypes filterType)
        {
            switch (filterType)
            {
                case TextureFilterTypes.NEAREST:
                    SetFilter(_textureID, TextureMinFilter.Nearest, TextureMagFilter.Nearest);
                    break;

                case TextureFilterTypes.LINEAR:
                    SetFilter(_textureID, TextureMinFilter.Linear, TextureMagFilter.Linear);
                    break;

                case TextureFilterTypes.BILINEAR:
                    SetFilter(_textureID, TextureMinFilter.LinearMipmapNearest, TextureMagFilter.Linear);
                    break;

                case TextureFilterTypes.TRILINEAR:
                    SetFilter(_textureID, TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear);
                    break;

            }
            _filterType = filterType;

        }

        public void SetAnisotropic (bool anisotropic)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            if (anisotropic)
            {
                float maxAnisotropy = GL.GetFloat(GetPName.MaxTextureMaxAnisotropy);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, maxAnisotropy);
            }
            // slukket = 0
            else
            {
                float maxAnisotropy = GL.GetFloat(GetPName.MaxTextureMaxAnisotropy);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, 0);
            }
        }

        public void Dispose()
        {
            AllTextures.Remove(_textureName);
        }
    }

}
