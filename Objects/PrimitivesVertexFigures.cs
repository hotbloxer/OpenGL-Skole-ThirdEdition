using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static openGL2.Objects.Figure;

namespace openGL2.Objects
{
    public static class PrimitivesVertexFigures
    {
        public static VertexInformation GetSquare()
        {
            return new VertexInformation(

                 // position
                 new float[] {
                         0.5f, -0.5f, 0.0f,  //  right bottom 
                         0.5f,  0.5f, 0.0f,  //  right top
                        -0.5f, -0.5f, 0.0f,  //  left 
                        -0.5f, -0.5f, 0.0f,  //  left bottom
                         0.5f,  0.5f, 0.0f,  //  right top
                        -0.5f,  0.5f, 0.0f,  //  left top
                 },

                 // uvs
                 new float[] {
                          1.0f,   0.0f,  //  right bottom
                          1.0f,   1.0f,  //  top right 
                          0.0f,   0.0f,  //  bottom left

                          0.0f,   0.0f,  //  bottom left
                          1.0f,   1.0f,  //  top right 
                          0.0f,   1.0f,  //  top left 
                          
                                  
                 },


                 // normals
                 new float[] {
                         0.0f,  0.0f, 1.0f,    //  bottom  right
                         0.0f,  0.0f, 1.0f,    //  top right 
                         0.0f,  0.0f, 1.0f,    //  bottom left
                                      
                         0.0f,  0.0f, 1.0f,    //  bottom left
                         0.0f,  0.0f, 1.0f,    //  top right 
                         0.0f,  0.0f, 1.0f,    //  top left 
                 }
            );
        }




        public static VertexInformation GetCube()
        {
            return new VertexInformation(

                // position
                new float[] {
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
                },

                // uvs
                new float[] {
                    0.0f, 0.0f,  // Front face
                    1.0f, 0.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f,

                    0.0f, 0.0f,  // Back face
                    1.0f, 0.0f,
                    1.0f, 1.0f,
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f,

                    1.0f, 0.0f,  // Left face
                    1.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 1.0f,
                    0.0f, 0.0f,
                    1.0f, 0.0f,

                    0.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 0.0f,  // Right face
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f,

                    0.0f, 1.0f,  // Bottom face
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f,

                    0.0f, 1.0f,  // Top face
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f
                },


                // normals
                new float[] {

                     0.0f,  0.0f, -1.0f, // Front face
                     0.0f,  0.0f, -1.0f,
                     0.0f,  0.0f, -1.0f,
                     0.0f,  0.0f, -1.0f,
                     0.0f,  0.0f, -1.0f,
                     0.0f,  0.0f, -1.0f,

                     0.0f,  0.0f,  1.0f, // Back face
                     0.0f,  0.0f,  1.0f,
                     0.0f,  0.0f,  1.0f,
                     0.0f,  0.0f,  1.0f,
                     0.0f,  0.0f,  1.0f,
                     0.0f,  0.0f,  1.0f,

                    -1.0f,  0.0f,  0.0f, // Left face
                    -1.0f,  0.0f,  0.0f,
                    -1.0f,  0.0f,  0.0f,
                    -1.0f,  0.0f,  0.0f,
                    -1.0f,  0.0f,  0.0f,
                    -1.0f,  0.0f,  0.0f,

                     1.0f,  0.0f,  0.0f, // Right face
                     1.0f,  0.0f,  0.0f,
                     1.0f,  0.0f,  0.0f,
                     1.0f,  0.0f,  0.0f,
                     1.0f,  0.0f,  0.0f,
                     1.0f,  0.0f,  0.0f,

                     0.0f, -1.0f,  0.0f, // Bottom face
                     0.0f, -1.0f,  0.0f,
                     0.0f, -1.0f,  0.0f,
                     0.0f, -1.0f,  0.0f,
                     0.0f, -1.0f,  0.0f,
                     0.0f, -1.0f,  0.0f,

                     0.0f,  1.0f,  0.0f, // Top face
                     0.0f,  1.0f,  0.0f,
                     0.0f,  1.0f,  0.0f,
                     0.0f,  1.0f,  0.0f,
                     0.0f,  1.0f,  0.0f,
                     0.0f,  1.0f,  0.0f
                }
            );
        }
    }


    public class VertexInformation
    {
        float[] _positions;
        float[] _normals;
        float[] _uvs;
        float[] _vbo;

        public float[] Vertices { get; }
        

        public VertexInformation(float[] positions, float[] uvs, float[] normals)
        {
            this._positions = positions;
            this._normals = normals;
            this._uvs = uvs;

            Vertices = GetCombinedInfoForVertecis(this);
        }

