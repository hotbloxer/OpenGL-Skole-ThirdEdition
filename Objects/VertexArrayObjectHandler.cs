using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public static class VertexArrayObjectHandler
    {
       
        public static int VAO { get => GetPositionAndUvVao(); }
        private static int GetPositionAndUvVao ()
        {
            int stride = 14;
            int offset = 0;

            int VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // position
            int positionLength = 3;
            GL.VertexAttribPointer(0, positionLength, VertexAttribPointerType.Float, false, stride * sizeof(float), offset);
            GL.EnableVertexAttribArray(0);
            offset += positionLength;

            // uv
            int uvLength = 2;
            GL.VertexAttribPointer(1, uvLength, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
            GL.EnableVertexAttribArray(1);
            offset += uvLength;

            // Normals
            int NormalLength = 3;
            GL.VertexAttribPointer(2, NormalLength, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
            GL.EnableVertexAttribArray(2);
            offset += NormalLength;

            
            int tangetLength = 3;
            GL.VertexAttribPointer(3, tangetLength, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
            GL.EnableVertexAttribArray(3);
            offset += tangetLength;

            int biNormalLength = 3;
            GL.VertexAttribPointer(4, biNormalLength, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
            GL.EnableVertexAttribArray(4);
            offset += biNormalLength;


            return VAO;
        }
    }
}

