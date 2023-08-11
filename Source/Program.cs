using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public static class Program
    {
        static void Main()
        {
            Vector2 dimensions = new Vector2(800, 600);
            int fps = 60;
            String title = "City Builder";
            ConfigFlags[] flags = new[] { ConfigFlags.FLAG_MSAA_4X_HINT };
            RaylibScreen.CreateWindow(dimensions, fps, title, flags);

            IScreen screen = RaylibScreen.TheWindow!;
            IMouse mouse = new RaylibMouse();
            IKeyboard keyboard = new RaylibKeyboard();
            ICameraControler2D cameraControler = new KeyboardCameraControler(keyboard);

            Game game = new(screen, cameraControler, 20, 15);
            while (!Raylib.WindowShouldClose())
            {
                bool mouseBlocked = mouse.IsAnythingMoused(false);
                mouse.CheckForClick();
                game.Update(mouseBlocked);

                screen.BeginDrawing();
                screen.ClearBackground(Color.RAYWHITE);

                game.Render();

                screen.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
