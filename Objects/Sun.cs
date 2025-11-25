using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public class Sun: IHaveUI
    {
        public Vector3 Position;
        public Vector3 LightColor = new Vector3(1,1,1);
        private float _sunDirectionRad;
        private float _sunHeightRad = 0;
        private float prevHeight = 0;

        public static Sun Instance;

        public delegate void SunDelegate (Vector3 dir, Vector3 color);
        public static event SunDelegate SunChangeEvent;


        float prevAngle;

        public Sun ()
        {
            Position = new(0, 100, 0);
            Instance = this;
            prevAngle = _sunDirectionRad;

        }
        bool useAnimation = false;
        public bool GetUI()
        {
            if (!useAnimation)
            { 
                if (ImGui.SliderAngle("SunDirection", ref _sunDirectionRad, 0, 360))
                {
                    UpdateLightDirection();
                
                }

                if (ImGui.SliderAngle("Sun Height", ref _sunHeightRad, -180, 180))
                {
                    UpdateLightDirection();

                }
            }

            if (ImGui.Button("Animation"))
            {
                useAnimation = !useAnimation;
            }

            return false;

        }

        float AnimationCounter = 0;
        public void UpdateSunAnimation ()
        {
            if (!useAnimation) return;
            _sunDirectionRad += 0.001f;
            if (_sunDirectionRad > 360)
            {
                _sunDirectionRad = 0;
            }
            


            _sunHeightRad += 0.001f;
            if (_sunDirectionRad > 180)
            {
                _sunDirectionRad = -180;
            }

            UpdateLightDirection();
        }

        private void UpdateLightDirection()
        {
            float newAngle =  _sunDirectionRad - prevAngle;
            Matrix4 sunRotation = Matrix4.CreateRotationY(newAngle);
            Vector4 sunPos4d = new Vector4(Position.X, Position.Y, Position.Z, 0);
            Position = new Vector3((sunPos4d * sunRotation));
            

            float newHeighAngle = _sunHeightRad - prevHeight;
            Matrix4 sunRotationHeight = Matrix4.CreateRotationX(newHeighAngle);
            Vector4 sunPos4dHeihgt = new Vector4(Position.X, Position.Y, Position.Z, 0);
            
            
            Position = new Vector3((sunPos4dHeihgt * sunRotationHeight));


     

         
            if (Position.Y > 51)
            {
                LightColor = new(1,1f, 1f);
            }


            else if (Position.Y < 70)
            {
                float factor = (Position.Y - 60 ) / 40;
                float nonReds = 1 + (1 - 0.7f) * factor;

                LightColor = new(1, nonReds, nonReds);
            }

            else if (Position.Y < 80)
            {
                LightColor = new(1, 0.5f, 0.5f);
            }

            SunChangeEvent.Invoke(Position, LightColor);
          

            prevAngle = _sunDirectionRad;
            prevHeight = _sunHeightRad;
        }
    }
}
