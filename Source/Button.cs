using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Raylib_cs;

namespace CityBuilder
{
    public class Button : IRectangle, IRenderable
    {
        public Button(IScreen screen, IMouse mouse, Vector2 position, Vector2 dimensions, String text)
        {
            Screen = screen;
            Mouse = mouse;
            mouse.MouseReleased += HandleClick;
            mouse.UpdateIsMoused += UpdateIsMoused;
            Position = position;
            Dimensions = dimensions;
            MouseCollider = new RectangleCollider(position, dimensions);
            Text = text;
        }
        protected IScreen Screen { get; }
        protected IMouse Mouse { get; }
        protected String Text { get; set; }
        protected RectangleCollider MouseCollider { get; set; }
        public delegate void Click();
        public event Click? Clicked;
        public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
        public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
        public Vector2 Position { get; set; }
        public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
        public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
        public Vector2 Dimensions { get; set; }
        public void Render()
        {
            Color boxColor = IsMoused ? new Color(255, 255, 255, 125) : new Color(255, 255, 255, 55);
            Screen.DrawRectangle(Position, Dimensions, boxColor);

            Screen.DrawText(Text, Color.LIGHTGRAY, Position, 20, 1);
        }
        protected void UpdateIsMoused(bool mouseBlocked)
        {
            Vector2 mousePosition = Mouse.Position;
            IsMoused = (mouseBlocked == false) && MouseCollider.Collides(mousePosition);
            if (IsMoused)
                Mouse.Block();
        }
        protected bool IsMoused { get; set; }
        public void HandleClick(IMouse mouse, MouseButton button)
        {
            if (button == MouseButton.MOUSE_LEFT_BUTTON && IsMoused)
            {
                Clicked?.Invoke();
                mouse.Block();
            }
        }
    }
}