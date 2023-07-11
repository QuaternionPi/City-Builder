using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public enum Terrain { Water = 0, Flat, Hills }
    public class Tile : IRenderable, IUpdatable, IClickable, ITriangle
    {
        public Tile()
        {
            Shown = true;
            Active = true;
        }
        [JsonInclude]
        [JsonConverter(typeof(TerrainJsonConverter))]
        public Terrain Terrain { get; protected set; }
        public bool Shown { get; }
        public bool Active { get; }
        protected TriangleCollider MouseCollider;
        public event EventHandler? LeftClick;
        public event EventHandler? RightClick;
        public float X { get { return Position.X; } private set { Position = new(value, Position.Y); } }
        public float Y { get { return Position.Y; } private set { Position = new(Position.X, value); } }
        public Vector2 Position
        {
            get { return (Point1 + Point2 + Point3) / 3; }
            private set
            {
                Vector2 delta = value - Position;
                Point1 += delta;
                Point2 += delta;
                Point3 += delta;
            }
        }
        public Vector2 Point1 { get { return MouseCollider.Point1; } private set { MouseCollider = new TriangleCollider(value, Point2, Point3); } }
        public Vector2 Point2 { get { return MouseCollider.Point2; } private set { MouseCollider = new TriangleCollider(Point1, value, Point3); } }
        public Vector2 Point3 { get { return MouseCollider.Point3; } private set { MouseCollider = new TriangleCollider(Point1, Point2, value); } }
        public void Render()
        {
            Color drawColor = IsMoused() ? Color.RED : Terrain switch
            {
                Terrain.Water => Color.BLUE,
                Terrain.Flat => Color.GREEN,
                Terrain.Hills => Color.DARKGREEN,
                _ => throw new Exception(),
            };
            Raylib.DrawTriangle(Point1, Point2, Point3, drawColor);
        }
        public void Update()
        {

        }
        public bool IsMoused()
        {
            Vector2 mousePosition = Raylib.GetMousePosition();
            return MouseCollider.Collides(mousePosition);
        }
        public void OnLeftClick()
        {
            Terrain = Terrain.Flat;
            LeftClick?.Invoke(this, EventArgs.Empty);
        }
        public void OnRightClick()
        {
            RightClick?.Invoke(this, EventArgs.Empty);
        }
        public void Initialize(GUIManager guiManager, Vector2 point1, Vector2 point2, Vector2 point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            guiManager.Attach(this, 1);
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