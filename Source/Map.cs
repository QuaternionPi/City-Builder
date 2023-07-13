using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public enum MapMode { PaintTerrain, DrawLine }
    public class Map : IGUIElement, IRenderable
    {
        public Map()
        {
            Cells = new Cell[1][];
            Cells[0] = new Cell[1];
            PaintTerrain = Terrain.Flat;
            LineSegment = new LineSegment();
            Line = new Line<LineSegment>();
            Shown = true;
            GUILayer = 1;
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
            LineSegment = new LineSegment();
            Line = new Line<LineSegment>();
            Shown = true;
            GUILayer = 4;
        }
        public GUIManager? GUIManager { get; set; }
        public int GUILayer { get; }
        public bool Shown { get; }
        public static readonly int CellSize = 40;
        [JsonInclude]
        public Cell[][] Cells { get; private set; }
        public MapMode Mode { get; set; }
        protected Terrain Terrain;
        public Terrain PaintTerrain { get { return Terrain; } set { Terrain = value; Mode = MapMode.PaintTerrain; } }
        protected LineSegment LineSegment;
        protected Line<LineSegment> Line;
        public void Render()
        {
            LineSegment.EndPosition = Raylib.GetMousePosition();
            if (LineSegment.StartPosition != Vector2.Zero)
                LineSegment.Render();
        }
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
            this.AttachGUI(guiManager);
        }
        protected void TileLeftClick(object? sender, TileClickedArgs args)
        {
            if (sender is not Cell cell) throw new Exception();
            if (Mode == MapMode.DrawLine)
                DrawLine(cell.Position);
        }
        protected void TileLeftDrag(object? sender, TileClickedArgs args)
        {
            if (Mode == MapMode.PaintTerrain)
                Paint(args.Tile);
        }
        protected void Paint(Tile tile)
        {
            tile.ChangeTerrain(PaintTerrain, Raylib.GetMousePosition());
        }
        protected void DrawLine(Vector2 position)
        {
            if ((Line.Connects(LineSegment) == false) || LineSegment.StartPosition == Vector2.Zero)
            {
                LineSegment = new LineSegment { StartPosition = position };
            }
            else
            {
                LineSegment.EndPosition = position;
                if (LineSegment.Length < 1.5 * CellSize)
                {
                    Line.AddLine(LineSegment);
                    LineSegment = new LineSegment();
                }
            }
        }
    }
    public interface IMapTool
    {
        public void LeftClick(Map map, Cell cell, Tile tile);
        public void RightClick(Map map, Cell cell, Tile tile);
        public void LeftDrag(Map map, Cell cell, Tile tile);
        public void RightDrag(Map map, Cell cell, Tile tile);
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
        public void LeftDrag(Map map, Cell cell, Tile tile)
        {

        }
        public void RightDrag(Map map, Cell cell, Tile tile)
        {

        }
    }
}