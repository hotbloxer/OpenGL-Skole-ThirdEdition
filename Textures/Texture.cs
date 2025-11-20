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

  

        public ImageInformation ImageInfo { get; private set; }

        public float MapIntensity { get; set; } = 1f;

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
        public Texture(ImageInformation imageInfo, string name)
        {
            _textureName = name;
            _textureID = LoadTextureToGPU(imageInfo);
            if (AllTextures.ContainsKey(name)) return;
            
            AllTextures.Add(_textureName, this);
            ImageInfo = imageInfo;
        }

        public static void RemoveAllTextures ()
        {
            foreach (Texture tex in AllTextures.Values)
            {
                tex.Dispose();
            }
        }
        protected virtual int Create(string filePath)
        {
            if (!ImageParser.ParseImage(filePath, ImageParser.ImageType.TGA, out ImageInformation imageInfo, out byte[] pixels))
            {
                throw new Exception("check this texture");
            }

            this.ImageInfo = imageInfo;

            // der bindes ikke til nogen texture unit her, det skal gøres i shaderen pr object
            return LoadTextureToGPU(imageInfo);
        }


        protected virtual int LoadTextureToGPU (ImageInformation imageInfo, bool repeatTiling = true)
        {
            GL.GenTextures(1, out int _textureID);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            SetTiling(_textureID);
            SetFilter(_textureID);

            if (imageInfo.alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageInfo.width, imageInfo.height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, imageInfo.pixels);

            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, imageInfo.width, imageInfo.height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, imageInfo.pixels);
            }

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return _textureID;
        }




        protected virtual  void SetTiling(int textureId, TextureWrapMode wrapMode = TextureWrapMode.Repeat)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        }

        protected virtual  void SetFilter(int textureId, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
        }

        public enum TextureFilterTypes { NEAREST, LINEAR, BILINEAR, TRILINEAR}
        protected virtual void SetFilterFromEnum (TextureFilterTypes filterType)
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

        public virtual void SetAnisotropic (bool anisotropic)
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
