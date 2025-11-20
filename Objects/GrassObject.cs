using OpenTK.Mathematics;


namespace openGL2.Objects
{
    internal class GrassObject : IHaveVertices
    {
        VertexInformation vertexInfo;
        
        public GrassObject ()
        {
            vertexInfo = GetVertexInfo();
        }


        public VertexInformation GetVertexInformation()
        {
            return vertexInfo;
        }

        private VertexInformation GetVertexInfo()
        {
            float[] planeTemp = new float[] {
                 0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
            };

            float[] plane1 = rotate(planeTemp, 0);

            float[] plane2 = rotate(planeTemp, 60);


            float[] plane3 = rotate(planeTemp, -60);


            int planeLength = planeTemp.Length; 
            float[] allPlanes = new float[planeLength * 3];

            Array.Copy(plane1, 0, allPlanes, 0, planeLength);
            Array.Copy(plane2, 0, allPlanes, planeLength, planeLength);
            Array.Copy(plane3, 0, allPlanes, planeLength * 2, planeLength);


            return new VertexInformation( // position
            allPlanes,

            // uvs
            new float[] {
                0.0f, 0.0f,  // Front face
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,

                0.0f, 0.0f,  // Front face
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,


                0.0f, 0.0f,  // Front face
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f,
            },


            // normals
            new float[] {

                    0.0f,  1.0f, 0.0f, // Front face
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,

                    0.0f,  1.0f, 0.0f, // Front face
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,


                    0.0f,  1.0f, 0.0f, // Front face
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
                    0.0f,  1.0f, 0.0f,
            });
        }

        private float[] rotate(float[] arrayIn, int degree)
        {
            float[] roateted = (float[])arrayIn.Clone();
            Vector3 center = new(0.0f, 0.0f, 0.0f);


            Matrix4 rot = Matrix4.CreateRotationY((float)(degree * (Math.PI / 180.0)));
            Matrix4 scale = Matrix4.CreateScale(0.2f);

            for (int i = 0; i< arrayIn.Length; i+= 3)
            {
                Vector4 vec = new(arrayIn[i], arrayIn[i + 1], arrayIn[i + 2], 1);

                Vector4 vecROtated = scale * rot * vec;
                roateted[i] = vecROtated.X;
                roateted[i+1] = vecROtated.Y;
                roateted[i+2] = vecROtated.Z;
            }



            return roateted;
        }


    }
}
