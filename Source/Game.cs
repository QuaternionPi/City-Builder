using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Game
    {
        public Game() { Map = new Map(); }
        public Game(int x, int y)
        {
            Map = new Map(x, y);
            Initialize();
        }
        [JsonInclude]
        public Map Map { get; private set; }
        protected event EventHandler? Render;
        protected event EventHandler? Update;
        protected event EventHandler? LeftClick;
        protected event EventHandler? RightClick;
        public void Initialize()
        {
            Map.Initialize(ref Render, ref Update, ref LeftClick, ref RightClick);
        }
        public void UpdateCycle()
        {
            Update?.Invoke(this, EventArgs.Empty);
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
                LeftClick?.Invoke(this, EventArgs.Empty);
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_RIGHT))
                RightClick?.Invoke(this, EventArgs.Empty);
        }
        public void RenderCycle()
        {
            Render?.Invoke(this, EventArgs.Empty);
        }
        public static Game LoadGame(String json)
        {
            Game game = JsonSerializer.Deserialize<Game>(json) ?? throw new Exception();
            game.Initialize();
            return game;
        }
    }
}