using openGL2.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Tools
{
    public class VetexSelector
    {


        // viewport = x, y, width, height
        public int[] ProjectVertexToScreen(int[] viewport, float[] v,  Matrix4 mpv)
        {
            float[] PMVArray = new float[4];
            // projection model view
            Vector4 vec4 = mpv * new Vector4(v[0], v[1], v[2], 1);
            PMVArray[0] = vec4.X;
            PMVArray[1] = vec4.Y;
            PMVArray[2] = vec4.Z;
            PMVArray[3] = vec4.W;

            float[] tmp = PMVArray;
            // Convert homogeneous coordinates to cartesian coordinates
            tmp[0] = tmp[0] / tmp[3];
            tmp[1] = tmp[1] / tmp[3];
            tmp[2] = tmp[2] / tmp[3];

            // Map x, y and z to range 0 - 1
            tmp[0] = tmp[0] * 0.5f + 0.5f;
            tmp[1] = tmp[1] * 0.5f + 0.5f;
            tmp[2] = tmp[2] * 0.5f + 0.5f;

            //Map x and y to viewport
            tmp[0] = tmp[0] * viewport[2] + viewport[0];
            tmp[1] = tmp[1] * viewport[3] + viewport[1];

            tmp[1] = viewport[3] - tmp[1];

            int[] result = { (int)tmp[0], (int)tmp[1] };
            return result;
        }

    }
}
