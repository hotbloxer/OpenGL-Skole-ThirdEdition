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
    public class Controls
    {
        Camera camera;

        float speed = 1.5f;

        public Controls(Camera camera) 
        { 
            this.camera = camera;
        }


        public void UpdatePosition(KeyboardState input, FrameEventArgs e)
        {
            Vector3 front = camera._front;
            Vector3 up = camera._up;

            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += front * speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= front * speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftControl))
            {
                camera.Position -= up * speed * (float)e.Time; //Down
            }
        }


        bool firstMove = true;
        private float sensitivity = 0.2f;
        private Vector2 lastMousePosition;

        public void UpdateViewDirection(MouseState mouse)
        {
            if (!mouse.IsButtonDown(MouseButton.Right))
            {
                return;
            }

            if (firstMove)
            {
                lastMousePosition = new Vector2(mouse.X, mouse.Y);
              
                firstMove = false;
            }
            
            camera.Yaw += (mouse.X - lastMousePosition.X) * sensitivity;
            camera.Pitch -= (mouse.Y - lastMousePosition.Y) * sensitivity;

            lastMousePosition = mouse.Position;
        }
    }
}
