using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using openGL2.Objects;
using openGL2.Objects.Terrain;
using openGL2.Shaders;
using openGL2.Shaders.ShaderComAndElements;
using openGL2.Textures;
using openGL2.Window;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects.Scenes
{
    public abstract class Scene
    {
        public abstract void Load();
        public abstract void OnRenderFrame();

        public void CloseScene()
        {
            ShaderHandler.RemoveAllShaders();
            ObjectHandler.RemoveAllObjects();
            Texture.RemoveAllTextures();

        }

        public abstract void OpenScene();

    }
}
