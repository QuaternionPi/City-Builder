using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    class Program
    {
        static void Main()
        {
            Vector2 dimensions = new Vector2(800, 600);
            int fps = 60;
            String title = "City Builder";
            ConfigFlags[] flags = new[] { ConfigFlags.FLAG_MSAA_4X_HINT };
            RaylibScreen.CreateWindow(dimensions, fps, title, flags);
            IScreen screen = RaylibScreen.TheWindow!;

            Game game = new(20, 15);
            while (!Raylib.WindowShouldClose())
            {
                game.UpdateCycle();
                game.ClickCycle();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                Raylib.DrawText("Hello, world!", 12, 12, 20, Color.BLACK);
                game.RenderCycle();
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
