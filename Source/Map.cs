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

            Vector2 stationDimensions = new(20, 10);
            List<Transit.TrainStation> stations = new() {
                new(screen, new(100, 100), stationDimensions),
                new(screen, new(100, 200), stationDimensions),
                new(screen, new(200, 300), stationDimensions),
                new(screen, new(200, 400), stationDimensions),
                new(screen, new(300, 450), stationDimensions),
                new(screen, new(400, 450), stationDimensions),
                new(screen, new(500, 350), stationDimensions),
                };
            TrainLine = new(screen, "Expo Line", stations);
        }
        protected IScreen Screen { get; }
        protected IMouse Mouse { get; }
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        protected Transit.TrainLine TrainLine { get; }
        public delegate void Select(Map map, Cell cell, Tile tile);
        public event Select? Selected;
        public Terrain PaintTerrain;
        public static readonly int CellSize = 40;
        public void Render()
        {
            foreach (var row in Cells)
            {
                foreach (var cell in row)
                {
                    cell.Render();
                }
            }
            TrainLine.Render();
        }
        protected void HandleTileSelect(Cell cell, Tile tile)
        {
            Selected?.Invoke(this, cell, tile);
        }
        public void Paint(Tile tile)
        {
            tile.ChangeTerrain(PaintTerrain);
        }
        public void AddTrainStation(Vector2 position)
        {
            Vector2 stationDimensions = new(20, 10);
            Transit.TrainStation station = new(Screen, position, stationDimensions);
            TrainLine.AddTrainStation(station);
        }
    }
}