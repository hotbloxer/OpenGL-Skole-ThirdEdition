namespace openGL2.Objects
{
    public class Face
    {
        public Vertex Vertex1 { get; private set; }
        public Vertex Vertex2 { get; private set; }
        public Vertex Vertex3 { get; private set; }

        public Face(Vertex vertex1, Vertex vertex2, Vertex vertex3, bool generateTanAndBi = true)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;

            if (generateTanAndBi)
            {
                float[] tanAndBiNorm = CreateTangentsAndBiNormals(Vertex1, Vertex2, Vertex3);

                float[] tangents = [tanAndBiNorm[0], tanAndBiNorm[1], tanAndBiNorm[2]];
                float[] biNormals = [tanAndBiNorm[3], tanAndBiNorm[4], tanAndBiNorm[5]];


                Vertex1.SetTanAndBi(tangents, biNormals);
                Vertex2.SetTanAndBi(tangents, biNormals);
                Vertex3.SetTanAndBi(tangents, biNormals);

            }
        }

        public float[] CreateTangentsAndBiNormals (Vertex v1, Vertex v2, Vertex v3)
        {
            float[] positions = [
                v1.PositionX, v1.PositionY, v1.PositionZ,
                v2.PositionX, v2.PositionY, v2.PositionZ,
                v3.PositionX, v3.PositionY, v3.PositionZ,
            ];

            float[] uvs =
                [
                v1.UvX, v1.UvY,
                v2.UvX, v2.UvY,
                v3.UvX, v3.UvY,
                ];

           return TangentAndBiNormalLogic.MakeTangentsAndBiNormalsForTriangles(positions, uvs);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Face) return false;

            Face face = (Face)obj;

            return
                face.Vertex1 == this.Vertex1 &&
                face.Vertex2 == this.Vertex2 &&
                face.Vertex3 == this.Vertex3;   
        }
    }
}
