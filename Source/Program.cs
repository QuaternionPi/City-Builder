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
            Raylib.SetTargetFPS(60);
            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            Raylib.InitWindow(800, 600, "City Builder");

            //String text = File.ReadAllText("../Resources/Data/test.json");
            //Game game = Game.LoadGame(text);
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
