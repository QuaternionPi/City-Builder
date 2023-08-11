using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Game : IRenderable
    {
        public Game(IScreen screen, ICameraControler2D cameraControler, int x, int y)
        {
            Screen = screen;
            CameraControler = cameraControler;
            ButtonMouse = new RaylibMouse();
            MapMouse = new CameraOffsetMouse(new RaylibMouse());
            Map = new Map(screen, MapMouse, x, y);
            Camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
            Tool = new Selector(Screen, this);

            Map.Selected += HandleTileSelect;
        }
        protected IScreen Screen { get; }
        protected IMouse ButtonMouse { get; set; }
        protected CameraOffsetMouse MapMouse { get; }
        protected ICameraControler2D CameraControler { get; }
        protected Camera2D Camera;
        protected MapTool Tool;
        [JsonInclude]
        protected Map Map { get; }
        public static Game LoadGame(String json)
        {
            Game game = JsonSerializer.Deserialize<Game>(json) ?? throw new Exception();
            return game;
        }
        public void Update(bool mouseBlocked)
        {
            MapMouse.UpdatePosition(Camera);
            bool mouseBlockedByButton = ButtonMouse.IsAnythingMoused(mouseBlocked);
            if (mouseBlocked == false)
                ButtonMouse.CheckForClick();

            bool mouseBlockedByMap = MapMouse.IsAnythingMoused(mouseBlocked || mouseBlockedByButton);
            if ((mouseBlocked || mouseBlockedByButton) == false)
                MapMouse.CheckForClick();

            Tool.Update();
        }
        public void Render()
        {
            Camera.target += CameraControler.Direction * 10;
            Raylib.BeginMode2D(Camera);
            Map.Render();
            Raylib.EndMode2D();
            Tool.Render();
        }
        public void HandleTileSelect(Map map, Cell cell, Tile tile) => Tool.HandleTileSelect(map, cell, tile);
        protected abstract class MapTool
        {
            public MapTool(IScreen screen, Game game)
            {
                Screen = screen;
                Game = game;
                Buttons = new List<Button>();
            }
            protected IScreen Screen { get; }
            protected Game Game { get; }
            protected List<Button> Buttons { get; set; }
            public abstract void Update();
            public abstract void Render();
            public abstract void HandleTileSelect(Map map, Cell cell, Tile tile);
        }
        protected class Selector : MapTool
        {
            public Selector(IScreen screen, Game game) : base(screen, game)
            {
                Game.ButtonMouse = new RaylibMouse();
                Vector2 dimension = new(240, 40);
                Button mapPainter = new(screen, Game.ButtonMouse, new(240, 100), dimension, "Map Painter");
                Button trainLineCreater = new(screen, Game.ButtonMouse, new(240, 160), dimension, "Train Line Creater");
                Buttons = new List<Button>() { mapPainter, trainLineCreater };
                mapPainter.Clicked += SelectMapPainter;
                trainLineCreater.Clicked += SelectTrainLineCreater;
            }
            public override void Update() { }
            public override void Render()
            {
                foreach (Button button in Buttons)
                {
                    button.Render();
                }
                Game.Screen.DrawText("Selector Mode", Color.BLACK, new(400, 50), 30, 2);
            }
            public override void HandleTileSelect(Map map, Cell cell, Tile tile)
            {

            }
            protected void SelectMapPainter()
            {
                Game.Tool = new MapPainter(Screen, Game);
            }
            protected void SelectTrainLineCreater()
            {
                Game.Tool = new TrainLineCreater(Screen, Game);
            }
        }
        protected class MapPainter : MapTool
        {
            public MapPainter(IScreen screen, Game game) : base(screen, game)
            {

                Game.ButtonMouse = new RaylibMouse();
                Vector2 dimension = new Vector2(240, 40);
                Button water = new(screen, Game.ButtonMouse, new(240, 100), dimension, "Water");
                Button land = new(screen, Game.ButtonMouse, new(240, 160), dimension, "Land");
                Button hills = new(screen, Game.ButtonMouse, new(240, 220), dimension, "Hills");
                Button selector = new(screen, Game.ButtonMouse, new(240, 280), dimension, "Selector");
                Buttons = new List<Button>() { water, land, hills, selector };
                water.Clicked += PaintWater;
                land.Clicked += PaintLand;
                hills.Clicked += PaintHills;
                selector.Clicked += SelectSelector;
            }
            public override void Update() { }
            public override void Render()
            {
                foreach (Button button in Buttons)
                {
                    button.Render();
                }
                Game.Screen.DrawText("Map Painter Mode", Color.BLACK, new(400, 50), 30, 2);
            }
            public override void HandleTileSelect(Map map, Cell cell, Tile tile)
            {
                map.Paint(tile);
            }
            protected void PaintWater()
            {
                Game.Map.PaintTerrain = Terrain.Water;
            }
            protected void PaintLand()
            {
                Game.Map.PaintTerrain = Terrain.Flat;
            }
            protected void PaintHills()
            {
                Game.Map.PaintTerrain = Terrain.Hills;
            }
            protected void SelectSelector()
            {
                Game.Tool = new Selector(Screen, Game);
            }
        }
        protected class TrainLineCreater : MapTool
        {
            public TrainLineCreater(IScreen screen, Game game) : base(screen, game)
            {
                Game.ButtonMouse = new RaylibMouse();
            }
            public override void Update() { }
            public override void Render()
            {
                foreach (Button button in Buttons)
                {
                    button.Render();
                }
                Game.Screen.DrawText("Train Line Creater Mode", Color.BLACK, new(400, 50), 30, 2);
            }
            public override void HandleTileSelect(Map map, Cell cell, Tile tile)
            {
                map.AddTrainStation(cell.Position);
            }
        }
    }
}