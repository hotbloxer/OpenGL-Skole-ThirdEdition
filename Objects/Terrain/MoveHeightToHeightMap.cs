using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects.Terrain
{
    public class MoveHeightToHeightMap
    {
        List<Figure> figuresToMove = new();
        List<float> uvs = new();
        ImageInformation heightMap;

        bool firstTime = true;  
        float previousHeight;

        HeightMapVertexShader heightMapShader;



        public MoveHeightToHeightMap(HeightMapVertexShader heightMapShader)
        {
            this.heightMap = heightMapShader.ImageInfo;
            this.heightMapShader = heightMapShader;
        }

        public void AddFiguresAndUVs(Figure[] figures, float[] uvs)
        {
            foreach (Figure f in figures)
            {
                figuresToMove.Add(f);
            }

            foreach (float uv in uvs)
            {
                this.uvs.Add(uv);
            }
        }

        public void MoveFiguresToHeight()
        {
            float difference = 0;
            if (firstTime)
            {
                previousHeight = heightMapShader.Height;
                firstTime = false;

                difference = heightMapShader.Height;

                UpdatePositions(difference);
            }

            else if (previousHeight == heightMapShader.Height)
            {
                return;
            }



            else  
            {
                float rawHeight = heightMapShader.Height;
                difference =  rawHeight - previousHeight;
                previousHeight = rawHeight;
                UpdatePositions(difference);
            }
        }

        private void UpdatePositions (float difference)
        {
            for (int i = 0; i < figuresToMove.Count; i++)
            {
                Figure f = figuresToMove[i];

                float u = uvs[i * 2];
                float v = uvs[i * 2 + 1];

                byte[] pixel = heightMap.GetPixels(u, v);
                float height = (float)pixel[0] / 255 * difference / 255;
                Matrix4 translation = Matrix4.CreateTranslation(new(0.0f, height, 0.0f));

                f.TranslateFigure(translation);
            }
        }
    }
}
