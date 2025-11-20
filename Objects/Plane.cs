using ImGuiNET;
using System.Numerics;


namespace openGL2.Objects
{
    public class Plane: IGeometry, IHaveUI
    {
        int _width = 1;
        int _height = 1;
        int _subdivideWidth = 20;
        int _subdivideHeight = 20;

        bool _updateGeometry = false;

        private VertexInformation _vertexInformation;
        public VertexInformation VertexInformation { get => _vertexInformation; }


        public int Width { get => _width; 
            set  { if (value > 0) _width = value; } }  
        public int Height { get => _height; set { if (value > 0) _height = value; } }
        public int SubdivideWidth { get => _subdivideWidth; 
            set { if (value > 0) _subdivideWidth = value; } }
        public int SubdivideHeight { get => _subdivideHeight; set { if (value > 0) _subdivideHeight = value; } }
        


        public VertexInformation GetVertexInformation()
        {
            float widthSize = (float)_width / _subdivideWidth;
            float heightSize = (float)_height / _subdivideHeight;

            List<Face> faces = new();
            Vector3 startPos = new(-_width / 2f, 0f, -_height / 2f);
            Vector2 uvLeft = new(0, 0);

            float uvWidth = 1f / _subdivideWidth;
            float uvHeight = 1f / _subdivideHeight;

            for (int h = 0; h < _subdivideHeight; h++)
            { 
                for (int w = 0; w < _subdivideWidth; w++)
                {

                    startPos = new Vector3(widthSize * w, 0, heightSize * h);


                    uvLeft = new Vector2(uvWidth * w, uvHeight * h);

                    Face[] facesArray = CreateQuad(startPos, widthSize, heightSize, uvLeft , uvWidth, uvHeight);
                    faces.AddRange(facesArray);
                }
            }
            return faces.ToArray().ToVertexInformation();
        }


        private static Face[] CreateQuad(Vector3 posLeftBottom, float width, float height, Vector2 uvLeft, float uvWidth, float uvHeight)
        {
            float[] normal = { 0.0f, 1.0f, 0.0f };

            Vertex leftBottom = new Vertex(
                new float[] { posLeftBottom.X, posLeftBottom.Y, posLeftBottom.Z },
                new float[] { uvLeft.X, uvLeft.Y },
                normal
            );

            Vertex rightBottom = new Vertex(
                 new float[] { posLeftBottom.X + width, posLeftBottom.Y, posLeftBottom.Z },
                 new float[] { uvLeft.X + uvWidth, uvLeft.Y },
                 normal
            );

            Vertex rightTop = new Vertex(
                new float[] { posLeftBottom.X + width, posLeftBottom.Y, posLeftBottom.Z + height},
                new float[] { uvLeft.X + uvWidth, uvLeft.Y + uvHeight },
                normal
            );


            Vertex leftTop = new Vertex(
                new float[] { posLeftBottom.X , posLeftBottom.Y, posLeftBottom.Z + height },
                new float[] { uvLeft.X, uvLeft.Y + uvHeight },
                normal
            );

            Face face1 = new Face(rightBottom, rightTop, leftBottom, true);
            Face face2 = new Face(leftBottom, rightTop, leftTop, true);

            Face[] faces = { face1, face2 };

            return faces;
        }

        public bool UpdatedGeometryThatAffectVBOLength()
        {

            if (_updateGeometry)
            {
                _vertexInformation = GetVertexInformation();
                _updateGeometry = false;
                return true;
            }

            return false;
        }



        public bool GetUI()
        {

            if (ImGui.InputInt("width", ref _width))
            {
                _updateGeometry = true;
            }

            if (ImGui.InputInt("height", ref _height))
            {
                _updateGeometry = true;
            }

            if (ImGui.InputInt("subdivideWidth", ref _subdivideWidth))
            {
                _updateGeometry = true;
            }

            if (ImGui.InputInt("subdivideHeight", ref _subdivideHeight))
            {
                _updateGeometry = true;
            }
            return true;
        }
    }
}
