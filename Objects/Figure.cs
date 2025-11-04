using openGL2.Shaders;
using openGL2.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace openGL2.Objects
{
    public class Figure: IDisposable
    {
        
        public string Name { get; } = $"objekt {ObjectHandler.ObjectnameCounter}";


        // Rendering
        private Shader _shader;        
        private Matrix4 _modelSpace;
        public bool Render { get; set; } = true;

        // Material
        public IMaterial Material;


        //Geometry
        private VertexInformation _vertexInformation;
        public IGeometry Geometry { get; }


        // Buffers
        public int VBOHandle { get; } = -1;
        public int VAOHandle { get; private set; } = -1;


        


        public Figure (Shader shader, IGeometry geometry, bool withUV = true)
        {
            Geometry = geometry;

            _shader = shader;

            _vertexInformation = geometry.GetVertexInformation();


            GenerateVBOHandle();
            VAOHandle = VertexArrayObjectHandler.VAO;

            _modelSpace = Matrix4.Identity;
            Name = ObjectNamer();

            Material = new Material();

            ObjectHandler.AddFigureToScene(this);
        }



        private string ObjectNamer ()
        {
            return $"objekt "+ ObjectHandler.ObjectnameCounter;
        }



        //REFA Denne er ikke nødvendig mere

        //public enum FigureType { QUAD, CUBE, TERRAIN }
        //public enum VertexInfo { POSITION, UV }

        //private VertexInformation GetVertexInformation(FigureType type) => type switch
        //{
        //    FigureType.QUAD => PrimitivesVertexFigures.GetSquare(),
        //    FigureType.CUBE => PrimitivesVertexFigures.GetCube(),
        //    FigureType.TERRAIN => PrimitivesVertexFigures.GetSquare(), // TODO fix 
        //    _ => PrimitivesVertexFigures.GetSquare(),
        //};


        public void UpdateModelsSpace ()
        {
            Shader.UpdateModelSpace(_modelSpace);
        }

        
        public void TranslateFigure (Matrix4 translation)
        {
            _modelSpace *= translation;
        }

        public float[] GetVertices ()
        {
            return _vertexInformation.Vertices;
        }

        protected void GenerateVBOHandle()
        {
            VAOHandle = GL.GenBuffer();
            BindVBOAndData();

        }

        private void BindVBOAndData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VAOHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexInformation.Vertices.Length * sizeof(float), _vertexInformation.Vertices, BufferUsageHint.DynamicDraw);
        }

        public void Draw ()
        {
            if (!Render) return;

            _shader.Use();

            GL.BindVertexArray(VAOHandle);
            Material.ActivateMaterial(_shader);

            if (Geometry.UpdatedGeometryThatAffectVBOLength()) 
            {
                _vertexInformation = Geometry.GetVertexInformation();
                RebuildVBO();

            }
            
            _shader.SetUVTest(UI.displaUVTesting);
            _shader.SetUsingTexture(UI.useTexture);
            _shader.SetUsingBlinn(UI.UsingBlinnLight);
            _shader.UsingCellShader(UI.UsingCellShading);
            _shader.UsingRimLight(UI.UsingRimLight);
            _shader.SetLightColor(UI.LightColorTK);
            _shader.SetObjectColor(UI.ObjectColorTK);
            _shader.UpdateUniformValuesForRender();

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexInformation.Vertices.Length);
        }


        /// <summary>
        /// Only used to change exising VBO data, not anything that affect the length of the VBO
        /// </summary>
        public void UpdateVBO()
        {

            // code received from Søren
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);
            IntPtr p = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.ReadWrite);

            unsafe
            {
                float* f = (float*)p;
                for (int i = 0; i < _vertexInformation.Vertices.Length; i++)
                {
                    f[i] = _vertexInformation.Vertices[i];
                }
            }
            GL.UnmapBuffer(BufferTarget.ArrayBuffer);

            // code received from Søren
        }

        /// <summary>
        /// this was an important thing to implement
        /// when geometry is adding og removing vertices, then it cannot just be change, since the length has change
        /// it resulted in an afterimage or errors. So instead the entire VBO is remade once more vertices are added.
        /// </summary>
        public void RebuildVBO ()
        {
            BindVBOAndData();
        }


        public void Dispose()
        {
            GL.DeleteBuffer(VBOHandle);
            GL.DeleteVertexArray(VAOHandle);
        }
    }

    public interface IGeometry 
    {
        public void GetUI();
        public bool UpdatedGeometryThatAffectVBOLength();

        public VertexInformation GetVertexInformation();
    }


}
