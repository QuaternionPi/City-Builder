using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder;
public static class Program
{
    public static void Main(string[] args)
    {
        int fps = 60;
        float deltaTime = (float)1 / fps;

        //sWindow.SetTargetFPS(fps);
        Window.SetConfigFlags(Raylib_cs.ConfigFlags.FLAG_MSAA_4X_HINT);
        Window.Init(800, 600, "City Builder");

        int seed = 1291174;
        Map.Map map = Map.MapGen.FromSeed(80, 60, seed);

        IGraphics graphics = new RaylibGraphics();
        IKeyboard keyboard = new RaylibKeyboard();
        IMouse mouse = new RaylibMouse();

        while (!Window.ShouldClose())
        {
            graphics.BeginDrawing();
            graphics.ClearBackground(Color.RayWhite);
            map.Draw(graphics);
            Raylib_cs.Raylib.DrawFPS(20, 20);
            graphics.EndDrawing();

            map.Update(keyboard, mouse, deltaTime);
            if (keyboard.IsKeyReleased(KeyboardKey.Space))
            {
                seed++;
                map = Map.MapGen.FromSeed(80, 60, seed);
            }
        }
        Window.Close();
    }
}