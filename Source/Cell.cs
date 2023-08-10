using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Cell : IRectangle, IRenderable
    {
        public Cell(IScreen screen, IMouse mouse, Vector2 position, Vector2 dimensions)
        {
            Screen = screen;
            Mouse = mouse;
            Vector2 topLeft = position - dimensions / 2;
            Vector2 bottomRight = position + dimensions / 2;
            Vector2 topRight = position + new Vector2(dimensions.X, -dimensions.Y) / 2;
            Vector2 bottomLeft = position + new Vector2(-dimensions.X, dimensions.Y) / 2;

            Tile top = new(screen, mouse, new[] { position, topRight, topLeft });
            Tile right = new(screen, mouse, new[] { position, bottomRight, topRight });
            Tile bottom = new(screen, mouse, new[] { position, bottomLeft, bottomRight });
            Tile left = new(screen, mouse, new[] { position, topLeft, bottomLeft });
            Tiles = new Tile[4] { top, right, bottom, left };
            foreach (Tile tile in Tiles)
            {
                tile.Selected += HandleTileSelect;
            }
        }
        protected IScreen Screen;
        protected IMouse Mouse;
        public Tile[] Tiles;
        public delegate void Select(Cell cell, Tile tile);
        public event Select? Selected;
        public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
        public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
        public Vector2 Position { get; set; }
        public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
        public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
        public Vector2 Dimensions { get; set; }
        public void Render()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Render();
            }
        }
        public void HandleTileSelect(Tile tile)
        {
            Selected?.Invoke(this, tile);
        }
    }
}