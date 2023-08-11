using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    namespace Transit
    {
        public interface IRoute<TVehicle, TStop> : IRenderable
            where TVehicle : IVehicle
            where TStop : IStop
        {
            public String Name { get; }
        }
        public interface IVehicle : IRenderable, IPosition
        {

        }
        public interface IStop : IRenderable, IPosition
        {

        }
        public class TrainLine : IRoute<Train, TrainStation>
        {
            public TrainLine(IScreen screen, String name, List<TrainStation> stations)
            {
                Screen = screen;
                Name = name;
                Trains = new List<Train>();
                Stations = stations;
                List<Vector2> controlPoints = (from Station in stations select Station.Position).ToList<Vector2>();
                Spline = new Spline(screen, controlPoints);
                Spline.Style = new Spline.Cardinal(Spline, Color.DARKBLUE, 3, 0.2f);
            }
            public IScreen Screen;
            public String Name { get; }
            protected List<Train> Trains { get; }
            protected List<TrainStation> Stations { get; }
            protected Spline Spline;
            public void Render()
            {
                Spline.Render();
                foreach (Train train in Trains)
                {
                    train.Render();
                }
                foreach (TrainStation station in Stations)
                {
                    station.Render();
                }
            }
        }
        public class Train : IVehicle
        {
            public Train(IScreen screen, Vector2 position, Vector2 dimensions)
            {
                Screen = screen;
                Position = position;
                Dimensions = dimensions;
            }
            protected IScreen Screen;
            public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
            public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
            public Vector2 Position { get; set; }
            public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
            public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
            public Vector2 Dimensions { get; set; }
            public void Render()
            {
                Screen.DrawRectangle(Position, Dimensions, Color.DARKBLUE);
            }
        }
        public class TrainStation : IStop
        {
            public TrainStation(IScreen screen, Vector2 position, Vector2 dimensions)
            {
                Screen = screen;
                Position = position;
                Dimensions = dimensions;
            }
            protected IScreen Screen;
            public float X { get { return Position.X; } protected set { Position = new(value, Position.Y); } }
            public float Y { get { return Position.Y; } protected set { Position = new(Position.X, value); } }
            public Vector2 Position { get; set; }
            public float Width { get { return Dimensions.X; } protected set { Dimensions = new(value, Dimensions.Y); } }
            public float Height { get { return Dimensions.Y; } protected set { Dimensions = new(Dimensions.X, value); } }
            public Vector2 Dimensions { get; set; }
            public void Render()
            {
                Screen.DrawRectangle(Position, Dimensions, Color.MAROON);
            }
        }
    }
}