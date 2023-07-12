using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Map
    {
        public Map()
        {
            Cells = new Cell[1][];
            Cells[0] = new Cell[1];
            PaintTerrain = Terrain.Flat;
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
        }
        public static readonly int CellSize = 40;
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        public Terrain PaintTerrain { get; set; }
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
                    cell.TileLeftDragged += Paint;
                }
        }
        protected void Paint(object? sender, TileClickedArgs tileClickedArgs)
        {
            Tile tile = tileClickedArgs.Tile;
            tile.ChangeTerrain(PaintTerrain, Raylib.GetMousePosition());
        }
    }
}