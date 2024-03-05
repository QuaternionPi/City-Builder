using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder;
public static class Program
{
    static void Main(string[] args)
    {
        int fps = 60;
        float deltaTime = 1 / fps;

        Window.SetTargetFPS(fps);
        Window.SetConfigFlags(Raylib_cs.ConfigFlags.FLAG_MSAA_4X_HINT);
        Window.Init(800, 600, "City Builder");

        int seed = 12991174;
        Map.Map map = Map.MapGen.FromSeed(80, 60, seed);

        IGraphics graphics = new RaylibGraphics();
        IKeyboard keyboard = new RaylibKeyboard();
        IMouse mouse = new RaylibMouse();

        while (!Window.ShouldClose())
        {
            graphics.BeginDrawing();
            graphics.ClearBackground(Color.RAYWHITE);
            map.Draw(graphics);
            graphics.EndDrawing();

            map.Update(keyboard, mouse, deltaTime);
        }
        Window.Close();
    }
}