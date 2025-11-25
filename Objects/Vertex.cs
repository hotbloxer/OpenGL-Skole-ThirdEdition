using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public class Vertex
    {
        public bool HasTanAndBiNormals = false;

        public float PositionX;
        public float PositionY;
        public float PositionZ;
       
        public float UvX;
        public float UvY;
    
        public float NormalX;
        public float NormalY;
        public float NormalZ;
        public float TangentX  {get; private set;}
        public float TangentY  {get; private set;}
        public float TangentZ  {get; private set;}
        public float BiNormalX {get; private set;}
        public float BiNormalY {get; private set;}
        public float BiNormalZ {get; private set;}

        public float colorR = 100;
        public float colorG = 100;
        public float colorB = 100;

        public Vertex(float[] positions, float[] uvs, float[] normals, bool hasTanAndBi = false)
        {
            PositionX = positions[0];
            PositionY = positions[1];
            PositionZ = positions[2];

            UvX = uvs[0];
            UvY = uvs[1];

            NormalX = normals[0];
            NormalY = normals[1];
            NormalZ = normals[2];

            if (hasTanAndBi)
            {
                float[] tanAndBis = TangentAndBiNormalLogic.MakeTangentsAndBiNormalsForTriangles(positions, uvs);
                SetTanAndBi(new float[] { tanAndBis[0], tanAndBis[1], tanAndBis[2] }, new float[] { tanAndBis[3], tanAndBis[4], tanAndBis[5] });
            }

     

        }

        public Vertex() 
        {
 

        }


        public void SetTanAndBi(float[] tangents, float[] biNormals)
        {
            HasTanAndBiNormals = true;
            TangentX = tangents[0];
            TangentY = tangents[1];
            TangentZ = tangents[2];
            BiNormalX = biNormals[0];
            BiNormalY = biNormals[1];
            BiNormalZ = biNormals[2];
        }
 


    }

    public static class VertexExtensions
    {
        public static float[] ToFlatArray(this Vertex[] vertcies)
        {
            List<float> flatArray = new List<float>();
            foreach (Vertex v in vertcies)
            {
                foreach (float f in v.ToArray())
                {
                    flatArray.Add(f);
                }
            }
            return flatArray.ToArray();
        }



        public static float[] ToArray(this Vertex vertex)
        {
            if (vertex.HasTanAndBiNormals)
            {
                return new float[]
                {
                    vertex.PositionX, vertex.PositionY, vertex.PositionZ,
                    vertex.UvX, vertex.UvY,
                    vertex.NormalX, vertex.NormalY, vertex.NormalZ,
                    vertex.TangentX, vertex.TangentY, vertex.TangentZ,
                    vertex.BiNormalX, vertex.BiNormalY, vertex.BiNormalZ 
                };
            }

            else
            {
                return new float[]
                {
                     vertex.PositionX, vertex.PositionY, vertex.PositionZ,
                    vertex.UvX, vertex.UvY,
                    vertex.NormalX, vertex.NormalY, vertex.NormalZ,
                };
            }
        }


        public static float[] PositionsToArray(this Vertex[] vertices)
        {
            float[] positionsArray = new float[vertices.Length * 3];
            int vertexIndex = 0;
            for (int i = 0; i < positionsArray.Length; i ++)
            {
                positionsArray[i++] = (vertices[vertexIndex].PositionX);
                positionsArray[i++] = (vertices[vertexIndex].PositionY);
                positionsArray[i  ] = (vertices[vertexIndex].PositionZ);
                vertexIndex++;
            }
            return positionsArray;
        }


        public static Face[] QuadToFaces(this Vertex[] vertices, bool AddTangenAndBinormal = false)
        {
            if (vertices.Length % 4 != 0)
            {
                throw (new Exception("Error in vertex length, only 4 vertices can be transformed"));
            }

            Face[] faces = new Face[vertices.Length / 2];

            for (int i = 0; i< faces.Length; i +=2)
            {
                int verticesIndex = i * 2;
                Face face1 = new Face(vertices[verticesIndex], vertices[verticesIndex + 1], vertices[verticesIndex + 2], AddTangenAndBinormal);
                Face face2 = new Face(vertices[verticesIndex], vertices[verticesIndex + 2], vertices[verticesIndex + 3], AddTangenAndBinormal);
                faces[i] = face1;
                faces[i +1] = face2;
            }
            return faces;
        }


        public static Face[] TriToFaces(this Vertex[] vertices, bool AddTangenAndBinormal = false)
        {
            if (vertices.Length % 3 != 0)
            {
                throw (new Exception("Error in vertex length, only 3 vertices can be transformed"));
            }

            Face[] faces = new Face[vertices.Length / 3];

            for (int i = 0; i < faces.Length; i ++)
            {
                int verticesIndex = i * 3;
                Face face1 = new Face(vertices[verticesIndex], vertices[verticesIndex + 1], vertices[verticesIndex + 2], AddTangenAndBinormal);
  
                faces[i] = face1;
            }
            return faces;



        }

    }
}