        public VertexInformation(float[] positions, float[] uvs, float[] normals, float[] vertices)
        {
            this._positions = positions;
            this._normals = normals;
            this._uvs = uvs;

            Vertices = vertices;
        }





        public float[] Positions { get => _positions; }
        public float[] Normals { get => _normals; }
        public float[] Uvs { get => _uvs; }


        public float[] VBO { get => _vbo; }


        private static float[] GetCombinedInfoForVertecis(VertexInformation vertexInfo)
        {

            int positionLength = vertexInfo.Positions.Length;
            int biAndTangentLength = vertexInfo.Positions.Length * 2; // binormal og tanget er altid 6 lang, altså dobbelt af position
            int uvsLength = vertexInfo.Uvs.Length;
            int normalLength = vertexInfo.Normals.Length;
            
            int totalLength = positionLength + uvsLength + normalLength + biAndTangentLength;

            int verticesInTotal = positionLength / 3; // position er altid 3 lang, og altid nødvendig, så derfor tages total vertices herfra
            int dataLengthInVertex = totalLength / verticesInTotal;

            float[] combinedInfo = new float[totalLength];

            int vertexCount = 0;
            int uvCounter = 0;
            int normalCounter = 0;

            int binormalAndTangetCounter = 0;
            int triangleCounter = 0;
            float[] tangetAndBinormals = TangentAndBiNormalLogic.MakeTangentsAndBiNormalsForTriangles(vertexInfo.Positions, vertexInfo.Uvs);

            bool firstTriangle = true;

  

            for (int i = 0; i < totalLength; i += dataLengthInVertex)
            {
                Vertex vertex = new Vertex();

                // add positions
                combinedInfo[i] = vertexInfo.Positions[vertexCount++];
                combinedInfo[i + 1] = vertexInfo.Positions[vertexCount++];
                combinedInfo[i + 2] = vertexInfo.Positions[vertexCount++];


                // add uvs
                combinedInfo[i + 3] = vertexInfo.Uvs[uvCounter++];
                combinedInfo[i + 4] = vertexInfo.Uvs[uvCounter++];

                // add normals
                combinedInfo[i + 5] = vertexInfo.Normals[normalCounter++];
                combinedInfo[i + 6] = vertexInfo.Normals[normalCounter++];
                combinedInfo[i + 7] = vertexInfo.Normals[normalCounter++];

                // add binormal and tanget til hver trekant ved at sætte de samme 3 gange i træk

                if (triangleCounter % 3 == 0 && firstTriangle)
                {
                    binormalAndTangetCounter += 6;
                    triangleCounter = 0;
                    firstTriangle = false;
                }

                combinedInfo[i + 8] = tangetAndBinormals[binormalAndTangetCounter ];
                combinedInfo[i + 9] = tangetAndBinormals[binormalAndTangetCounter  +1];
                combinedInfo[i + 10] = tangetAndBinormals[binormalAndTangetCounter +2];

                combinedInfo[i + 11] = tangetAndBinormals[binormalAndTangetCounter +3];
                combinedInfo[i + 12] = tangetAndBinormals[binormalAndTangetCounter +4];
                combinedInfo[i + 13] = tangetAndBinormals[binormalAndTangetCounter + 5];


            }
            return combinedInfo;
        }

        private static float[] CombineVBOFromFaces(int[] faces, float[] positions, float[] uvs, float[] normals)
        {
            // every face has 3 informations pos, uv and normal
            List<float> vboBuilder = new List<float>();

            List<Vertex> vertices = new List<Vertex>(); 

            for (int i = 0; i < faces.Length; i++)
            {
                // position
                int pos = faces[i];
                vboBuilder.Add( positions[3 * (pos - 1)]);
                vboBuilder.Add( positions[3 * (pos - 1) + 1]);
                vboBuilder.Add( positions[3 * (pos - 1) + 2]);

                // uvs
                i++;
                int uvPos = faces[i];
                vboBuilder.Add(  uvs[2 * (uvPos - 1)]);
                vboBuilder.Add(  uvs[2 * (uvPos - 1) + 1]);

                // normal
                i++;
                int normalPos = faces[i];
                vboBuilder.Add(  normals[3 * (normalPos - 1)]);
                vboBuilder.Add(  normals[3 * (normalPos - 1) + 1]);
                vboBuilder.Add(  normals[3 * (normalPos - 1) + 2]);
            }

  

            return vboBuilder.ToArray();
        }

        private static Vertex[] CalculateVertices  (int[] faces)
        {
            Vertex[] vertices = new Vertex[faces.Length / 6];
            for (int i = 0; i < faces.Length /6; i ++)
            {
                vertices[i] = new Vertex();

            }
            return vertices;
        }

    }
}
