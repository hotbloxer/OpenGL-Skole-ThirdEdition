using OpenTK.Mathematics;

namespace openGL2.Objects
{
    public class TangentAndBiNormalLogic
    {
        public static float[] MakeTangentsAndBiNormalsForTriangles(float[] positions, float[] uv)
        {
            Vector3 pos1, pos2, pos3;
            Vector2 uv1, uv2, uv3;


            int positionsCounter = 0;
            int uvCounter = 0;
            float[] tangetAndBiNormal = new float[(uv.Length / 2) * 6];
            for (int i = 0; i < uv.Length / 2; i += 6)
            {
                pos1 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);
                pos2 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);
                pos3 = new Vector3(positions[positionsCounter++], positions[positionsCounter++], positions[positionsCounter++]);

                uv1 = new Vector2(uv[uvCounter++], uv[uvCounter++]);
                uv2 = new Vector2(uv[uvCounter++], uv[uvCounter++]);
                uv3 = new Vector2(uv[uvCounter++], uv[uvCounter++]);

                float[] calculatedBnT = CalculateBiNormalerOgTangenter(pos1, pos2, pos3, uv1, uv2, uv3);
                tangetAndBiNormal[i] = calculatedBnT[0];
                tangetAndBiNormal[i + 1] = calculatedBnT[1];
                tangetAndBiNormal[i + 2] = calculatedBnT[2];
                tangetAndBiNormal[i + 3] = calculatedBnT[3];
                tangetAndBiNormal[i + 4] = calculatedBnT[4];
                tangetAndBiNormal[i + 5] = calculatedBnT[5];
            }
            return tangetAndBiNormal;
        }
        private static float[] CalculateBiNormalerOgTangenter(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            Vector3 tangent = new Vector3(0, 0, 0);
            Vector3 binormal = new Vector3(0, 0, 0);

            Vector3 edge1 = pos2 - pos1;
            Vector3 edge2 = pos3 - pos1;
            Vector2 delta1 = uv2 - uv1;
            Vector2 delta2 = uv3 - uv1;

            float f = 1.0f / (delta1.X * delta2.Y - delta2.X * delta1.Y);

            tangent.X = f * ((delta2.Y * edge1.X) - (delta1.Y * edge2.X));
            tangent.Y = f * ((delta2.Y * edge1.Y) - (delta1.Y * edge2.Y));
            tangent.Z = f * ((delta2.Y * edge1.Z) - (delta1.Y * edge2.Z));

            binormal.X = f * ((-delta2.X * edge1.X) + (delta1.X * edge2.X));
            binormal.Y = f * ((-delta2.X * edge1.Y) + (delta1.X * edge2.Y));
            binormal.Z = f * ((-delta2.X * edge1.Z) + (delta1.X * edge2.Z));

            tangent.Normalize();
            binormal.Normalize();

            return [tangent.X, tangent.Y, tangent.Z, binormal.X, binormal.Y, binormal.Z];
        }


    }
}