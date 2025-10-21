using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Textures
{
    public class ImageParser
    {

        public enum ImageType {TGA}
        public static bool ParseImage(string fileName, ImageType imageType, out ImageInformation imageInfo, out byte[] pixels)
        {
            imageInfo = new();
            pixels = new byte[0];

            switch (imageType)
            {
                case ImageType.TGA:
                    using (var test = new TGAReader())
                    {
                        return test.ParseImageToBytes(fileName, out imageInfo, out pixels);
                    }
                default:
                    return false;
            }

        }


    }
}

