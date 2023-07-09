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
    class TestClass
    {
        public TestClass() { }
        public TestClass(Vector2 position) { Position = position; }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Position { get; private set; }
    }
}
