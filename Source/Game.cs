using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Game
    {
        public Game()
        {
            Map = new Map();
            GUIManager = new(5);
        }
        public Game(int x, int y)
        {
            Map = new Map(x, y);
            GUIManager = new(5);
            Initialize();
        }
        [JsonInclude]
        public Map Map { get; private set; }
        public GUIManager GUIManager { get; private set; }
        public void Initialize()
        {
            Map.Initialize(GUIManager);
            Button setWater = new(new(150, 50), new(250, 40), "Water");
            Button setFlat = new(new(150, 100), new(250, 40), "Flat");
            Button setHills = new(new(150, 150), new(250, 40), "Hills");
            setWater.Initialize(GUIManager);
            setFlat.Initialize(GUIManager);
            setHills.Initialize(GUIManager);
        }
        public void RenderCycle()
        {
            GUIManager.RenderCycle();
        }
        public void UpdateCycle()
        {
            GUIManager.UpdateCycle();
        }
        public void ClickCycle()
        {
            GUIManager.ClickCycle();
        }
        public static Game LoadGame(String json)
        {
            Game game = JsonSerializer.Deserialize<Game>(json) ?? throw new Exception();
            game.Initialize();
            return game;
        }
    }
}