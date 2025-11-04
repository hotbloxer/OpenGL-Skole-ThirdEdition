using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static openGL2.Objects.Figure;

namespace openGL2.Objects
{
    public static class PrimitivesVertexFigures
    {
        public static VertexInformation GetSquare()
        {
            return new VertexInformation(

                 // position
                 new float[] {
                         0.5f, -0.5f, 0.0f,  //  right bottom 
                         0.5f,  0.5f, 0.0f,  //  right top
                        -0.5f, -0.5f, 0.0f,  //  left 
                        -0.5f, -0.5f, 0.0f,  //  left bottom
                         0.5f,  0.5f, 0.0f,  //  right top
                        -0.5f,  0.5f, 0.0f,  //  left top
                 },

                 // uvs
                 new float[] {
                          1.0f,   0.0f,  //  right bottom
                          1.0f,   1.0f,  //  top right 
                          0.0f,   0.0f,  //  bottom left

                          0.0f,   0.0f,  //  bottom left
                          1.0f,   1.0f,  //  top right 
                          0.0f,   1.0f,  //  top left 
                          
                                  
                 },


                 // normals
                 new float[] {
                         0.0f,  0.0f, 1.0f,    //  bottom  right
                         0.0f,  0.0f, 1.0f,    //  top right 
                         0.0f,  0.0f, 1.0f,    //  bottom left
                                      
                         0.0f,  0.0f, 1.0f,    //  bottom left
                         0.0f,  0.0f, 1.0f,    //  top right 
                         0.0f,  0.0f, 1.0f,    //  top left 
                 }
            );
        }




        public static VertexInformation GetCube()
        {
            return new VertexInformation(

                // position
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
                }
            );


        }
    }
}
