using openGL2.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace openGL2.Window
{
    public class Camera 
    {
      

        Vector2 _windowSize;

        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }

        private Vector3 _position = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 Position { get => _position; set => _position = value; }

        public Vector3 _front = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 _up = new Vector3(0.0f, 1.0f, 0.0f);

        

        private Controls _controls;

        private float _pitch;
        private float _yaw;

        /// <summary>
        /// set and get pitch in degrees
        /// </summary>
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        /// <summary>
        /// set and get yaw in degrees
        /// </summary>
        public float Yaw
        {
            
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public Camera(Vector2 size)
        {
            _windowSize = size;

            _controls = new Controls(this);
            Pitch = 0.0f;
            Yaw = -90.0f;
        }

        public Camera(Window window)
        {
            _windowSize = window.Size;

            _controls = new Controls(this);
            Pitch = 0.0f;
            Yaw = -90.0f; 

        }

        public Camera(System.Numerics.Vector2 windowSize)
        {
        }

        private void UpdateVectors()
        {
            // kode copy fra OpenTK learn
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);
        }

        public void UpdateView()
        {
            SetViewMatrix();
            SetProjectionMatrix();
            SetCameraPosition();
        }

        private void SetCameraPosition()
        {
            Shader.UpdateCameraPosition(_position);
        }

        private void SetProjectionMatrix()
        {
            Projection = 
                Matrix4.
                    CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(45.0f), // field of view
                        _windowSize.X/ _windowSize.Y,  // aspect radius på viewport
                        0.1f,  // min clipping
                        1000.0f); // max clipping


            Shader.UpdateProjection(Projection);
        }

        private void SetViewMatrix()
        {
            Shader.UpdateView(View);
       
        }


        public void UpdateCamera (KeyboardState input, FrameEventArgs e, MouseState mouse)
        {
            _controls.UpdatePosition(input, e); // updates position
            View = Matrix4.LookAt(_position, _position + _front, _up);
            _controls.UpdateViewDirection(mouse);

        }
    }
}
