namespace openGL2.Objects
{
    public static class FaceExtensions
    {
        public static float[] ToFlatArray(this Face face)
        {
            float[] vertex1 = face.Vertex1.ToArray();
            float[] vertex2 = face.Vertex2.ToArray(); 
            float[] vertex3 = face.Vertex3.ToArray();

            float[] combined = new float[vertex1.Length + vertex2.Length + vertex3.Length];

            for (int i = 0; i < vertex1.Length; i ++)
            {
                combined[i                      ] = vertex1[i];
                combined[i + vertex1.Length     ] = vertex2[i];
                combined[i + vertex1.Length * 2 ] = vertex3[i];
            }
            return combined;
        }


        public static float[] ToArray(this Face[] faces)
        {
            int totalLengthOfReturnArray = faces.Length * faces[0].ToFlatArray().Length;
            float[] returnFaces = new float[totalLengthOfReturnArray];

            int currentIndex = 0;
            foreach (Face face in faces)
            {
                float[] faceAsArray = face.ToFlatArray();
                for (int faceIndex = 0; faceIndex < faceAsArray.Length; faceIndex++)
                {
                    returnFaces[currentIndex++] = faceAsArray[faceIndex];
                }
            }

            return returnFaces;
        }

        public static Vertex[] ToVertexArray(this Face[] faces)
        {
            List<Vertex> vertices = new List<Vertex>();

            foreach (Face face in faces)
            {
                vertices.Add(face.Vertex1);
                vertices.Add(face.Vertex2);
                vertices.Add(face.Vertex3);

            }

            return vertices.ToArray();
        }


        public static VertexInformation ToVertexInformation(this Face[] faces)
        {
            List<float> positions = new List<float>();
            List<float> uvs = new List<float>();
            List<float> normals = new List<float>();

            foreach (Face face in faces)
            {
                Vertex[] faceVertices = new Vertex[] { face.Vertex1, face.Vertex2, face.Vertex3 };

                foreach (Vertex vertex in faceVertices)
                {
                    positions.Add(vertex.PositionX);
                    positions.Add(vertex.PositionY);
                    positions.Add(vertex.PositionZ);
                    uvs.Add(vertex.UvX);
                    uvs.Add(vertex.UvY);
                    normals.Add(vertex.NormalX);
                    normals.Add(vertex.NormalY);
                    normals.Add(vertex.NormalZ);
                }
            }

            return new VertexInformation(positions.ToArray(), uvs.ToArray(), normals.ToArray(), faces.ToArray());


        }


        public static float[] ToNormals (this Face[] faces)
        {
            float[] normals = new float[faces.Length * 3 * 3];
            int faceIndex = 0;
            for (int i = 0; i < normals.Length; i++)
            {
                Face face = faces[faceIndex++];
                normals[i++] = face.Vertex1.NormalX;
                normals[i++] = face.Vertex1.NormalY;
                normals[i++] = face.Vertex1.NormalZ;

                normals[i++] = face.Vertex2.NormalX;
                normals[i++] = face.Vertex2.NormalY;
                normals[i++] = face.Vertex2.NormalZ;

                normals[i++] = face.Vertex3.NormalX;
                normals[i++] = face.Vertex3.NormalY;
                normals[i  ] = face.Vertex3.NormalZ;
            }
            return normals;
        }

        public static float[] ToPositions(this Face[] faces)
        {
            float[] positions = new float[faces.Length * 3 * 3];
            int faceIndex = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                Face face = faces[faceIndex++];
                positions[i++] = face.Vertex1.PositionX;
                positions[i++] = face.Vertex1.PositionY;
                positions[i++] = face.Vertex1.PositionZ;

                positions[i++] = face.Vertex2.PositionX;
                positions[i++] = face.Vertex2.PositionY;
                positions[i++] = face.Vertex2.PositionZ;

                positions[i++] = face.Vertex3.PositionX;
                positions[i++] = face.Vertex3.PositionY;
                positions[i] = face.Vertex3.PositionZ;
            }
            return positions;
        }


        public static float[] ToUvs(this Face[] faces)
        {
            float[] uvs = new float[faces.Length * 2 * 3];
            int faceIndex = 0;
            for (int i = 0; i < uvs.Length; i++)
            {
                Face face = faces[faceIndex++];
               uvs[i++] = face.Vertex1.UvX;
               uvs[i++] = face.Vertex1.UvY;
 
               uvs[i++] = face.Vertex2.UvX;
               uvs[i++] = face.Vertex2.UvY;
 
               uvs[i++] = face.Vertex3.UvX;
               uvs[i  ] = face.Vertex3.UvY;
   
            }
            return uvs;
        }





    }
}
