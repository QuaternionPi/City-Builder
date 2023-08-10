using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public interface IScreen
    {
        public Vector2 Dimensions { get; }
        public Vector2 Middle { get; }
        public void DrawText(String text, Color color, Vector2 centre, int fontSize, int fontSpacing);
        public void DrawTexture(Texture2D texture, Vector2 centre);
        public void DrawTexture(Texture2D texture, Vector2 centre, float rotation);
        public void BeginDrawing();
        public void EndDrawing();
        public void ClearBackground(Color color);
    }
    public class RaylibScreen : IScreen
    {
        private RaylibScreen(Vector2 dimensions, int frameRate, String title, ConfigFlags[] configFlags)
        {
            Raylib.SetTargetFPS(frameRate);
            foreach (ConfigFlags flags in configFlags)
            {
                Raylib.SetConfigFlags(flags);
            }
            Raylib.InitWindow((int)dimensions.X, (int)dimensions.Y, title);
        }
        public Vector2 Dimensions
        {
            get
            {
                return new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }
        }
        public Vector2 Middle { get { return Dimensions * 0.5f; } }
        public static RaylibScreen? TheWindow { get; private set; }
        public void DrawText(String text, Color color, Vector2 centre, int fontSize, int fontSpacing)
        {
            Vector2 dimensions = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, fontSpacing);
            Vector2 position = centre - dimensions / 2;
            int x = (int)position.X;
            int y = (int)position.Y;
            Raylib.DrawText(text, x, y, fontSize, color);
        }
        public void DrawTexture(Texture2D texture, Vector2 centre) => DrawTexture(texture, centre, 0);
        public void DrawTexture(Texture2D texture, Vector2 centre, float rotation)
        {
            Vector2 dimensions = new(texture.width, texture.height);
            Rectangle rectangle = new(0, 0, dimensions.X, Dimensions.Y);

            Color color = Color.WHITE;
            Raylib.DrawTexturePro(texture, rectangle, rectangle, centre, rotation, color);
        }
        public void BeginDrawing() => Raylib.BeginDrawing();
        public void EndDrawing() => Raylib.EndDrawing();
        public void ClearBackground(Color color) => Raylib.ClearBackground(color);
        public static void CreateWindow(Vector2 dimensions, int frameRate, String title, ConfigFlags[] configFlags)
        {
            if (TheWindow != null) throw new Exception("This is a singleton");
            TheWindow = new RaylibScreen(dimensions, frameRate, title, configFlags);
        }
    }
}