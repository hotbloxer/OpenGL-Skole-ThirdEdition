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
        float[] positions;
        float[] normals;
        float[] uvs;



        public VertexInformation(float[] positions, float[] uvs, float[] normals)
        {
            this.positions = positions;
            this.normals = normals;
            this.uvs = uvs;
        }

       
        public float[] Positions { get => positions; }
        public float[] Normals { get => normals; }
        public float[] Uvs { get => uvs; }


        public static float[] GetCombinedInfoForVertecis(VertexInformation vertexInfo)
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
            float[] tangetAndBinormals = MakeTangentsAndBiNormalsForTriangles(vertexInfo.Positions, vertexInfo.Uvs);

            bool firstTriangle = true;
            for (int i = 0; i < totalLength; i += dataLengthInVertex)
            {

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
                combinedInfo[i + 13] = tangetAndBinormals[binormalAndTangetCounter +5];

                
            }
            return combinedInfo;
        }
       

        private static float[] MakeTangentsAndBiNormalsForTriangles(float[] positions, float[] uv)
        {
            Vector3 pos1, pos2, pos3;
            Vector2 uv1, uv2, uv3;


            int positionsCounter = 0;
            int uvCounter = 0;
            float[] tangetAndBiNormal = new float[(uv.Length / 2) * 6];
            for (int i = 0; i < uv.Length / 2; i += 6)
            {
                pos1 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);
                pos2 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);
                pos3 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);

                uv1 = new Vector2(uv[uvCounter++], uv[uvCounter++]);
                uv2 = new Vector2(uv[uvCounter++], uv[uvCounter++]);
                uv3 = new Vector2(uv[uvCounter++], uv[uvCounter++]);

                float[] calculatedBnT = CalculateBiNormalerOgTangenter(pos1, pos2, pos3, uv1, uv2, uv3);
                tangetAndBiNormal[i] = calculatedBnT[0];
                tangetAndBiNormal[i + 1] = calculatedBnT[1];
                tangetAndBiNormal[i + 2] = calculatedBnT[2];
                tangetAndBiNormal[i + 3] = calculatedBnT[3];
                tangetAndBiNormal[i + 4] = calculatedBnT[4];
                tangetAndBiNormal[i + 5] = calculatedBnT[5];
            }
            return tangetAndBiNormal;
        }

        private static float[] CalculateBiNormalerOgTangenter(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            Vector3 tangent = new Vector3(0, 0, 0);
            Vector3 binormal = new Vector3(0, 0, 0);

            Vector3 edge1 = pos2 - pos1;
            Vector3 edge2 = pos3 - pos1;
            Vector2 delta1 = uv2 - uv1;
            Vector2 delta2 = uv3 - uv1;

            float f = 1.0f / (delta1.X * delta2.Y  - delta2.X * delta1.Y);

            tangent.X = f *  ((delta2.Y  * edge1.X) - (delta1.Y * edge2.X));
            tangent.Y = f *  ((delta2.Y  * edge1.Y) - (delta1.Y * edge2.Y));
            tangent.Z = f *  ((delta2.Y  * edge1.Z) - (delta1.Y * edge2.Z));

            binormal.X = f * ((-delta2.X * edge1.X) + (delta1.X * edge2.X));
            binormal.Y = f * ((-delta2.X * edge1.Y) + (delta1.X * edge2.Y));
            binormal.Z = f * ((-delta2.X * edge1.Z) + (delta1.X * edge2.Z));

            tangent.Normalize();
            binormal.Normalize();

            return [tangent.X, tangent.Y, tangent.Z, binormal.X, binormal.Y, binormal.Z];
        }
    }
}
