namespace openGL2.Objects
{
    public class VertexInformation
    {
        float[] _positions;
        float[] _normals;
        float[] _uvs;
        float[] _vbo;
        Face[] _faces;

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

        public VertexInformation(Face[] faces)
        {
            this._faces = faces;

            _normals = faces.ToNormals();
            _positions = faces.ToPositions();
            _uvs = faces.ToUvs();
            Vertices = GetCombinedInfoForVertecis(this);

        }

        public float[] Positions { get => _positions; }
        public float[] Normals { get => _normals; }
        public float[] Uvs { get => _uvs; }
        public Face[] Faces { get => _faces; }
        public float[] VBO { get => _vbo; }


        private static float[] GetCombinedInfoForVertecis(VertexInformation vertexInfo)
        {

            int positionLength = vertexInfo.Positions.Length;
            int biAndTangentLength = vertexInfo.Positions.Length * 2; // binormal og tanget er altid 6 lang, altså dobbelt af position
            int uvsLength = vertexInfo.Uvs.Length;
            int normalLength = vertexInfo.Normals.Length;
            
            int totalLength = positionLength + uvsLength + normalLength + biAndTangentLength;
            if (totalLength == 0) totalLength = 1; // used for division later, and thus cannot be zero

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
    }

    public interface IParseObjectToVertexInformation
    {

    }
}
