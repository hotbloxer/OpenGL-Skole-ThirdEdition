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
        public Shader [] Shaders;        
        private Matrix4 _modelSpace;
        public bool Render { get; set; } = true;

        // Material
        public IMaterial Material;

        //Geometry
        private VertexInformation _vertexInformation;
        public IGeometry Geometry { get; }

        public IHaveUI HaveUI { get; }

        // Buffers
        public int VBOHandle { get; private set; } = -1;
        public int VAOHandle { get; private set; } = -1;

        private bool haveMaterial = true;

        public Figure (Shader[] shader, IGeometry geometry, IHaveUI ui, bool haveMaterial = true)
        {
            Geometry = geometry;

            HaveUI = ui;

            Shaders = shader;

            _vertexInformation = geometry.GetVertexInformation();

            GenerateVBOHandle();
            VAOHandle = VertexArrayObjectHandler.VAO;

            _modelSpace = Matrix4.Identity;
            Name = ObjectNamer();


            if (haveMaterial )
            {
                Material = new Material();
            }
            

            ObjectHandler.AddFigureToScene(this);
        }

        public Figure(Shader[] shader, IHaveVertices vertices, bool haveMaterial = true)
        {
            Shaders = shader;

            _vertexInformation = vertices.GetVertexInformation();


            GenerateVBOHandle();
            VAOHandle = VertexArrayObjectHandler.VAO;

            _modelSpace = Matrix4.Identity;
            Name = ObjectNamer();


            if (haveMaterial)
            {
                Material = new Material();
            }

            ObjectHandler.AddFigureToScene(this);
        }


        public Figure(Shader[] shader, VertexInformation imageInfo, bool haveMaterial = true)
        {
            this.haveMaterial = haveMaterial;

            Shaders = shader;

            _vertexInformation = imageInfo;


            GenerateVBOHandle();
            VAOHandle = VertexArrayObjectHandler.VAO;

            _modelSpace = Matrix4.Identity;
            Name = ObjectNamer();

            if (haveMaterial)
            {
                Material = new Material();
            }
            

            ObjectHandler.AddFigureToScene(this);
        }



        private string ObjectNamer ()
        {
            return $"objekt "+ ObjectHandler.ObjectnameCounter;
        }

        public void UpdateModelsSpace ()
        {
            Shader.UpdateModelSpace(_modelSpace);
        }

        
        public void TranslateFigure (Matrix4 translation)
        {
            _modelSpace *= translation;
        }

        public void SetModelSpace (Matrix4 modelSpace)
        {
            _modelSpace = modelSpace;
        }

        protected void GenerateVBOHandle()
        {
            VBOHandle = GL.GenBuffer();
            BindVBOAndData();
        }

        private void BindVBOAndData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexInformation.Vertices.Length * sizeof(float), _vertexInformation.Vertices, BufferUsageHint.DynamicDraw);
        }

        public void Draw ()
        {
            if (!Render) return;

            UpdateModelsSpace();

            foreach (Shader shader in Shaders)
            {
                shader.Use();



                if (haveMaterial)
                {
                    shader.SetUsingTexture(UI.useTexture);
                    if (Material.NormalTexture.MapIntensity <0.1f)
                    {
                        shader.SetUsingNormalMap(true);
                    }
                    else
                    {
                        shader.SetUsingNormalMap(false);
                    }
                }
                shader.SetUVTest(UI.displaUVTesting);

                shader.SetUsingBlinn(UI.UsingBlinnLight);
                shader.UsingRimLight(UI.UsingRimLight);
                shader.SetLightColor(UI.LightColorTK);
                shader.SetObjectColor(UI.ObjectColorTK);
                shader.UpdateUniformValuesForRender();

                shader.UpdateUniforms();
            
            

            GL.BindVertexArray(VAOHandle);

            if (Material != null && shader.usesMaterial)
            {
                Material.ActivateMaterial(shader);
                
            }
            
            
            if (Geometry != null)
            {
                if (Geometry.UpdatedGeometryThatAffectVBOLength())
                {
                    _vertexInformation = Geometry.GetVertexInformation();
                    RebuildVBO();
                }
            }
            

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexInformation.Vertices.Length);
            }
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

        public Figure GetDublicate ()
        {
            Figure newFigure = new Figure(Shaders, _vertexInformation);
            newFigure.Material = this.Material;
            newFigure.SetModelSpace(this._modelSpace);
            return newFigure;
        }

    }

    public interface IHaveUI
    {
        public bool GetUI();
    }

    public interface IGeometry 
    {

        public bool UpdatedGeometryThatAffectVBOLength();
        public VertexInformation GetVertexInformation();
    }

    public interface IHaveVertices
    {
        public VertexInformation GetVertexInformation();
    }

}
