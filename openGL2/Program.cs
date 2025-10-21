
using openGL2;
using openGL2.Window;
using OpenTK.Windowing.Desktop;



namespace OpenGL
{
    public class Program
    {
        static void Main(string[] args)
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(1500, 800),
            };

            using Window window = new(GameWindowSettings.Default, nativeWindowSettings);
            using UI ui = new();
            ui.Start();
            window.Run();
        }
    }
}
