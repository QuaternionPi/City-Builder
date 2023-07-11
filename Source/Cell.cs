using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Cell : IRectangle
    {
        public Cell()
        {
            TopTile = new Tile();
            RightTile = new Tile();
            BottomTile = new Tile();
            LeftTile = new Tile();
        }
        [JsonInclude]
        public Tile TopTile { get; set; }
        [JsonInclude]
        public Tile RightTile { get; set; }
        [JsonInclude]
        public Tile BottomTile { get; set; }
        [JsonInclude]
        public Tile LeftTile { get; set; }
        public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
        public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
        public Vector2 Position { get; set; }
        public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
        public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
        public Vector2 Dimensions { get; set; }
        public void Initialize(
            ref EventHandler? Render,
            ref EventHandler? Update,
            ref EventHandler? LeftClick,
            ref EventHandler? RightClick,
            Vector2 position,
            Vector2 dimensions)
        {
            Position += position;
            Dimensions += dimensions;
            Vector2 topLeft = Position - Dimensions / 2;
            Vector2 topRight = Position + new Vector2(Width / 2, -Height / 2);
            Vector2 bottomRight = Position + Dimensions / 2;
            Vector2 bottomLeft = Position + new Vector2(-Width / 2, Height / 2);
            TopTile.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick, Position, topRight, topLeft);
            RightTile.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick, Position, bottomRight, topRight);
            BottomTile.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick, Position, bottomLeft, bottomRight);
            LeftTile.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick, Position, topLeft, bottomLeft);
            TopTile.LeftClick += PrintSomething;
            RightTile.LeftClick += PrintSomething;
            BottomTile.LeftClick += PrintSomething;
            LeftTile.LeftClick += PrintSomething;
        }
        protected void PrintSomething(object? sender, EventArgs eventArgs)
        {
            Console.WriteLine("Something");
        }
    }
}