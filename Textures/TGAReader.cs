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


            

                // set vigtiste out info
                imageInfo.alpha = tdaHeader.alpha;
                imageInfo.width = tdaHeader.width;
                imageInfo.height = tdaHeader.height;

                if (tdaHeader.alpha) 
                    pixels = SwitchRedAndBlueWithAlpha(bytes, (uint) tdaHeader.width * tdaHeader.height);
                else 
                    pixels = SwitchRedAndBlueWithoutAlpha(bytes, (uint) tdaHeader.width * tdaHeader.height);

            }
            return true;
        }


        public byte[] SwitchRedAndBlueWithoutAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray)
        {
            byte[] pixels = new byte[tgaFormattedArray.Length];

            int pixelCount = 0;
            for (uint i = 0; i < sizeOfPixelArray *3; i += 3)
            {
                pixels[pixelCount++]= tgaFormattedArray[18 + i + 2];
                pixels[pixelCount++] = tgaFormattedArray[18 + i + 1];
                pixels[pixelCount++] = tgaFormattedArray[18 + i];
            }
            return pixels;
        }

        public byte[] SwitchRedAndBlueWithAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray)
        {
            byte[] pixels = new byte[tgaFormattedArray.Length];
            int pixelCount = 0;
            for (uint i = 0;  i < sizeOfPixelArray *4; i += 4)
            {
                pixels[pixelCount++] = tgaFormattedArray[18+ i + 2];
                pixels[pixelCount++] = tgaFormattedArray[18 + i+ 1];
                pixels[pixelCount++] = tgaFormattedArray[18 + i + 1];
                pixels[pixelCount++] = tgaFormattedArray[18 + i + 3];
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
        byte[] SwitchRedAndBlueWithoutAlpha(byte[] tgaFormattedArray, uint sizeOfHeader);
        public byte[] SwitchRedAndBlueWithAlpha(byte[] tgaFormattedArray, uint sizeOfPixelArray);
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

    public struct ImageInformation
    {
        public ushort width;
        public ushort height;
        public bool alpha;
    }


}
