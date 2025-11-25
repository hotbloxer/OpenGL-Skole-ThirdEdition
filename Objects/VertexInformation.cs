namespace openGL2.Objects
{
    public class VertexInformation
    {
        float[] _positions;
        float[] _normals;
        float[] _uvs;
        float[] _vbo;
        Face[] _faces;
        float[] _colors;
        List<Vertex> _vertexes = new();

        public float[] Vertices { get; private set; }
        

        public VertexInformation(float[] positions, float[] uvs, float[] normals)
        {
            this._positions = positions;
            this._normals = normals;
            this._uvs = uvs;
            InitColors();
            Vertices = GetCombinedInfoForVertecis(this);
        }

        public VertexInformation(float[] positions, float[] uvs, float[] normals, float[] vertices)
        {
            this._positions = positions;
            this._normals = normals;
            this._uvs = uvs;
            InitColors();
            Vertices = GetCombinedInfoForVertecis(this);

        }

        public VertexInformation(Face[] faces)
        {
            this._faces = faces;

            _normals = faces.ToNormals();
            _positions = faces.ToPositions();
            _uvs = faces.ToUvs();
            InitColors();
            Vertices = GetCombinedInfoForVertecis(this);


        }


        public VertexInformation(VertexInformation[] objects)
        {
        
            List<Face> faces = new List<Face>();

            foreach (VertexInformation v in objects)
            {
                v.MakeFaces();
                foreach (Face face in v.Faces)
                {
                    
                    faces.Add(face);
                }
            }

            this._faces = faces.ToArray();

            _normals = faces.ToArray().ToNormals();
            _positions = faces.ToArray().ToPositions();
            _uvs = faces.ToArray().ToUvs();
      

        }


        public float[] Positions { get => _positions; }
        public float[] Normals { get => _normals; }
        public float[] Uvs { get => _uvs; }
        public Face[] Faces { get => _faces; }
        public float[] VBO { get => _vbo; }
        public float[] Colors { get => _colors; set  { _colors = value; Vertices = GetCombinedInfoForVertecis(this); } }



        private void InitColors()
        {
            _colors = new float[_positions.Length];
            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = 0.5f;
            }

        }

        private float[] GetCombinedInfoForVertecis(VertexInformation vertexInfo)
        {

            int positionLength = vertexInfo.Positions.Length;
            int biAndTangentLength = vertexInfo.Positions.Length * 2; // binormal og tanget er altid 6 lang, altså dobbelt af position
            int uvsLength = vertexInfo.Uvs.Length;
            int normalLength = vertexInfo.Normals.Length;
            int colors = vertexInfo.Positions.Length; // never mind, det skal gå hurtigt, 3 color, 3 positions, det går lige up i sidste ende
            
            int totalLength = positionLength + uvsLength + normalLength + biAndTangentLength + colors;
            if (totalLength == 0) totalLength = 1; // used for division later, and thus cannot be zero

            int verticesInTotal = positionLength / 3; // position er altid 3 lang, og altid nødvendig, så derfor tages total vertices herfra
            int dataLengthInVertex = totalLength / verticesInTotal;

            float[] combinedInfo = new float[totalLength];

            int vertexCount = 0;
            int uvCounter = 0;
            int normalCounter = 0;
            int colorCounter = 0;

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

                vertex.PositionX = combinedInfo[i];
                vertex.PositionY = combinedInfo[i + 1];
                vertex.PositionZ = combinedInfo[i + 2];

                // add uvs
                combinedInfo[i + 3] = vertexInfo.Uvs[uvCounter++];
                combinedInfo[i + 4] = vertexInfo.Uvs[uvCounter++];

                vertex.UvX = combinedInfo[i + 3];
                vertex.UvY = combinedInfo[i + 4];
            

                // add normals
                combinedInfo[i + 5] = vertexInfo.Normals[normalCounter++];
                combinedInfo[i + 6] = vertexInfo.Normals[normalCounter++];
                combinedInfo[i + 7] = vertexInfo.Normals[normalCounter++];

                vertex.NormalX = combinedInfo[i + 5];
                vertex.NormalY = combinedInfo[i + 6];
                vertex.NormalZ = combinedInfo[i + 7];


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

                combinedInfo[i + 14] = _colors[colorCounter++];
                combinedInfo[i + 15] = _colors[colorCounter++];
                combinedInfo[i + 16] = _colors[colorCounter++];


                _vertexes.Add(vertex);

            }
            return combinedInfo;
        }


        public Face[] MakeFaces ()
        {
            List<Face> faces = new List<Face>();
            for (int i = 0; i < _vertexes.Count; i+=3)
            {
                faces.Add(new Face(_vertexes[i++], _vertexes[i ++], _vertexes[i ++]));
            }

            return faces.ToArray();
        }
    }

    public interface IParseObjectToVertexInformation
    {

    }
}
