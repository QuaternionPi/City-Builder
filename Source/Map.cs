using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public enum MapMode { PaintTerrain, DrawLine }
    public class Map
    {
        public Map()
        {
            Cells = new Cell[1][];
            Cells[0] = new Cell[1];
            PaintTerrain = Terrain.Flat;
            Line = new Line();
        }
        public Map(int x, int y)
        {
            Cells = new Cell[x][];
            for (int i = 0; i < x; i++)
            {
                Cells[i] = new Cell[y];
                for (int j = 0; j < y; j++)
                {
                    Cells[i][j] = new Cell();
                }
            }
            Line = new Line();
        }
        public static readonly int CellSize = 40;
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        public MapMode Mode { get; set; }
        protected Terrain Terrain;
        public Terrain PaintTerrain { get { return Terrain; } set { Terrain = value; Mode = MapMode.PaintTerrain; } }
        protected Line Line;
        public void Initialize(GUIManager guiManager)
        {
            for (int i = 0; i < Cells.Length; i++)
                for (int j = 0; j < Cells[i].Length; j++)
                {
                    Cell cell = Cells[i][j];
                    int x = (int)(CellSize * (i + 0.5));
                    int y = (int)(CellSize * (j + 0.5));
                    Vector2 position = new(x, y);
                    Vector2 dimensions = new(CellSize, CellSize);
                    cell.Initialize(guiManager, position, dimensions);
                    cell.TileLeftClicked += TileLeftClick;
                    cell.TileLeftDragged += TileLeftDrag;
                }
            Line.Initialize(guiManager);
        }
        protected void TileLeftClick(object? sender, TileClickedArgs args)
        {
            if (Mode == MapMode.DrawLine)
                Line.StartPosition = Raylib.GetMousePosition();
        }
        protected void TileLeftDrag(object? sender, TileClickedArgs args)
        {
            if (Mode == MapMode.PaintTerrain)
                Paint(args.Tile);
            else if (Mode == MapMode.DrawLine)
                DrawLine();
        }
        protected void Paint(Tile tile)
        {
            tile.ChangeTerrain(PaintTerrain, Raylib.GetMousePosition());
        }
        protected void DrawLine()
        {
            Line.EndPosition = Raylib.GetMousePosition();
        }
    }
}