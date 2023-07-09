using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace CityBuilder
{
    class Program
    {
        static void Main()
        {
            Raylib.InitWindow(800, 600, "Hello World!");

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                Raylib.DrawText("Hello, world!", 12, 12, 20, Color.BLACK);

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}