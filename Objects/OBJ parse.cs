using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public static class OBJParser
    {
        public static ObjectInformation LoadOBJ(string path)
        {
            StringBuilder sb = new StringBuilder();
            string file = File.ReadAllText(path);
            if (file == null || file.Length == 0) return null;

            List<float> positionsInfoHolder = new List<float>();
            List<float> uvsInfoHolder = new List<float>();
            List<float> normalsInfoHolder = new List<float>();
            List<int> facesInfoHolder = new List<int>();
            
            string[] lines = file.Split('\n');

            foreach (string line in lines)
            {
                if (line.Length < 2) continue;
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                switch (parts[0])
                {
                    case "v":
                        AddToVertexInfoHolder(positionsInfoHolder, parts);
                        break;

                    case "vt":
                        AddToVertexInfoHolder(uvsInfoHolder, parts);
                        break;

                    case "vn":
                        AddToVertexInfoHolder(normalsInfoHolder, parts);
                        break;

                    case "f":
                        AddToFacesParser(facesInfoHolder ,parts);
                        break;
                }
            }

            ObjectInformation objectInformation = new()
            {
                Positions = positionsInfoHolder.ToArray(),
                Uvs = uvsInfoHolder.ToArray(),
                Normals = normalsInfoHolder.ToArray(),
                FaceInformation = facesInfoHolder.ToArray()
            };
            return objectInformation;
            
        }

        private static void AddToVertexInfoHolder(List<float> list, string[] parts)
        {
            // skib den første del som er identifieren
            for (int i = 1; i < parts.Length; i++)
            {
                list.Add(float.Parse(parts[i], CultureInfo.InvariantCulture));
            }
        }

        private static void AddToFacesParser(List<int> list, string[] part)
        {
            for (int i = 1; i < part.Length; i++)
            {
                string[] segment = part[i].Split('/');
                foreach (string data in segment)
                {
                    list.Add(int.Parse(data, CultureInfo.InvariantCulture));
                }
            }
        }
    }


    public class ObjectInformation
    {
        public Face[] Faces { get => GetFacesFromFaceinformation(Vertices); }

        public int[] FaceInformation;
        public float[] Positions;
        public float[] Uvs;
        public float[] Normals;

        public VertexInformation VertexInformation { get => MakeVertexInformation(Positions, Uvs, Normals, Faces); }

        /// <summary>
        /// returns the vertices from the faceInformation, as quads
        /// </summary>
        public Vertex[] Vertices
        {
            get
            {
                return CreateVerticesFromFaceInformation(FaceInformation, Positions, Uvs, Normals);
            }
        }


        private static Face[] GetFacesFromFaceinformation(Vertex[] vertices)
        {
            return vertices.QuadToFaces(true);
        }

        private static Vertex[] CreateVerticesFromFaceInformation(int[] faceInformation, float[] positions, float[] uvs, float[] normals)
        {
            // an obj gives 3 information points per vertex in a face: position, uv, normal
            int informationPointsInVertex = 3;
            int vertexCount = faceInformation.Length / informationPointsInVertex;

            Vertex[] vertices = new Vertex[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                Vertex vertex = new Vertex();
                int faceIndex = i * 3;
                                                                  //- 1 to convert from 1 based to 0 based indexing
                int positionStart = (faceInformation[faceIndex    ] - 1) * 3;
                int uvStart       = (faceInformation[faceIndex + 1] - 1) * 2;
                int normalStart   = (faceInformation[faceIndex + 2] - 1) * 3;

                vertex.PositionX = positions[positionStart ++];
                vertex.PositionY = positions[positionStart ++];
                vertex.PositionZ = positions[positionStart   ];

                vertex.UvX = uvs[uvStart ++];
                vertex.UvY = uvs[uvStart   ];

                vertex.NormalX = normals[normalStart ++];
                vertex.NormalY = normals[normalStart ++];
                vertex.NormalZ = normals[normalStart   ];

                vertices[i] = vertex;
            }
            return vertices;
        }

        public static VertexInformation MakeVertexInformation(float[] positions, float[] uvs, float[] normals, Face[] faces)
        {
            return new VertexInformation(positions, uvs, normals, faces.ToArray());
        }
    }
}
