using ImGuiNET;
using openGL2.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class AnimatedGrass : ShaderElementBase, IHaveUI
    {
        float sizeFactor = 0.01f;
        float timeFactor = 5f;
        float _WindDirRad = 0;
        Vector3 windDir = new Vector3(1, 0, 1);

        UniformFloatElement timer = new UniformFloatElement("time", 0);
        UniformFloatElement factorUniform;
        UniformVec3Element windDirUniform;


        public AnimatedGrass() : base(ShaderType.VertexShader)
        {
            Apply = true;


            

            factorUniform = new UniformFloatElement("factor", sizeFactor);
            windDirUniform = new UniformVec3Element("windDir", windDir);

            uniforms.Add("animTime", timer);
            uniforms.Add("factorUni", factorUniform);
            uniforms.Add("windUni", windDirUniform);

            ShaderCode = @$"     
                
                float newX = {PositionVertexShader.Position}.y > 0 ? time :  0;
                float newZ = {PositionVertexShader.Position}.y > 0 ? time :  0;

                
                vec4 tempPos = model * vec4({PositionVertexShader.Position}, 1);

                tempPos.x += sin(newX * 6.2831853) * factor * windDir.x;
                tempPos.z += sin(newZ * 6.2831853) * factor * windDir.z;


                {PositionVertexShader.Position} = vec3 (inverse(model) * tempPos);

                
                
                gl_Position = projectionViewModel * vec4({PositionVertexShader.Position}, 1.0);
            ";
        }

        public bool GetUI()
        {
            if (ImGui.SliderFloat("Sway distance", ref sizeFactor, 0.00001f, 0.1f))
            {
                UpdateFactor();
            }

            if (ImGui.SliderFloat("time factor", ref timeFactor, 50, 0.1f))
            {
                UpdateFactor();
            }

            if (ImGui.SliderAngle("wind direction", ref _WindDirRad, -180, 180))
            {
                UpdateWindDir();
            }

            return true;
        }

        public void UpdateTime (float newTime)
        {
            timer.UpdateValue(newTime * timeFactor);
        }

        private void UpdateFactor ()
        {
            factorUniform.UpdateValue(sizeFactor);
        }

        private void UpdateWindDir ()
        {
            Matrix4 windRotation = Matrix4.CreateRotationY(_WindDirRad);
            Vector4 windDirVec4 = new Vector4(windDir.X, windDir.Y, windDir.Z, 0);
            Vector3 NormalizedVec3WindDir = new Vector3((windDirVec4 * windRotation));
            windDirUniform.UpdateValue(NormalizedVec3WindDir);
        }
    }
}
