using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Raylib_cs;

namespace CityBuilder
{
    public class Button : IRenderable, IClickable, IRectangle
    {
        public Button(Vector2 position, Vector2 dimensions, String text)
        {
            Shown = true;
            Position = position;
            Dimensions = dimensions;
            MouseCollider = new RectangleCollider(position, dimensions);
            Text = text;
        }
        protected String Text { get; set; }
        public bool Shown { get; }
        protected RectangleCollider MouseCollider { get; set; }
        public event EventHandler? LeftClick;
        public event EventHandler? RightClick;
        public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
        public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
        public Vector2 Position { get; set; }
        public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
        public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
        public Vector2 Dimensions { get; set; }
        public void Render()
        {
            Color boxColor = IsMoused() ? new Color(255, 255, 255, 125) : new Color(255, 255, 255, 55);
            Raylib.DrawRectangle((int)(X - Width / 2), (int)(Y - Height / 2), (int)Width, (int)Height, boxColor);
            Vector2 textDimensions = Raylib.MeasureTextEx(Raylib.GetFontDefault(), Text, 20, 1);
            float x = X - textDimensions.X / 2;
            float y = Y - textDimensions.Y / 2;
            Raylib.DrawTextEx(Raylib.GetFontDefault(), Text, new(x, y), 20, 1, Color.LIGHTGRAY);
        }
        public bool IsMoused()
        {
            Vector2 mousePosition = Raylib.GetMousePosition();
            return MouseCollider.Collides(mousePosition);
        }
        public void OnLeftClick()
        {
            LeftClick?.Invoke(this, EventArgs.Empty);
        }
        public void OnRightClick()
        {
            RightClick?.Invoke(this, EventArgs.Empty);
        }
        public void Initialize(GUIManager guiManager)
        {
            guiManager.Attach(this, 3);
        }
    }
}