using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public class Cube : IHaveVertices
    {
        VertexInformation vertexInfo;

        public Cube() 
        {
            vertexInfo = GetVertexInfo();
        }

        public VertexInformation GetVertexInformation()
        {
            return vertexInfo;
        }

        private VertexInformation GetVertexInfo ()
        {
            return new VertexInformation( // position
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
            });
        }
              
           
         
    }
}
