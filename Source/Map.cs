using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public enum MapMode { PaintTerrain, DrawLine }
    public class Map : IRenderable
    {
        public Map(IScreen screen, IMouse mouse, int x, int y)
        {
            Screen = screen;
            Mouse = mouse;
            Cells = new Cell[x][];
            Vector2 cellDimensions = new(CellSize, CellSize);
            for (int i = 0; i < x; i++)
            {
                Cells[i] = new Cell[y];
                for (int j = 0; j < y; j++)
                {
                    Vector2 position = new Vector2(i, j) * CellSize + cellDimensions / 2;
                    Cell cell = new Cell(screen, mouse, position, cellDimensions);
                    Cells[i][j] = cell;
                    cell.Selected += HandleTileSelect;
                }
            }
            List<Vector2> controlPoints = new() {
                new(100, 100),
                new(100, 200),
                new(200, 300),
                new(200, 400),
                new(300, 450),
                new(400, 450),
                new(500, 350)
                };
            Spline = new(screen, controlPoints);
            Spline.Style = new Spline.Cardinal(Spline, Color.DARKGRAY, 3, 0.05f);
        }
        protected IScreen Screen { get; }
        protected IMouse Mouse { get; }
        protected Spline Spline { get; }
        public static readonly int CellSize = 40;
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        public MapMode Mode { get; set; }
        protected Terrain Terrain;
        public Terrain PaintTerrain { get { return Terrain; } set { Terrain = value; Mode = MapMode.PaintTerrain; } }
        public void Render()
        {
            foreach (var row in Cells)
            {
                foreach (var cell in row)
                {
                    cell.Render();
                }
            }
            Spline.Render();
        }
        protected void HandleTileSelect(Cell cell, Tile tile)
        {
            if (Mode == MapMode.PaintTerrain)
                Paint(tile);
        }
        protected void Paint(Tile tile)
        {
            tile.ChangeTerrain(PaintTerrain, Raylib.GetMousePosition());
        }
    }
    public interface IMapTool
    {
        public void LeftClick(Map map, Cell cell, Tile tile);
        public void RightClick(Map map, Cell cell, Tile tile);
    }
    public class MapPainter : IMapTool
    {
        private MapPainter()
        {

        }
        public MapPainter SingleTilePainter()
        {
            return new MapPainter();
        }
        public void LeftClick(Map map, Cell cell, Tile tile)
        {

        }
        public void RightClick(Map map, Cell cell, Tile tile)
        {

        }
    }
}