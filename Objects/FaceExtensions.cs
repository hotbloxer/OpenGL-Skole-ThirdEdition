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


    }
}
