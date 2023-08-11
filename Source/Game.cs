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
            MapMouse = new RaylibMouse();
            Map = new Map(screen, MapMouse, x, y);
            Camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);

            Button setWater = new(screen, ButtonMouse, new(150, 50), new(250, 40), "Water");
            Button setFlat = new(screen, ButtonMouse, new(150, 100), new(250, 40), "Flat");
            Button setHills = new(screen, ButtonMouse, new(150, 150), new(250, 40), "Hills");
            Button setDrawLine = new(screen, ButtonMouse, new(150, 200), new(250, 40), "Draw Line");

            setWater.Clicked += SetMapPaintTerrainWater;
            setFlat.Clicked += SetMapPaintTerrainFlat;
            setHills.Clicked += SetMapPaintTerrainHills;
            setDrawLine.Clicked += SetMapDrawLine;
            Buttons = new List<Button>() { setWater, setFlat, setHills, setDrawLine };
        }
        protected IScreen Screen { get; }
        protected IMouse ButtonMouse { get; }
        protected IMouse MapMouse { get; }
        protected ICameraControler2D CameraControler { get; }
        protected Camera2D Camera;
        [JsonInclude]
        protected Map Map { get; }
        protected List<Button> Buttons { get; }
        public static Game LoadGame(String json)
        {
            Game game = JsonSerializer.Deserialize<Game>(json) ?? throw new Exception();
            return game;
        }
        public void Update(bool mouseBlocked)
        {
            bool mouseBlockedByButton = ButtonMouse.IsAnythingMoused(mouseBlocked);
            if (mouseBlocked == false)
                ButtonMouse.CheckForClick();

            bool mouseBlockedByMap = MapMouse.IsAnythingMoused(mouseBlocked || mouseBlockedByButton);
            if ((mouseBlocked || mouseBlockedByButton) == false)
                MapMouse.CheckForClick();

        }
        public void Render()
        {
            Camera.target += CameraControler.Direction * 10;
            Raylib.BeginMode2D(Camera);
            Map.Render();
            Raylib.EndMode2D();
            foreach (Button button in Buttons)
            {
                button.Render();
            }
        }
        public void SetMapPaintTerrainWater()
        {
            Map.PaintTerrain = Terrain.Water;
        }
        public void SetMapPaintTerrainFlat()
        {
            Map.PaintTerrain = Terrain.Flat;
        }
        public void SetMapPaintTerrainHills()
        {
            Map.PaintTerrain = Terrain.Hills;
        }
        public void SetMapDrawLine()
        {
            Map.Mode = MapMode.DrawLine;
        }
    }
}