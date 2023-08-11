using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public enum Terrain { Water = 0, Flat, Hills }
    public class Tile : IRenderable
    {
        public Tile(IScreen screen, IMouse mouse, Vector2[] points)
        {
            Screen = screen;
            Mouse = mouse;
            Points = points;
            MouseCollider = new TriangleCollider(points[0], points[1], points[2]);
            mouse.MouseReleased += HandleClick;
            mouse.UpdateIsMoused += UpdateIsMoused;
        }
        protected IScreen Screen { get; }
        protected IMouse Mouse { get; }
        protected Vector2[] Points { get; }
        protected TriangleCollider MouseCollider;
        [JsonInclude]
        [JsonConverter(typeof(TerrainJsonConverter))]
        public Terrain Terrain { get; protected set; }
        public delegate void Select(Tile sender);
        public event Select? Selected;
        public float X { get { return Position.X; } }
        public float Y { get { return Position.Y; } }
        public Vector2 Position
        {
            get { return (Points[1] + Points[2] + Points[3]) / 3; }
        }
        public void Render()
        {
            Color drawColor = IsMoused ? Color.RED : Terrain switch
            {
                Terrain.Water => Color.BLUE,
                Terrain.Flat => Color.GREEN,
                Terrain.Hills => Color.DARKGREEN,
                _ => throw new Exception(),
            };
            Screen.DrawTriangle(Points, drawColor);
        }
        protected void HandleClick(IMouse mouse, MouseButton button)
        {
            if (button == MouseButton.MOUSE_LEFT_BUTTON && IsMoused)
            {
                Selected?.Invoke(this);
                mouse.Block();
            }
        }
        protected void UpdateIsMoused(bool mouseBlocked)
        {
            Vector2 mousePosition = Mouse.Position;
            IsMoused = (mouseBlocked == false) && MouseCollider.Collides(mousePosition);
            if (IsMoused)
                Mouse.Block();
        }
        protected bool IsMoused { get; set; }
        public void ChangeTerrain(Terrain terrain, ICollider collider)
        {
            if (MouseCollider.Collides(collider))
                ChangeTerrain(terrain);
        }
        public void ChangeTerrain(Terrain terrain)
        {
            Terrain = terrain;
        }
    }
    public static class TerrainExtensions
    {
        public static Terrain ToTerrain(this String text) => text switch
        {
            "Water" => Terrain.Water,
            "Flat" => Terrain.Flat,
            "Hills" => Terrain.Hills,
            _ => throw new ArgumentException($"There is no Terrain named: {text}"),
        };
        public static String ToString(this Terrain terrain) => terrain switch
        {
            Terrain.Water => "Water",
            Terrain.Flat => "Flat",
            Terrain.Hills => "Hills",
            _ => throw new ArgumentException($"There is no Terrain: {terrain}"),
        };
    }
}