using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static openGL2.Textures.Texture;

namespace openGL2.Textures
{
    public class TGAReader : IChangeTGAColorToRGB, IParseImageToBytes, IDisposable
    {
        public bool ParseImageToBytes(string fileName, out ImageInformation imageInfo, out byte[] pixels)
        {
            imageInfo = new ImageInformation();
            TDAHeader tdaHeader = new TDAHeader();
            pixels = new byte[0];
            if (!File.Exists(fileName)) return false;
            byte[] bytes = File.ReadAllBytes(fileName);
            if (bytes != null)
            {

                // sørens bit magi
                tdaHeader.identSize = bytes[0];
                tdaHeader.colorMapType = bytes[1];
                tdaHeader.imageType = bytes[2];
                tdaHeader.colorMapStart = (ushort)(bytes[3] + (bytes[4] << 8));
                tdaHeader.colorMapLength = (ushort)(bytes[5] + (bytes[6] << 8));
                tdaHeader.colorMapBits = bytes[7];
                tdaHeader.startX = (ushort)(bytes[8] + (bytes[9] << 8));
                tdaHeader.startY = (ushort)(bytes[10] + (bytes[11] << 8));
                tdaHeader.width = (ushort)(bytes[12] + (bytes[13] << 8));
                tdaHeader.height = (ushort)(bytes[14] + (bytes[15] << 8));
                tdaHeader.bits = bytes[16];
                tdaHeader.descriptor = bytes[17];
                byte colorChannels = (byte)(tdaHeader.bits >> 3);
                tdaHeader.alpha = colorChannels > 3;
                pixels = new byte[tdaHeader.height * tdaHeader.width * colorChannels];





                int offset = 18 + tdaHeader.identSize;
                if (tdaHeader.alpha) 
                {
                    pixels = SwitchRedAndBlueWithAlpha(bytes, (uint) tdaHeader.width * tdaHeader.height, offset);
                   
                }
                else 
                    pixels = SwitchRedAndBlueWithoutAlpha(bytes, (uint) tdaHeader.width * tdaHeader.height, offset);

                // set vigtiste out info
                imageInfo.alpha = tdaHeader.alpha;
                imageInfo.width = tdaHeader.width;
                imageInfo.height = tdaHeader.height;
                imageInfo.pixels = pixels;
            }
            return true;
        }


        public byte[] SwitchRedAndBlueWithoutAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray, int offset)
        {
            byte[] pixels = new byte[sizeOfPixelArray * 3];

            int pixelCount = 0;
            for (uint i = 0; i < sizeOfPixelArray *3; i += 3)
            {
                pixels[pixelCount++]= tgaFormattedArray[offset + i + 2];
                pixels[pixelCount++] = tgaFormattedArray[offset + i + 1];
                pixels[pixelCount++] = tgaFormattedArray[offset + i];
            }



            return pixels;
        }

        public byte[] SwitchRedAndBlueWithAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray, int offset)
        {
            byte[] pixels = new byte[sizeOfPixelArray * 4];
            int pixelCount = 0;
            for (uint i = 0;  i < sizeOfPixelArray * 4; i += 4)
            {
                pixels[pixelCount++] = tgaFormattedArray[offset + i + 2];
                pixels[pixelCount++] = tgaFormattedArray[offset + i + 1];
                pixels[pixelCount++] = tgaFormattedArray[offset + i];
                pixels[pixelCount++] = tgaFormattedArray[offset + i + 3];
            }


            return pixels;
        }

        public void Dispose()
        {

        }
    }

    public interface IParseImageToBytes
    {
        bool ParseImageToBytes(string fileName, out ImageInformation imageInfo, out byte[] pixels);

    }

    public interface IChangeTGAColorToRGB
    {
        byte[] SwitchRedAndBlueWithoutAlpha(byte[] tgaFormattedArray, uint sizeOfHeader, int offset);
        public byte[] SwitchRedAndBlueWithAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray, int offset);
    }

    public struct TDAHeader
    {
        public byte identSize;
        public byte colorMapType;
        public byte imageType;
        public ushort colorMapStart;
        public ushort colorMapLength;
        public byte colorMapBits;
        public ushort startX;
        public ushort startY;
        public ushort width;
        public ushort height;
        public byte bits;
        public byte descriptor;
        public bool alpha;
    }

    public class ImageInformation
    {
        public ushort width;
        public ushort height;
        public bool alpha;
        public byte[] pixels;


        public byte[] GetPixels(float u, float v)
        {

            byte[] pixel = new byte[3] ;
            int x = (int)((width - 1) * (u > 0.0f ? (u < 1.0f ? u : 1.0f) : 0.0f));
            int y = (int)((height - 1) * (v > 0.0f ? (v < 1.0f ? v : 1.0f) : 0.0f));

            pixel[0] = pixels[(y * width + x) * 3 ] ;
            pixel[1] = pixels[(y * width + x) * 3 +1];
            pixel[2] = pixels[(y * width + x) * 3 +2];

            return pixel;
        }
    }


}
