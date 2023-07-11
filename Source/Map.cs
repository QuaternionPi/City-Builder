using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Map
    {
        public Map() { Cells = new Cell[1][]; Cells[0] = new Cell[1]; }
        public static readonly int CellSize = 20;
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        public void Initialize(
            ref EventHandler? Render,
            ref EventHandler? Update,
            ref EventHandler? LeftClick,
            ref EventHandler? RightClick)
        {
            for (int i = 0; i < Cells.Length; i++) for (int j = 0; j < Cells[i].Length; j++)
                {
                    Cell cell = Cells[i][j];
                    Vector2 position = new(CellSize * i, CellSize * j);
                    Vector2 dimensions = new(CellSize, CellSize);
                    cell.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick, position, dimensions);
                }
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
    }
}