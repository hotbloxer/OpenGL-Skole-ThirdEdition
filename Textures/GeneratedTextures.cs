using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static openGL2.Textures.GeneratedTextures;

namespace openGL2.Textures
{
    public static class GeneratedTextures
    {
        public enum GeneratedTexures { CHECKERED, WHITE }
        public static Texture GetGeneratedTexture (GeneratedTexures texture)
        {
            ImageInformation i = new();
            byte[] p = [];
            string _textureName;

            switch (texture)
            {
                case GeneratedTexures.CHECKERED:
                    i = GeneratedTextures.GenerateChekceredTexture(out i, out  p);
                    _textureName = "Chekered";
                    break;

                case GeneratedTexures.WHITE:
                    i = GeneratedTextures.GenerateWhiteTexture(out i, out  p);
                    _textureName = "White";
                    break;

                default:
                    _textureName = "Unknown";
                    break;
            }
            return new Texture(i, _textureName);
        }



    public static ImageInformation GenerateChekceredTexture(out ImageInformation imageInformation, out byte[] pixels)
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

            return imageInformation;
        }



        public static ImageInformation GenerateWhiteTexture(out ImageInformation imageInformation, out byte[] pixels)
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
        return imageInformation;
        }

    }
}
